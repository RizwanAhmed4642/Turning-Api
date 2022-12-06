using AutoMapper;
using Meeting_App.Data.Database.Tables;
using Meeting_App.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App
{
    public class AutoMapperProfile
    {

        


        public class UserProfile : Profile
        {
            public UserProfile()
            {
                #region FormsIndicatorSettings
                CreateMap<TaskDTO, tbl_Task>();
                CreateMap<tbl_Task, TaskDTO>();
                CreateMap<ContactDTO, tbl_Contacts>();
                CreateMap<tbl_Contacts, ContactDTO>();
                CreateMap<ContactTypeDTO, tbl_ContactDesignation>();
                CreateMap<tbl_ContactDesignation, ContactTypeDTO>();
                CreateMap<tbl_DailyEngagement, DailyEngagementDTO>();
                CreateMap<DailyEngagementDTO, tbl_DailyEngagement>();
                CreateMap<FileDTO, Files>().ReverseMap();
                CreateMap<EventCalender, EventCalenderView>().ReverseMap();
                CreateMap<tbl_MeetingOrganizer, MeetingOrganizerDTO>();
                CreateMap<MeetingOrganizerDTO, tbl_MeetingOrganizer>();
                CreateMap<tbl_MeetingVenue, MeetingVenueDTO>();
                CreateMap<MeetingVenueDTO, tbl_MeetingVenue>();
                CreateMap<tbl_Conference, ConferenceViewDTO>();
                CreateMap<ConferenceViewDTO, tbl_Conference>();
                #endregion
            }
        }
    }
}
