using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models
{
    public class NotificationModel
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
        public string userNameFrom { get; set; }
        public string userNameTo { get; set; }

    }
}
