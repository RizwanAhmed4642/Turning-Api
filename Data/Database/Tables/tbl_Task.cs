using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class tbl_Task
    {
        public tbl_Task()
        {
            Attachment = new HashSet<Attachment>();
            tbl_TaskAssignee = new HashSet<tbl_TaskAssignee>();
            tbl_TaskCc = new HashSet<tbl_TaskCc>();
            tbl_TaskDetails = new HashSet<tbl_TaskDetails>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Designation { get; set; }
        public Guid? AssignTo { get; set; }
        public int? TaskStatus { get; set; }
        public DateTime? DueDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdationDate { get; set; }
        public bool? RecordStatus { get; set; }
        public string Priority { get; set; }
        public bool? IsReopen { get; set; }
        public DateTime? ExtendedDate { get; set; }
        public int? ParentTaskId { get; set; }
        public bool? IsSMSSent { get; set; }

        public virtual tbl_Status TaskStatusNavigation { get; set; }
        public virtual ICollection<Attachment> Attachment { get; set; }
        public virtual ICollection<tbl_TaskAssignee> tbl_TaskAssignee { get; set; }
        public virtual ICollection<tbl_TaskCc> tbl_TaskCc { get; set; }
        public virtual ICollection<tbl_TaskDetails> tbl_TaskDetails { get; set; }
    }
}
