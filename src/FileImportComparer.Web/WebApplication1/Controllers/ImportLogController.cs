using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace WebApplication1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ImportLogController : ControllerBase {
        private readonly EnvironmentsConfiguration _config;
        private Func<int, string> _todoSql = count => $@"
SELECT TOP {count}  
	t.TodoID,
	t.TodoName ,
	tf.PartnerID, 
	p.PartnerName, 
	t.StartDate ,
	t.EndDate ,
	t.CreatedBy ,
	t.CreatedDate ,
	tl.TodoLogID ,
	tl.MessageID ,
	tl.TotalNumberOfRows ,
	tl.TotalNumberOfCommands ,
	tl.TotalNumberOfErrors ,
	tl.TotalNumberOfDuplicates, 
	tf.FileID AS FileId,
	ISNULL(tf.RealFileName, t.TodoName) AS 'FileName',
	ISNULL(tld.NumberOfEvents, 0) AS 'TotalNumberOfEvents'
 FROM dbo.tTodoLog tl 
 INNER JOIN dbo.tTodo AS t ON tl.TodoID = t.TodoID 
 LEFT JOIN dbo.tTodoFile AS tf ON t.TodoID = tf.TodoID 
 INNER JOIN dbo.tPartner AS p ON p.PartnerID = tf.PartnerID
 LEFT JOIN (SELECT TodoLogID, SUM(NumberOfRows) AS 'NumberOfEvents' 
				FROM dbo.tTodoLogDetail
				GROUP BY TodoLogID) tld ON tl.TodoLogID = tld.TodoLogID
ORDER BY tl.TodoLogID DESC;
";
        private const string _todoByTodoId = @"
SELECT
	t.TodoID,
	t.TodoName ,
	tf.PartnerID, 
	p.PartnerName, 
	t.StartDate ,
	t.EndDate ,
	t.CreatedBy ,
	t.CreatedDate ,
	tl.TodoLogID ,
	tl.MessageID ,
	tl.TotalNumberOfRows ,
	tl.TotalNumberOfCommands ,
	tl.TotalNumberOfErrors ,
	tl.TotalNumberOfDuplicates, 
	ISNULL(tf.RealFileName, t.TodoName) AS 'FileName' 
 FROM dbo.tTodoLog tl 
 INNER JOIN dbo.tTodo AS t ON tl.TodoID = t.TodoID 
 LEFT JOIN dbo.tTodoFile AS tf ON t.TodoID = tf.TodoID 
 INNER JOIN dbo.tPartner AS p ON p.PartnerID = tf.PartnerID
 WHERE t.TodoId = @todoId
ORDER BY tl.TodoLogID DESC;";

        private const string _todoLogSql = @"
select tld.* 
from dbo.tTodoLog tl
join dbo.tTodoLogDetail tld ON tl.TodoLogID = tld.TodoLogID
where tl.TodoID = @todoId
order by tld.EventTypeID";

        public ImportLogController(EnvironmentsConfiguration config) {
            _config = config;
        }
        [HttpGet("{environment}")]
        public ActionResult<TodoLogsResult> GetTodos(string environment) {
            var env = _config.Environments.FirstOrDefault(x => x.Name == environment);
            if (env == null)
                return NotFound(new ProblemDetails {
                    Title = "Environment not found",
                    Detail = $"Missing environment: {environment}"
                });

            try {
                using (var conn = new SqlConnection(env.ConnectionString)) {
                    var result = conn.Query<TodoLog>(_todoSql(50));
                    return Ok(new TodoLogsResult { Items = result.Select(TransformTodoLog).ToArray() });
                }
            }
            catch (Exception e) {
                return StatusCode(500, new ProblemDetails {
                    Title = "InternalServerError",
                    Detail = e.Message
                });
            }
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
                    var result = conn.Query<TodoLog>(_todoByTodoId, new {todoId});
                    return Ok(new TodoLogsResult { Items = result.Select(TransformTodoLog).ToArray() });
                }
            }
            catch (Exception e) {
                return StatusCode(500, new ProblemDetails {
                    Title = "InternalServerError",
                    Detail = e.Message
                });
            }
        }

        private string CreateId(TodoLog log) {
            var date = new DateTime(log.StartDate.Year, log.StartDate.Month, log.StartDate.Day, log.StartDate.Hour, log.StartDate.Minute - (log.StartDate.Minute % 10), 0);
            return $"{log.TodoID}_{log.PartnerID}_{log.FileId}";
        }

        private TodoLogResult TransformTodoLog(TodoLog todoLog) {
            return new TodoLogResult {
                Id = CreateId(todoLog),
                TodoID = todoLog.TodoID,
                TodoName = todoLog.TodoName,
                PartnerID = todoLog.PartnerID,
                PartnerName = todoLog.PartnerName,
                Duration = todoLog.EndDate.Subtract(todoLog.StartDate).Duration().TotalMilliseconds,
                StartDate = todoLog.StartDate,
                EndDate = todoLog.EndDate,
                CreatedBy = todoLog.CreatedBy,
                CreatedDate = todoLog.CreatedDate,
                TodoLogID = todoLog.TodoLogID,
                MessageID = todoLog.MessageID,
                NumberOfRows = todoLog.TotalNumberOfRows,
                NumberOfCommands = todoLog.TotalNumberOfCommands,
                NumberOfErrors = todoLog.TotalNumberOfErrors,
                NumberOfDuplicates = todoLog.TotalNumberOfDuplicates,
                FileName = todoLog.FileName,
                TotalNumberOfEvents = todoLog.TotalNumberOfEvents,
            };
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
                    var result = conn.Query<TodoLogDetail>(_todoLogSql, new { todoId });
                    return Ok(new TodoDetailsResult { Items = result.Select(TransformTodoLogDetail).ToArray() });
                }
            }
            catch (Exception e) {
                return StatusCode(500, new ProblemDetails {
                    Title = "InternalServerError",
                    Detail = e.Message
                });
            }
        }

        private TodoDetailResult TransformTodoLogDetail(TodoLogDetail detail) {
            return new TodoDetailResult {
                TodoLogDetailID = detail.TodoLogDetailID,
                TodoLogID = detail.TodoLogID,
                EventTypeID = detail.EventTypeID,
                NumberOfRows = detail.NumberOfRows,
                Description = detail.Description
            };
        }
    }

    public class TodoDetailsResult {
        public TodoDetailResult[] Items { get; set; }
    }

    public class TodoDetailResult {
        public int TodoLogDetailID { get; set; }
        public int TodoLogID { get; set; }
        public int EventTypeID { get; set; }
        public int NumberOfRows { get; set; }
        public string Description { get; set; }
    }

    public class TodoLogResult {
        public int TodoID { get; set; }
        public string TodoName { get; set; }
        public int PartnerID { get; set; }
        public string PartnerName { get; set; }
        public double Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TodoLogID { get; set; }
        public Guid MessageID { get; set; }
        public int NumberOfRows { get; set; }
        public int NumberOfCommands { get; set; }
        public int NumberOfErrors { get; set; }
        public int NumberOfDuplicates { get; set; }
        public string FileName { get; set; }
        public string Id { get; set; }
        public int TotalNumberOfEvents { get; set; }
    }

    public class TodoLogsResult {
        public TodoLogResult[] Items { get; set; }
    }

    public class TodoLog {
        public int TodoID { get; set; }
        public string TodoName { get; set; }
        public int PartnerID { get; set; }
        public string PartnerName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TodoLogID { get; set; }
        public Guid MessageID { get; set; }
        public int TotalNumberOfRows { get; set; }
        public int TotalNumberOfCommands { get; set; }
        public int TotalNumberOfErrors { get; set; }
        public int TotalNumberOfDuplicates { get; set; }
        public string FileName { get; set; }
        public int FileId { get; set; }
        public int TotalNumberOfEvents { get; set; }
    }
    public class TodoLogDetail {
        public int TodoLogDetailID { get; set; }
        public int TodoLogID { get; set; }
        public int EventTypeID { get; set; }
        public int NumberOfRows { get; set; }
        public string Description { get; set; }
    }
}
