using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class Folders
    {
        public Folders()
        {
            Files = new HashSet<Files>();
            InverseFk_Parent = new HashSet<Folders>();
        }

        public int PkCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool? EnableFlag { get; set; }
        public int? Fk_ParentId { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string DeletedBy { get; set; }

        public virtual Folders Fk_Parent { get; set; }
        public virtual ICollection<Files> Files { get; set; }
        public virtual ICollection<Folders> InverseFk_Parent { get; set; }
    }
}
