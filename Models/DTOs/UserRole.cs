using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs
{
    public class UserRole
    {
        public const string admin = "admin";

        public const string user = "user";

        public string UserId { get; set; }

        public string[] AddRoleId { get; set; }

        public string[] DeleteRoleId { get; set; }
    }

    public class ChangePassword
    {

        public Guid userid { get; set; }

        [Required(ErrorMessage = "Current Password is required")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New Password is required")]
        public string Newpassword { get; set; }
    }
}
