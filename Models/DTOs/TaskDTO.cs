using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs
{
    public class TaskDTO
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Designation { get; set; }
        public string Priority { get; set; }
        public Guid? AssignTo { get; set; }
        public DateTime? DueDate { get; set; }
        public List<IFormFile> TaskAttachments { get; set; }
        public int TaskStatus { get; set; }
        public DateTime? ExtendedDate { get; set; }
        public string TaskAssigneeData { get; set; }
        public string TaskCcData { get; set; }
        public List<TaskAssigneeList> TaskAssignees = new List<TaskAssigneeList>();
        public List<TaskCcList> TaskCcs = new List<TaskCcList>();
        public int? ParentTaskId { get; set; }


    }


    public class TaskDetailDTO
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string Comments { get; set; }
        public List<IFormFile> TaskAttachments { get; set; }
        public List<string> Attachment { get; set; }
        public int TaskStatus { get; set; }
        public string TaskStatusName { get; set; }
        public DateTime? ReadDateTime { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string TaskAssignee { get; set; }
        public string TaskAssigneeId { get; set; }
        public DateTime? ExtendedDate { get; set; }
        public bool? IsRead { get; set; }
    }


    public class TaskFilterDTO
    {
        public Guid? userList { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }

    }
    public class TaskAssigneeList
    {
        public int? Id { get; set; }
        public int? TaskId { get; set; }
        public Guid? TaskAssigneeId { get; set; }
        public string TaskAssigneeName { get; set; }
    }
    public class TaskCcList
    {
        public int? Id { get; set; }
        public int? TaskId { get; set; }
        public Guid? TaskCcId { get; set; }
        public string TaskCcName { get; set; }
    }
}
