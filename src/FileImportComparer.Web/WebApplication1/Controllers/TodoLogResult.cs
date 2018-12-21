using System;

namespace WebApplication1.Controllers {
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
}