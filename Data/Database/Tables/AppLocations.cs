using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class AppLocations
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string CategoryCode { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int? ProvinceId { get; set; }
        public string OldCode { get; set; }
        public string ContectPerson { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public int? DocterDealCovid { get; set; }
        public int? ParamadicDealCovid { get; set; }
        public string IsCovidFacility { get; set; }
        public bool? IsTrue { get; set; }
        public string NameUrdu { get; set; }
    }
}
