using Meeting_App.Models;
using Meeting_App.Service;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.SIgnalRHub
{
    public class NotificationHub:Hub
    {
        private NotificationService _notificationService=new NotificationService();
        public async Task BroadcastChartData(string userId)
        {
          
            await Clients.All.SendAsync("broadcastchartdata", _notificationService.GetNotifications("*",userId,true));
            
        }
    }
}
