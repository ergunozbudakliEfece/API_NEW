using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using SQL_API.Context;
using SQL_API.Models;
using SQL_API.Wrappers.Abstract;
using SQL_API.Wrappers.Concrete;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SQL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly NOVAEFECEDbContext _context;
        private readonly IConfiguration _configuration;

        public ShippingController(NOVAEFECEDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        #region ShippingForms

        [HttpGet("ShippingForms")]
        public async Task<IResponse> GetShippingForms() 
        {
            try
            {
                IQueryable<ShippingFormDTO> Result = _context.Database.SqlQueryRaw<ShippingFormDTO>(@$"EXEC SP_SHIPPINGFORMS").AsQueryable();

                return new SuccessResponse<string>(JsonConvert.SerializeObject(Result), "");
            }
            catch (Exception ex) 
            {
                return new ErrorResponse(ex);
            }
        }

        [HttpGet("ShippingForms/{Order}")]
        public async Task<IResponse> GetShippingFormById(string Order)
        {
            try
            {
                IQueryable<ShippingFormDTO> Result = _context.Database.SqlQueryRaw<ShippingFormDTO>($"EXEC SP_SHIPPINGFORMS {Order}");

                return new SuccessResponse<string>(JsonConvert.SerializeObject(Result), "");
            }
            catch (Exception ex)
            {
                return new ErrorResponse(ex);
            }
        }

        [HttpPost("ShippingForms/Create")]
        public async Task<IResponse> CreateShippingForm() 
        {
            throw new NotImplementedException();
        }

        [HttpPut("ShippingForms/Update")]
        public async Task<IResponse> UpdateShippingForm()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("ShippingForms/Delete/{formNo}")]
        public async Task<IResponse> DeleteShippingForm(string formNo)
        {
            try
            {
                List<ShippingForm> shippingForms = await _context.TBL_SHIPPINGFORMS.Where(x => x.BELGE_NO == formNo).ToListAsync();

                if (shippingForms.Count > 0) 
                {
                    _context.TBL_SHIPPINGFORMS.RemoveRange(shippingForms);

                    await _context.SaveChangesAsync();

                    return new SuccessResponse<string>("", "");
                }


                return new ErrorResponse("Herhangi bir belge bulunamadı.");
            }
            catch (Exception ex) 
            {
                return new ErrorResponse($"{ex.Message} {(ex.InnerException is not null ? $"(Detail: {ex.InnerException.Message})" : "")}");
            }
        }

        [HttpPost("ShippingForms/SetStatus")]
        public async Task<IResponse> SetStatusShippingForm(ShippingFormStateChangeRequest shippingFormRequest)
        {
            try
            {
                IQueryable<ShippingForm> shippingForms = _context.TBL_SHIPPINGFORMS.Where(x => x.TYPE == shippingFormRequest.TYPE && x.BELGE_NO == shippingFormRequest.BELGE_NO && x.SIPARIS_NO == shippingFormRequest.SIPARIS_NO && x.STOK_KODU == shippingFormRequest.STOK_KODU);

                switch (shippingFormRequest.TYPE)
                {
                    case 6:
                        shippingForms = shippingForms.Where(x => x.CIKIS_DEPO == shippingFormRequest.CIKIS_DEPO);
                        break;
                    case 7:
                        shippingForms = shippingForms.Where(x => x.GIRIS_DEPO == shippingFormRequest.GIRIS_DEPO);
                        break;
                    case 9:
                        shippingForms = shippingForms.Where(x => x.GIRIS_DEPO == shippingFormRequest.GIRIS_DEPO && x.CIKIS_DEPO == shippingFormRequest.CIKIS_DEPO);
                        break;
                }

                foreach (ShippingForm form in await shippingForms.ToListAsync())
                {
                    form.ACTIVE = shippingFormRequest.STATE;
                    form.UPD_DATE = DateTime.Now;
                    form.UPD_USER_ID = shippingFormRequest.USER_ID;
                }

                await _context.SaveChangesAsync();

                return new SuccessResponse<string>("Herhangi bir belge bulunamadı.", "");
            }
            catch (Exception ex)
            {
                return new ErrorResponse($"{ex.Message} {(ex.InnerException is not null ? $"(Detail: {ex.InnerException.Message})" : "")}");
            }
        }
        #endregion
    }
}
