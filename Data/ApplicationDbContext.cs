using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Meeting_App.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Meeting_App.Data
{
    public class ApplicationDbContext : IdentityDbContext<Applicationuser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
