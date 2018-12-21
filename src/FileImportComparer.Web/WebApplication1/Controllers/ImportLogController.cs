using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace WebApplication1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ImportLogController : ControllerBase {
        private readonly EnvironmentsConfiguration _config;
        private readonly IMemoryCache _cache;

        public ImportLogController(EnvironmentsConfiguration config, IMemoryCache cache) {
            _config = config;
            _cache = cache;
        }
        [HttpGet("{environment}")]
        public ActionResult<TodoLogsResult> GetTodos(string environment) {
            var env = _config.Environments.FirstOrDefault(x => x.Name == environment);
            if (env == null) {
                return NotFound(new ProblemDetails {
                    Title = "Environment not found",
                    Detail = $"Missing environment: {environment}"
                });
            }

            if (!_cache.TryGetValue(CacheKeys.GetTodos(env.Name), out TodoLogsResult cacheEntry))
            {

                try {
                    using (var conn = new SqlConnection(env.ConnectionString)) {
                        var result = conn.Query<TodoLog>(ImportLogSql.TodoSql(50));
                        cacheEntry = new TodoLogsResult { Items = result.Select(Transformers.TransformTodoLog).ToArray() };

                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                            // Keep in cache for this time, reset time if accessed.
                            .SetSlidingExpiration(TimeSpan.FromSeconds(5));

                        // Save data in cache.
                        _cache.Set(CacheKeys.GetTodos(env.Name), cacheEntry, cacheEntryOptions);

                    }
                }
                catch (Exception e) {
                    return StatusCode(500, new ProblemDetails {
                        Title = "InternalServerError",
                        Detail = e.Message
                    });
                }

                // Set cache options.
            }

            return Ok(cacheEntry);
        }

        [HttpGet("{environment}/{todoId}")]
        public ActionResult<TodoLogsResult> GetTodosByTodoId(string environment, int todoId) {
            var env = _config.Environments.FirstOrDefault(x => x.Name == environment);
            if (env == null)
                return NotFound(new ProblemDetails {
                    Title = "Environment not found",
                    Detail = $"Missing environment: {environment}"
                });

            try {
                using (var conn = new SqlConnection(env.ConnectionString)) {
                    var result = conn.Query<TodoLog>(ImportLogSql.TodoByTodoId, new {todoId});
                    return Ok(new TodoLogsResult { Items = result.Select(Transformers.TransformTodoLog).ToArray() });
                }
            }
            catch (Exception e) {
                return StatusCode(500, new ProblemDetails {
                    Title = "InternalServerError",
                    Detail = e.Message
                });
            }
        }


        [HttpGet("detail/{environment}/{todoId}")]
        public ActionResult<TodoDetailsResult> GetDetail(string environment, int todoId) {
            var env = _config.Environments.FirstOrDefault(x => x.Name == environment);
            if (env == null)
                return NotFound(new ProblemDetails {
                    Title = "Environment not found",
                    Detail = $"Missing environment: {environment}"
                });

            try {
                using (var conn = new SqlConnection(env.ConnectionString)) {
                    var result = conn.Query<TodoLogDetail>(ImportLogSql.TodoLogSql, new { todoId });
                    return Ok(new TodoDetailsResult { Items = result.Select(Transformers.TransformTodoLogDetail).ToArray() });
                }
            }
            catch (Exception e) {
                return StatusCode(500, new ProblemDetails {
                    Title = "InternalServerError",
                    Detail = e.Message
                });
            }
        }
    }
}
