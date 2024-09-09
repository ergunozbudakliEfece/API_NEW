using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SQL_API.Context;
using SQL_API.Wrappers.Abstract;
using SQL_API.Wrappers.Concrete;
using System.Data;

namespace SQL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : Controller
    {
        private readonly NOVAEFECEDbContext _Context;
        private readonly IConfiguration _configuration;
        public CustomersController(NOVAEFECEDbContext Context, IConfiguration configuration)
        {
            _Context = Context;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IResponse> GetCustomers()
        {
            try
            {
                
                return new SuccessResponse<string>(JsonConvert.SerializeObject(_Context.TBL_CUSTOMERS), "Başarılı");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
    }
}
