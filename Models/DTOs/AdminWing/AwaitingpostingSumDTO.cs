using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs.AdminWing
{
    public class AwaitingpostingSumDTO
    {
       public string  ForwardingOfficer_Id {get;set;}
       public string Section { get; set; }
       public int Total { get; set; }
    }
}
