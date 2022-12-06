using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class ContactsDetailView
    {
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Designation { get; set; }
        public int DesignationId { get; set; }
        public string categoryName { get; set; }
        public int? CategoryId { get; set; }
        public string departmentName { get; set; }
        public int? DepartmentId { get; set; }
        public string companyName { get; set; }
        public int? CompanyId { get; set; }
        public Guid ParticipantId { get; set; }
        public string ParticipantName { get; set; }
    }
}
