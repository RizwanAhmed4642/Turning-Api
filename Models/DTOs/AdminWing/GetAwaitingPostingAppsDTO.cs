using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs.AdminWing
{
    public class GetAwaitingPostingAppsDTOResponseDTO
    {
        public GetAwaitingPostingAppsDTOResponseDTO()
        {
            List = new List<GetAwaitingPostingAppsDTO>();
        }

        public List<GetAwaitingPostingAppsDTO> List { get; set; }


    }
    public class GetAwaitingPostingAppsDTO
    {

        public int Id { get; set; }
        public int TrackingNumber { get; set; }
        public string ApplicationType { get; set; }
        public string Section { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public DateTime? Datetime { get; set; }
    }
}
