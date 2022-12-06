using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs
{
    public class PaginationDTO
    {
        public int Skip { get; set; }
        public int pagesize { get; set; }
        public int OfficerId { get; set; }
        public string SignedBy { get; set; }
    }
}
