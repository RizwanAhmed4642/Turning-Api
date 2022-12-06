using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs
{
    public class ListTaskDTO
    {
            public int? Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Designation { get; set; }
            public string TaskAssignee { get; set; }
            public string TaskAssigneeId { get; set; }
            public string TaskCC { get; set; }
            public string Priority { get; set; }
            public Guid? AssignTo { get; set; }
            public DateTime? DueDate { get; set; }
            public List<string> Attachment { get; set; }
            public bool? RecordStatus { get; set; }
            public string Comments { get; set; }
            public string TaskStatus { get; set; }
            public string CreatedBy { get; set; }
            public string CreatedById { get; set; }
            public DateTime? CreatedDate { get; set; }
        public DateTime? ExtendedDate { get; set; }

        public int UnReadComentsCount { get; set; }
        public int? ParentTaskId { get; set; }
        public bool IsAssignAble { get; set; }
        public List<ListTaskDTO> IsParentTaskDetail { get; set; }
        public List<ListTaskDTO> IsSubTaskDetail { get; set; }

        public bool? IsSMSSent { get; set; }
    }
}
