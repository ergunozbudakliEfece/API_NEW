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
        [HttpGet("{receiverid}")]
        public async Task<IEnumerable> GetNotifications(int receiverid)
        {
            
            
            return _Context.Database.SqlQueryRaw<NotificationModel>($"SP_NOTIFICATIONS {receiverid}");


        }
        [HttpPost("Delete/{id}/{receiverid}")]
        public async Task<IResponse> DeleteNoti(int id,int receiverid)
        {
            try
            {
                
                var Query = $"SP_NOTIFICATIONDELETE {id},{receiverid}";


                var ChatEntry = _Context.Database.ExecuteSqlRaw(Query);

                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı", "Notification başarıyla silindi.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse("Beklenmeyen bir hata oluştu.");
            }
        }
        [HttpPost("Read/{id}/{receiverid}")]
        public async Task<IResponse> UpdateNoti(int id, int receiverid)
        {
            try
            {

                var Query = $"SP_NOTIFICATIONREAD {id},{receiverid}";


                var ChatEntry = _Context.Database.ExecuteSqlRaw(Query);

                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı", "Notification başarıyla update edildi.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse("Beklenmeyen bir hata oluştu.");
            }
        }
        [HttpPost("Target/{id}/{receiverid}")]
        public async Task<IResponse> CreateTarget(int id, int receiverid)
        {
            try
            {

                var Query = $"SP_NOTIFICATIONTARGETINS {id},{receiverid}";


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
