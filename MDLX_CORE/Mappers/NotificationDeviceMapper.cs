using MDL_CORE.ComponentCore.Entities;
using MDL_CORE.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;

namespace MDLX_CORE.Mappers
{
    public static class NotificationDeviceMapper
    {
        public static List<NotificationDeviceViewModel> ToList<T>(this IQueryable<NotificationDevice> source)
        {
            List<NotificationDeviceViewModel> q = source
                .Select(x => new { 
                    Date = x.RegistrationDate,
                    UserName = x.User.UserName,
                    PushEndpoint = x.PushEndpoint,
                    UserId = x.UserId,
                    Id = x.Id
                })
                .ToList()
                .Select(x => new NotificationDeviceViewModel()
                {
                    RegisterDate = x.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                    UserName = x.UserName,
                    PushEndpoint = x.PushEndpoint,
                    UserId = x.UserId,
                    Id = x.Id,
                })
                .ToList();
            return q;
        }
    }
}