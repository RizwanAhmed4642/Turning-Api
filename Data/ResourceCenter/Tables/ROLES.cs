using System;
using System.Collections.Generic;

namespace Meeting_App.Data.ResourceCenter.Tables
{
    public partial class ROLES
    {
        public ROLES()
        {
            USERS = new HashSet<USERS>();
        }

        public long ROLE_ID { get; set; }
        public string ROLE_NAME { get; set; }
        public long ROLE_DPT { get; set; }
        public bool ROLE_IS_ADMIN { get; set; }
        public bool? ROLE_STATUS { get; set; }
        public long? ROLE_CREATED_BY { get; set; }
        public DateTime ROLE_CREATED_ON { get; set; }
        public long? ROLE_UPDATED_BY { get; set; }
        public DateTime? ROLE_UPDATED_ON { get; set; }

        public virtual DPT ROLE_DPTNavigation { get; set; }
        public virtual ICollection<USERS> USERS { get; set; }
    }
}
