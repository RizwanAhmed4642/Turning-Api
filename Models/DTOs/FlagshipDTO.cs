using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs
{
    public class FlagshipDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileUrl {get;set;}
        public DateTime CreatedOn { get; set; }
    }
}
