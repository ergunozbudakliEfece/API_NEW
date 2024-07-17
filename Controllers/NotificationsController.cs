using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SQL_API.Context;
using SQL_API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SQL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly ApplicationDbContext _Context;

        public NotificationsController(ApplicationDbContext Context)
        {
            _Context = Context;
        }

        [HttpGet("{RECEIVER_ID}")]
        public async Task<IQueryable> GetNotifications(string RECEIVER_ID) 
        {
            return _Context.Database.SqlQueryRaw<NotificationQuery>($"SP_NOTIFICATIONS {RECEIVER_ID}");
        }


        [HttpGet("sent/{SENDER_ID?}")]
        public async Task<IQueryable> GetNotificationsSent(string? SENDER_ID)
        {
            if (SENDER_ID is null)
                return _Context.Database.SqlQueryRaw<NotificationSentQuery>("SP_NOTIFICATIONSSENT");

            return _Context.Database.SqlQueryRaw<NotificationSentQuery>($"SP_NOTIFICATIONSSENT {SENDER_ID}");
        }

        [HttpPost("Create")]
        public async Task<Notification> CreateNotification(NotificationCreateRequest CreateRequest) 
        {
            EntityEntry<Notification> Created = await _Context.NOTIFICATIONS.AddAsync(CreateRequest.Notification);

            await _Context.SaveChangesAsync();

            foreach (int USER_ID in CreateRequest.Users)
            {
                _Context.Database.ExecuteSqlRaw($"SP_NOTIFICATIONTARGETINS {Created.Entity.NOTIFICATION_ID},{USER_ID}");
            }

            return Created.Entity;
        }

        [HttpGet("ReadNotificationFromTarget/{USER_ID}/{NOTIFICATION_ID}")]
        public async Task ReadNotificationFromTarget(int USER_ID, int NOTIFICATION_ID)
        {
            _Context.Database.ExecuteSqlRaw($"SP_NOTIFICATIONREAD {NOTIFICATION_ID},{USER_ID}");
        }

        [HttpPost("ReadRangeNotificationFromTarget")]
        public async Task ReadRangeNotificationFromTarget(List<NotificationRequest> NOTIFICATIONS)
        {
            foreach (NotificationRequest NOTIFICATION in NOTIFICATIONS)
            {
                IQueryable<NotificationTarget> Targets = _Context.NOTIFICATIONTARGETS.Where(x => x.RECEIVER_ID == NOTIFICATION.RECEIVER_ID && x.NOTIFICATION_ID == NOTIFICATION.ID).AsQueryable();

                foreach (NotificationTarget Target in Targets)
                {
                    Target.RECEIVER_READ = true;
                }
            }

            await _Context.SaveChangesAsync();
        }

        [HttpDelete("DeleteNotificationFromTarget/{USER_ID}/{NOTIFICATION_ID}")]
        public async Task DeleteNotificationFromTarget(int USER_ID, int NOTIFICATION_ID)
        {
            IQueryable<NotificationTarget> Targets = _Context.NOTIFICATIONTARGETS.Where(x => x.RECEIVER_ID == USER_ID && x.NOTIFICATION_ID == NOTIFICATION_ID).AsQueryable();

            foreach (NotificationTarget Target in Targets)
            {
                Target.RECEIVER_DELETE = true;
            }

            await _Context.SaveChangesAsync();
        }

        [HttpPost("DeleteRangeNotificationFromTarget")]
        public async Task DeleteRangeNotificationFromTarget(List<NotificationRequest> NOTIFICATIONS)
        {
            foreach (NotificationRequest NOTIFICATION in NOTIFICATIONS)
            {
                IQueryable<NotificationTarget> Targets = _Context.NOTIFICATIONTARGETS.Where(x => x.RECEIVER_ID == NOTIFICATION.RECEIVER_ID && x.NOTIFICATION_ID == NOTIFICATION.ID).AsQueryable();

                foreach (NotificationTarget Target in Targets)
                {
                    Target.RECEIVER_DELETE = true;
                }
            }

            await _Context.SaveChangesAsync();
        }

        [HttpDelete("DeleteAllNotificationFromTarget/{NOTIFICATION_ID}")]
        public async Task DeleteAllNotificationFromTarget(int NOTIFICATION_ID) 
        {
            IQueryable<NotificationTarget> Targets = _Context.NOTIFICATIONTARGETS.Where(x => x.NOTIFICATION_ID == NOTIFICATION_ID).AsQueryable();

            foreach (NotificationTarget Target in Targets)
            {
                Target.RECEIVER_DELETE = true;
            }

            await _Context.SaveChangesAsync();
        }

        [HttpPost("DeleteRangeAllNotificationFromTarget")]
        public async Task DeleteRangeAllNotificationFromTarget(List<int> NOTIFICATIONS)
        {
            foreach(int NOTIFICATION_ID in NOTIFICATIONS)
            {
                IQueryable<NotificationTarget> Targets = _Context.NOTIFICATIONTARGETS.Where(x => x.NOTIFICATION_ID == NOTIFICATION_ID).AsQueryable();

                foreach (NotificationTarget Target in Targets)
                {
                    Target.RECEIVER_DELETE = true;
                }
            }

            await _Context.SaveChangesAsync();
        }
    }
}
