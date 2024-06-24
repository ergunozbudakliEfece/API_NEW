using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQL_API.Context;
using SQL_API.Models;
using SQL_API.Wrappers.Abstract;
using SQL_API.Wrappers.Concrete;
using System.Collections;

namespace SQL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ApplicationDbContext _Context;

        public ChatController(ApplicationDbContext Context)
        {
            _Context = Context;
        }
        [HttpGet("{receiverid}/{senderid}")]
        public async Task<IEnumerable> GetChat(string receiverid)
        {
            
        
          return await _Context.CHAT.Where(x=>x.RECEIVER_ID== receiverid && x.RECEIVER_READ==false).ToListAsync();
            
            
        }
        [HttpPost("Create")]
        public async Task<IResponse> CreateChat([FromBody] ChatModel chat)
        {
            try
            {
               

                var ChatEntry = _Context.CHAT.AddAsync(chat);

                if (!ChatEntry.IsCompletedSuccessfully)
                    return new ErrorResponse("Beklenmeyen bir hata oluştu.");

                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı", "Chat başarıyla insert edildi.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
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
