using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SQL_API.Context;
using SQL_API.Models;
using SQL_API.Wrappers.Abstract;
using SQL_API.Wrappers.Concrete;

namespace SQL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ApplicationDbContext _Context;

        public CountriesController(ApplicationDbContext Context)
        {
            _Context = Context;
        }
        [HttpGet]
        public async Task<IResponse> GetCountries()
        {
            var list = await _Context.COUNTRIES.ToListAsync();
            return new SuccessResponse<string>(JsonConvert.SerializeObject(list), "Başarılı.");
        }
        [HttpGet("globalcities/all")]
        public async Task<IResponse> GetGlobalCities()
        {
            try
            {

                List<GlobalCityModel> list = await _Context.Database.SqlQueryRaw<GlobalCityModel>($"SELECT * FROM TBL_GLBCITIES")!.ToListAsync();
                return new SuccessResponse<string>(JsonConvert.SerializeObject(list), "Başarılı.");


            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }

        }
    }
}
