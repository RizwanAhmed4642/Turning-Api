using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Service.Common
{
    public class Raw
    {
    }

    public class DDLModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }
    }

    public class DDLStatusModel
    {
        public int Id { get; set; }
        public string Status { get; set; }
    }
    public class ConatctDesingantionModel
    {
        public Guid Id { get; set; }
        public string Designation { get; set; }

        public string OrderBy { get; set; }
    }
}
