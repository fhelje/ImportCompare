using System;

namespace WebApplication1.Controllers {
    public static class CacheKeys {
        public static Func<string, string> GetTodos = env => "_GetTodos" + env;
        public static string GetTodosByTodoId => "_GetTodosByTodoId";
        public static string GetDetail => "_GetDetail";
    }
}