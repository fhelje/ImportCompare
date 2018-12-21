using System;

namespace WebApplication1.Controllers {
    public static class Transformers {
        private static  string CreateId(TodoLog log) {
            var date = new DateTime(log.StartDate.Year, log.StartDate.Month, log.StartDate.Day, log.StartDate.Hour, log.StartDate.Minute - (log.StartDate.Minute % 10), 0);
            return $"{log.TodoID}_{log.PartnerID}_{log.FileId}";
        }
        public static  TodoLogResult TransformTodoLog(TodoLog todoLog) {
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

        public static TodoDetailResult TransformTodoLogDetail(TodoLogDetail detail) {
            return new TodoDetailResult {
                TodoLogDetailID = detail.TodoLogDetailID,
                TodoLogID = detail.TodoLogID,
                EventTypeID = detail.EventTypeID,
                NumberOfRows = detail.NumberOfRows,
                Description = detail.Description
            };
        }

    }
}