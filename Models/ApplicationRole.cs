using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models
{
    public class ApplicationRole: IdentityRole<Guid>
    {
        public ApplicationRole() : base() { }

        public ApplicationRole(string name) : base(name) { }
    }
}
