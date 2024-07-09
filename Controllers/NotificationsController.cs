using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQL_API.Context;
using SQL_API.Helper;
using SQL_API.Models;
using SQL_API.Wrappers.Abstract;
using SQL_API.Wrappers.Concrete;
using System.Collections;
using System.Collections.Generic;

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
        public async Task<IEnumerable> GetNotifications(int RECEIVER_ID)
        {
            return _Context.Database.SqlQueryRaw<NotificationModel>($"SP_NOTIFICATIONS {RECEIVER_ID}");
        }

        [HttpPost("Delete")]
        public async Task<IResponse> DeleteNoti(NotificationRequestModel notificationRequest)
        {
            try
            {
                
                var Query = $"SP_NOTIFICATIONDELETE {notificationRequest.ID},{notificationRequest.RECEIVER_ID}";


                var ChatEntry = _Context.Database.ExecuteSqlRaw(Query);

                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı", "Notification başarıyla silindi.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse("Beklenmeyen bir hata oluştu.");
            }
        }
        
        [HttpPost("DeleteRange")]
        public async Task<IResponse> DeleteRange(List<NotificationRequestModel> notificationRequest)
        {
            try
            {
                foreach(NotificationRequestModel Notification in notificationRequest)
                {
                    var Query = $"SP_NOTIFICATIONDELETE {Notification.ID},{Notification.RECEIVER_ID}";
                    var ChatEntry = _Context.Database.ExecuteSqlRaw(Query);
                }

                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı", "Notification başarıyla silindi.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse("Beklenmeyen bir hata oluştu.");
            }
        }

        [HttpPost("ReadRange")]
        public async Task<IResponse> ReadRange(List<NotificationRequestModel> notificationRequest)
        {
            try
            {
                foreach (NotificationRequestModel Notification in notificationRequest)
                {
                    var Query = $"SP_NOTIFICATIONREAD {Notification.ID},{Notification.RECEIVER_ID}";
                    var ChatEntry = _Context.Database.ExecuteSqlRaw(Query);
                }

                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı", "Notification başarıyla silindi.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse("Beklenmeyen bir hata oluştu.");
            }
        }

        [HttpPost("Read")]
        public async Task<IResponse> UpdateNoti(NotificationRequestModel notificationRequest)
        {
            try
            {

                var Query = $"SP_NOTIFICATIONREAD {notificationRequest.ID},{notificationRequest.RECEIVER_ID}";


                var ChatEntry = _Context.Database.ExecuteSqlRaw(Query);

                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı", "Notification başarıyla update edildi.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse("Beklenmeyen bir hata oluştu.");
            }
        }

        [HttpPost("Target")]
        public async Task<IResponse> CreateTarget(NotificationRequestModel notificationRequest)
        {
            try
            {

                var Query = $"SP_NOTIFICATIONTARGETINS {notificationRequest.ID},{notificationRequest.RECEIVER_ID}";


                var ChatEntry = _Context.Database.ExecuteSqlRaw(Query);

                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı", "Target başarıyla insert edildi.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse("Beklenmeyen bir hata oluştu.");
            }
        }

        [HttpPost("InsertNoti")]
        public async Task<IResponse> CreateNoti([FromBody] NotificationModel noti)
        {
            try
            {

                var Query = $"SP_NOTIFICATIONINS {noti.SENDER_ID},'{noti.SUBJECT}','{noti.NOTIFICATION_BODY}',{noti.IMPORTANCE}";


                var ChatEntry = _Context.Database.ExecuteSqlRaw(Query);

                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı", "Notification başarıyla insert edildi.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse("Beklenmeyen bir hata oluştu.");
            }
        }
    }
}
