using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

namespace SQL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public MenuController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet("{userid}")]
        public string Get(int userid)
        {


            DataTable table = new DataTable();


            string query = "EXEC SP_USERMENU " + userid;

            string sqldataSource = _configuration.GetConnectionString("Con")!;
            SqlDataReader sqlreader;
            using (SqlConnection mycon = new SqlConnection(sqldataSource))
            {
                mycon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, mycon))
                {
                    sqlreader = myCommand.ExecuteReader();
                    table.Load(sqlreader);
                    sqlreader.Close();
                    mycon.Close();
                }
            }

            return JsonConvert.SerializeObject(table);
        }
    }
}
