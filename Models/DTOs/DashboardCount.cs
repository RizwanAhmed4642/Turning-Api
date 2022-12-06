using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs
{
    public class DashboardCount
    {
        public int Pending { get; set; }
        public int Resolved { get; set; }

        public int InProcess { get; set; }

        public int Rejected { get; set; }

    }
}
