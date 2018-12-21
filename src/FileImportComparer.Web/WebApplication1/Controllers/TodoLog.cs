using System;

namespace WebApplication1.Controllers {
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
}