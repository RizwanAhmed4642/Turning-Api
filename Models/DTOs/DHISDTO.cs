using System.Collections.Generic;

namespace Meeting_App.Models.DTOs
{
  
    public class DataValue
    {
        public string dataElement { get; set; }
        public string period { get; set; }
        public string orgUnit { get; set; }
        public string value { get; set; }
        public string storedBy { get; set; }
        public string created { get; set; }
        public string lastUpdated { get; set; }
        public string comment { get; set; }
    }

    public class DHISDTO
    {
        public List<DataValue> dataValues { get; set; }
    }
}
