using System;
using System.Collections.Generic;

namespace Meeting_App.Data.ResourceCenter.Tables
{
    public partial class DPT
    {
        public DPT()
        {
            InverseDPT_PARENTNavigation = new HashSet<DPT>();
            RESOURCES = new HashSet<RESOURCES>();
            ROLES = new HashSet<ROLES>();
        }

        public long DPT_ID { get; set; }
        public long? DPT_PARENT { get; set; }
        public string DPT_NAME { get; set; }
        public string DPT_FOLDER { get; set; }
        public long DPT_TYPE { get; set; }
        public bool DPT_CAN_UPLOAD { get; set; }
        public bool? DPT_STATUS { get; set; }
        public long? DPT_CREATED_BY { get; set; }
        public DateTime DPT_CREATED_ON { get; set; }
        public long? DPT_UPDATED_BY { get; set; }
        public DateTime? DPT_UPDATED_ON { get; set; }

        public virtual DPT DPT_PARENTNavigation { get; set; }
        public virtual DPT_TYPE DPT_TYPENavigation { get; set; }
        public virtual ICollection<DPT> InverseDPT_PARENTNavigation { get; set; }
        public virtual ICollection<RESOURCES> RESOURCES { get; set; }
        public virtual ICollection<ROLES> ROLES { get; set; }
    }
}
