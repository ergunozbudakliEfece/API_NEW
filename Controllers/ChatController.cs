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
using System.Data;
using System.Data.SqlClient;

namespace SQL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly ApplicationDbContext _Context;

        public ChatController(ApplicationDbContext Context)
        {
            _Context = Context;
        }
        [HttpGet("{receiverid}")]
        public async Task<IEnumerable> GetChat(string receiverid)
        {
            var list = await _Context.CHAT.Where(x => (x.RECEIVER_ID == receiverid|| x.SENDER_ID == receiverid)&&x.SHOWID==receiverid).ToListAsync();
            list.ForEach(x => x.CHAT = Helpers.Decrypt(x.CHAT!));
            list.ForEach(x => x.RECEIVER_ID = x.RECEIVER_ID!.Trim());
            return list;
            
            
        }
        [HttpGet("allmychat/{receiverid}")]
        public async Task<IEnumerable> AllChat(string receiverid)
        {
            var list = _Context.Database.SqlQueryRaw<Model>($"SELECT SENDER_ID FROM TBL_CHAT WHERE ((SENDER_ID={receiverid} OR RECEIVER_ID={receiverid}) AND SENDER_ID<>{receiverid}) AND SHOWID={receiverid} UNION SELECT RECEIVER_ID FROM TBL_CHAT WHERE ((SENDER_ID={receiverid} OR RECEIVER_ID={receiverid}) AND RECEIVER_ID<>10002) AND SHOWID={receiverid}");
            var listShowId=new List<string>();
            foreach (var x in list)
            {
                if (!listShowId.Contains(x.SENDER_ID!))
                {
                    listShowId.Add(x.SENDER_ID!);
                }
                
            }
            return listShowId;


        }
        public class Model
        {
            public string SENDER_ID { get; set; }
        }
        [HttpPost("Create")]
        public async Task<IResponse> CreateChat([FromBody] ChatModel chat)
        {
            try
            {
                var d = chat.DATE.ToString("yyyy-MM-dd HH:mm:ss");
                var Query = $"SP_INSERTCHAT '{chat.CONNECTION_ID}','{chat.SENDER_ID}','{chat.RECEIVER_ID}',0,N'{Helpers.Encrypt(chat.CHAT!)}','{d}'";


                var ChatEntry = _Context.Database.ExecuteSqlRaw(Query);

                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı", "Chat başarıyla insert edildi.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse("Beklenmeyen bir hata oluştu.");
            }
        }
        [HttpPost("Delete")]
        public async Task<IResponse> DeleteChat([FromBody] ChatModel chat)
        {
            try
            {
                _Context.CHAT.Where(x => ((x.SENDER_ID == chat.SENDER_ID && x.RECEIVER_ID == chat.RECEIVER_ID) || (x.SENDER_ID == chat.RECEIVER_ID && x.RECEIVER_ID == chat.SENDER_ID)) && x.SHOWID == chat.SENDER_ID).ExecuteDelete();

                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı", "Chat başarıyla temizlend.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse("Beklenmeyen bir hata oluştu.");
            }
        }
        [HttpPost("AllDelete")]
        public async Task<IResponse> DeleteAllChat([FromBody] ChatModel chat)
        {
            try
            {
                _Context.CHAT.Where(x => x.SHOWID==chat.SENDER_ID).ExecuteDelete();

                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı", "Chat başarıyla temizlendi.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse("Beklenmeyen bir hata oluştu.");
            }
        }
        [HttpPost("Read")]
        public async Task<IResponse> UpdateAsRead([FromBody] ChatModel chat)
        {
            try
            {


                var ChatEntry= _Context.CHAT.Where(x => x.RECEIVER_ID ==chat.SENDER_ID && x.SENDER_ID == chat.RECEIVER_ID).ExecuteUpdate(s=>s.SetProperty(b=>b.RECEIVER_READ, true));

                

                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı", "Token başarıyla oluşturuldu.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }

    }
}
