using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models
{
    public class Applicationuser : IdentityUser<Guid>
    {
        public string NP { get; set; }
        public string Designation { get; set; }
        public string FullName { get; set; }
        public int Order { get; set; }
        public int ContactId { get; set; }
        public bool? IsRecordStatus { get; set; }
        public bool? IsDeleted { get; set; }
        public int? ContactDesignationId { get; set; }
        public int? ContactCategoryId { get; set; }
        public int? ContactDepartmentId { get; set; }
        public int? ContactCompanyId { get; set; }
        public string ProfilePic { get; set; }
    }
}
