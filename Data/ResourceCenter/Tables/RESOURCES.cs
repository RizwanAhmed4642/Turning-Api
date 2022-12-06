using System;
using System.Collections.Generic;

namespace Meeting_App.Data.ResourceCenter.Tables
{
    public partial class RESOURCES
    {
        public RESOURCES()
        {
            DPT_DETAIL = new HashSet<DPT_DETAIL>();
        }

        public long RESOURCE_ID { get; set; }
        public long RESOURCE_DPT { get; set; }
        public string RESOURCE_TITLE { get; set; }
        public string RESOURCE_DESCRIPTION { get; set; }
        public string RESOURCE_FILE_PDF { get; set; }
        public string RESOURCE_FILE_WORD { get; set; }
        public bool? RESOURCE_STATUS { get; set; }
        public long RESOURCE_CREATED_BY { get; set; }
        public DateTime RESOURCE_CREATED_ON { get; set; }
        public long? RESOURCE_UPDATED_BY { get; set; }
        public DateTime? RESOURCE_UPDATED_ON { get; set; }

        public virtual USERS RESOURCE_CREATED_BYNavigation { get; set; }
        public virtual DPT RESOURCE_DPTNavigation { get; set; }
        public virtual ICollection<DPT_DETAIL> DPT_DETAIL { get; set; }
    }
}
