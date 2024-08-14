using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SQL_API.Models;
using SQL_API.Wrappers.Abstract;
using SQL_API.Wrappers.Concrete;
using SQL_API.Context;

namespace SQL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _Context;
        public MenuController(IConfiguration configuration, ApplicationDbContext Context)
        {
            _configuration = configuration;
            _Context = Context;
        }
        
        [HttpGet("{userid}/{Type}")]
        public string Get(int userid,string Type)
        {


            DataTable table = new DataTable();


            string query = "EXEC SP_USERMENU " + userid+",'"+Type+"'";

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
       
        [HttpGet("Favorite/{UserID}/{Type}")]
        public async Task<IResponse> GetFav(int UserID,string Type)
        {
            try
            {

                List<FavoriteModuleModel> list = await _Context.Database.SqlQueryRaw<FavoriteModuleModel>($"EXEC SP_FAVORITEMODULES {UserID},'{Type}'")!.ToListAsync();
                return new SuccessResponse<List<FavoriteModuleModel>>(list, "Başarılı.");


            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }

        [HttpPost("ElapsedTime")]
        public async Task<IResponse> UpdateElapsedTime(int UserID, int ModuleID, int ElapsedTime,int LogID)
        {
            try
            {
                int EffectedRow = await _Context.Database.ExecuteSqlAsync($"EXEC SP_ACTIVITYINS {UserID},{ModuleID},{ElapsedTime},{LogID}");

                return new SuccessResponse<string>("Geçen süre başarıyla güncellendi.", "Başarılı.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
    }
}
