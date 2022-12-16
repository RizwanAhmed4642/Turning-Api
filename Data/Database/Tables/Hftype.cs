using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class Hftype
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? HFCat_Id { get; set; }
        public int? OrderBy { get; set; }
    }
}
