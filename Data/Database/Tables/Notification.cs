using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class Notification
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public Guid UserIdTo { get; set; }
        public Guid UserIdFrom { get; set; }
        public string SourceType { get; set; }
        public string SourceId { get; set; }
        public bool? RecordStatus { get; set; }

        public virtual AspNetUsers UserIdFromNavigation { get; set; }
        public virtual AspNetUsers UserIdToNavigation { get; set; }
    }
}
