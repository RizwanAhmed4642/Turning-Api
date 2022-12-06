using System;
using System.Collections.Generic;

namespace Meeting_App.Data.ResourceCenter.Tables
{
    public partial class DPT_TYPE
    {
        public DPT_TYPE()
        {
            DPT = new HashSet<DPT>();
        }

        public long DT_ID { get; set; }
        public string DT_NAME { get; set; }
        public bool? DT_STATUS { get; set; }
        public long? DT_CREATED_BY { get; set; }
        public DateTime DT_CREATED_ON { get; set; }
        public long? DT_UPDATED_BY { get; set; }
        public DateTime? DT_UPDATED_ON { get; set; }

        public virtual ICollection<DPT> DPT { get; set; }
    }
}
