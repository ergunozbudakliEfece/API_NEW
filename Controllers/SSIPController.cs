using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SQL_API.Context;
using SQL_API.Models;
using SQL_API.Wrappers.Abstract;
using SQL_API.Wrappers.Concrete;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace SQL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SSIPController : ControllerBase
    {
        private readonly ApplicationDbContext _Context;
        private readonly IConfiguration _configuration;
        public SSIPController(ApplicationDbContext Context, IConfiguration configuration)
        {
            _Context = Context;
            _configuration = configuration;
        }

        /* İthalat Siparişleri */
        [HttpGet("{Kapali?}")]
        public async Task<IResponse> GetSSIP(string? Kapali = null)
        {
            try
            {
                DataTable table = new DataTable();

                string query = $@"EXEC SP_SSIPDETAILS {(Kapali is null ? "" : $"'{Kapali}'")}";

                string sqldataSource = _configuration.GetConnectionString("NOVA_EFECE")!;
                SqlDataReader sqlreader;
                await using (SqlConnection mycon = new SqlConnection(sqldataSource))
                {
                    mycon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, mycon))
                    {
                        sqlreader = await myCommand.ExecuteReaderAsync();
                        table.Load(sqlreader);
                        sqlreader.Close();
                        mycon.Close();
                    }
                }
                return new SuccessResponse<string>(JsonConvert.SerializeObject(table), "Başarılı");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
    }
}
