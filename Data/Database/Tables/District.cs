using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class District
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string OldCode { get; set; }
        public string CapitalTehsilCode { get; set; }
    }
}
