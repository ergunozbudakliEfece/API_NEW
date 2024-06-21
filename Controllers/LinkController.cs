using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQL_API.Context;
using SQL_API.Models;
using SQL_API.Wrappers.Abstract;
using SQL_API.Wrappers.Concrete;

namespace SQL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkController : ControllerBase
    {
        private readonly ApplicationDbContext _Context;

        public LinkController(ApplicationDbContext Context)
        {
            _Context = Context;
        }

        [HttpGet("{Token}")]
        public async Task<IResponse> GetLink(string Token)
        {
            try
            {
                Link Result = await _Context.LINKS.Where(x => x.TOKEN == Token && x.SITUATION == true).FirstOrDefaultAsync();

                if (Result is null)
                    return new ErrorResponse("Mevcut link hiç oluşturulmamış veya pasif olabilir.");

                return new SuccessResponse<Link>(Result, "Token başarıyla getirildi.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }

        [HttpPost("Create")]
        public async Task<IResponse> CreateLink(LinkCreateRequest LinkRequest)
        {
            try
            {
                Link NewLink = new()
                {
                    TYPE = LinkRequest.TYPE,
                    SITUATION = true,
                    DURATION = LinkRequest.DURATION,
                    TOKEN = LinkRequest.TOKEN
                };

                var LinkEntry = _Context.LINKS.AddAsync(NewLink);

                if (!LinkEntry.IsCompletedSuccessfully)
                    return new ErrorResponse("Beklenmeyen bir hata oluştu.");

                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı", "Token başarıyla oluşturuldu.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }

        [HttpPut("SetPassive/{Token}")]
        public async Task<IResponse> UpdateLinkSituationToPassive(string Token)
        {
            try
            {
                Link Result = await _Context.LINKS.Where(x => x.TOKEN == Token && x.SITUATION == true).FirstOrDefaultAsync();

                if (Result is null)
                    return new ErrorResponse("Mevcut link hiç oluşturulmamış veya halihazırda pasif olabilir.");

                Result.SITUATION = false;

                await _Context.SaveChangesAsync();

                return new SuccessResponse<Link>(Result, "Token başarıyla pasif hale getirildi.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
    }
}
