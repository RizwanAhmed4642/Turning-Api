using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class ApplicationVersion
    {
        public int Id { get; set; }
        public string App_Version { get; set; }
        public string Json_Version { get; set; }
    }
}
