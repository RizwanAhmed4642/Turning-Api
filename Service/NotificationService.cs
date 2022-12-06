using AutoMapper;
using Meeting_App.Data.Database.Tables;
using Meeting_App.Models;
using Meeting_App.SIgnalRHub;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Service
{
    public class NotificationService
    {
        #region Fields
        private readonly IMapper _mapper;
   
        #endregion


        #region Constructors

        public NotificationService()
        {
       
        }
        public NotificationService(IMapper mapper)
        {
            _mapper = mapper;
          
        }
        #endregion

        #region GetNotification
        public List<NotificationModel> GetNotifications(string userIdFrom , string userIdTo, Boolean recordStatus)
        {
            try
            {
                using (var db = new IDDbContext())
                {
                    List<NotificationModel> res = new List<NotificationModel>();

                    res = db.Notification
                        .Where(t => 
                        ((userIdFrom != "*" && t.UserIdFrom == Guid.Parse(userIdFrom))   || (userIdTo != "*" && t.UserIdTo == Guid.Parse(userIdTo)  ) )
                        && t.RecordStatus == recordStatus)
                        .Select(t => new NotificationModel
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Description = t.Description,
                            Link = t.Link,
                            UserIdTo = t.UserIdTo,
                            UserIdFrom = t.UserIdFrom,
                            userNameTo= t.UserIdToNavigation.UserName,
                            userNameFrom=t.UserIdFromNavigation.UserName,
                            SourceId = t.SourceId,
                            SourceType=t.SourceType,
                            RecordStatus=t.RecordStatus

                        }).ToList();

                    return res;

                }
            }
            catch
            {
                throw;
            }
        }

        #endregion


        #region AddTask
        public int AddNotification(NotificationModel model)
        {
            using (var db = new IDDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                           var newNotification = new Notification();

                            newNotification.RecordStatus = true;
                            newNotification.Title = model.Title;
                            newNotification.Description = model.Description;
                            newNotification.Link = model.Link;
                            newNotification.UserIdTo = model.UserIdTo;
                            newNotification.UserIdFrom = model.UserIdFrom;
                            newNotification.SourceId = model.SourceId;
                            newNotification.SourceType = model.SourceType;
                            newNotification.RecordStatus = model.RecordStatus;


                            db.Notification.Add(newNotification);

                            db.SaveChanges();

                            trans.Commit();
                            
                           
                        return 1;

                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }


            }
        }
        #endregion
    }
}
