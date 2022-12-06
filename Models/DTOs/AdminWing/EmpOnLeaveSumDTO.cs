using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs.AdminWing
{
    public class EmpOnLeaveSumDTO
    {
        public int Officer_Id { get; set; }
        public string SignedBy { get; set; }
        public int Total { get; set; }

    }
}
