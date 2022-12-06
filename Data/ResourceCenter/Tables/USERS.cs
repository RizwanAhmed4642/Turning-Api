using System;
using System.Collections.Generic;

namespace Meeting_App.Data.ResourceCenter.Tables
{
    public partial class USERS
    {
        public USERS()
        {
            RESOURCES = new HashSet<RESOURCES>();
        }

        public long USER_ID { get; set; }
        public long USER_ROLE { get; set; }
        public string USER_NAME { get; set; }
        public string USER_USERNAME { get; set; }
        public string USER_PASSWORD { get; set; }
        public string USER_PHONE { get; set; }
        public string remember_token { get; set; }
        public bool? USER_STATUS { get; set; }
        public long? USER_CREATED_BY { get; set; }
        public DateTime USER_CREATED_ON { get; set; }
        public long? USER_UPDATED_BY { get; set; }
        public DateTime? USER_UPDATED_ON { get; set; }

        public virtual ROLES USER_ROLENavigation { get; set; }
        public virtual ICollection<RESOURCES> RESOURCES { get; set; }
    }
}
