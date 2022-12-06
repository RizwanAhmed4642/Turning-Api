using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs
{
    public class TaskCount
    {
         
        public int Total { get; set; }
        public int Pending{ get; set; }
        public int InProgress { get; set; }
        public int Submitted { get; set; }
        public int ReOpen { get; set; }
        public int OverDue { get; set; }
        public int Completed { get; set; }
    }
}
