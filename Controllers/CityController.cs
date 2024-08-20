using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SQL_API.Context;
using SQL_API.Models;
using SQL_API.Wrappers.Abstract;
using SQL_API.Wrappers.Concrete;
using System.Collections;

namespace SQL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ApplicationDbContext _Context;

        public CityController(ApplicationDbContext Context)
        {
            _Context = Context;
        }
        [HttpGet]
        public async Task<IResponse> GetCities()
        {
            var list = await _Context.CITIES.ToListAsync();
            return new SuccessResponse<string>(JsonConvert.SerializeObject(list), "Başarılı.");
        }
        [HttpGet("districts/{CITY_ID?}")]
        public async Task<IResponse> GetNotificationsSent(int? CITY_ID)
        {
            try
            {

                List<DistrictModel> list = await _Context.Database.SqlQueryRaw<DistrictModel>($"SP_GETDISTRICTS {CITY_ID}")!.ToListAsync();
                return new SuccessResponse<List<DistrictModel>>(list, "Başarılı.");


            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }

        }
        [HttpGet("districts/all")]
        public async Task<IResponse> GetAllDistricts()
        {
            try
            {

                List<DistrictModel> list = await _Context.Database.SqlQueryRaw<DistrictModel>($"SP_GETDISTRICTS")!.ToListAsync();
                return new SuccessResponse<string>(JsonConvert.SerializeObject(list), "Başarılı.");


            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }

        }
    }
}
