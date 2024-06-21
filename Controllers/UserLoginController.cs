using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SQL_API.Models;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json.Serialization;

namespace SQL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public UserLoginController(IConfiguration configuration)
        {

            _configuration = configuration;
        }
        [HttpGet("{id}")]
        public IActionResult GetByID(int id)
        {
            string query = @"SELECT TOP 1 * FROM TBL_LOGIN WHERE USER_ID="+id+" ORDER BY LOG_ID DESC";

            string sqldataSource = _configuration.GetConnectionString("Con")!;
            SqlDataReader sqlreader;
            DataTable table = new DataTable();
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
            return new ObjectResult(JsonConvert.SerializeObject(table));
        }
        [HttpPut]
        public IActionResult Update([FromBody] LoginModel item)
        {
            string query = @"UPDATE TBL_LOGIN SET LAST_ACTIVITY="+item.LAST_ACTIVITY+" WHERE LOG_ID="+item.LOG_ID;

            string sqldataSource = _configuration.GetConnectionString("Con")!;
            SqlDataReader sqlreader;
            using (SqlConnection mycon = new SqlConnection(sqldataSource))
            {
                mycon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, mycon))
                {
                    sqlreader = myCommand.ExecuteReader();
                    sqlreader.Close();
                    mycon.Close();
                }
            }
            return new NoContentResult();
        }
        [HttpPost]
        public IActionResult Insert([FromBody] LoginModel item)
        {
            string query = @"INSERT INTO TBL_LOGIN(USER_ID,PLATFORM) VALUES("+item.USER_ID+",'"+item.PLATFORM+"')";

            string sqldataSource = _configuration.GetConnectionString("Con")!;
            SqlDataReader sqlreader;
            using (SqlConnection mycon = new SqlConnection(sqldataSource))
            {
                mycon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, mycon))
                {
                    sqlreader = myCommand.ExecuteReader();
                    sqlreader.Close();
                    mycon.Close();
                }
            }
            return new NoContentResult();
        }
    }
}
