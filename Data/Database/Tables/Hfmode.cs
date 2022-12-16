using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class Hfmode
    {
        public int Id { get; set; }
        public string ModeName { get; set; }
        public int? Mode_Id { get; set; }
        public int? HF_Id { get; set; }
    }
}
