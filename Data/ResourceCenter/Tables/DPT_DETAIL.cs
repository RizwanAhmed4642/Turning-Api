using System;
using System.Collections.Generic;

namespace Meeting_App.Data.ResourceCenter.Tables
{
    public partial class DPT_DETAIL
    {
        public long DPTD_ID { get; set; }
        public long DPTD_RC_ID { get; set; }
        public DateTime? DPTD_SPSHD_DATE { get; set; }
        public DateTime? DPTD_SREGULAION_DATE { get; set; }
        public DateTime? DPTD_CM_DATE { get; set; }
        public DateTime? DPTD_CS_DATE { get; set; }
        public DateTime? DPTD_SF_DATE { get; set; }
        public DateTime? DPTD_SL_DATE { get; set; }
        public DateTime? DPTD_SH_DATE { get; set; }
        public DateTime? DPTD_SPD_DATE { get; set; }
        public DateTime? DPTD_BACK_DATE { get; set; }
        public DateTime? DPTD_OTHER_DATE { get; set; }
        public DateTime? DPTD_MOVING_DATE { get; set; }

        public virtual RESOURCES DPTD_RC_ { get; set; }
    }
}
