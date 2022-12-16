using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class abc
    {
        public long? Id { get; set; }
        public string TrainingTittle { get; set; }
        public string Cadre { get; set; }
        public string TrainingType { get; set; }
        public string TraingCategory { get; set; }
        public string DepartmentId { get; set; }
    }
}
