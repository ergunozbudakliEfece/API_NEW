using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using SQL_API.Wrappers.Abstract;
using SQL_API.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SQL_API.Wrappers.Concrete;
using SQL_API.Context;
using Microsoft.AspNetCore.Authorization;

namespace SQL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _Context;

        public UsersController( IConfiguration configuration, ApplicationDbContext Context)
        {
            _configuration = configuration;
            _Context = Context;
        }
        [HttpGet("usernames")]
        public string GetAUserByNames()
        {
            DataTable table = new DataTable();


            string query = @"SELECT USER_ID,USER_NAME FROM TBL_USERDATA WHERE ACTIVE=1";

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
        [HttpGet("exec/{name}/{password}")]
        public string UserExec(string name, string password)
        {

            DataTable table = new DataTable();


            string query = @"EXEC SP_AUTHENTICATION '" + name + "','" + password + "'";

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
        [HttpGet("active")]
        public string GetAllActiveUsers()
        {

            DataTable table = new DataTable();


            string query = @"SELECT USER_ID,USER_NAME,USER_FIRSTNAME,USER_LASTNAME,ACTIVE FROM TBL_USERDATA WHERE ACTIVE=1";

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

        [HttpPost, Route("UpdatePassword")]
        public async Task<IResponse> PasswordUpdate(PasswordUpdateRequest Request)
        {
            try
            {
                User UserResult = await _Context.USERS.Where(x => x.USER_ID == Request.USER_ID && x.ACTIVE).FirstOrDefaultAsync();
                Link LinkResult = await _Context.LINKS.Where(x => x.TOKEN == Request.TOKEN).FirstOrDefaultAsync();

                if (UserResult is null)
                    return new ErrorResponse("Kullanıcı bulunamadı.");

                UserResult.USER_PASSWORD = Request.USER_PASSWORD;
                LinkResult.SITUATION = false;

                _Context.USERS.Update(UserResult);
                _Context.LINKS.Update(LinkResult);

                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı.", "Şifre başarılı şekilde değiştirildi.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }

        [HttpGet("CheckMail/{Email}")]
        public async Task<IResponse> UserCheckWithMail(string Email)
        {
            try
            {
                User UserResult = await _Context.USERS.Where(x => x.USER_MAIL == Email && x.ACTIVE).FirstOrDefaultAsync();

                if (UserResult is null)
                    return new ErrorResponse("Kullanıcı bulunamadı.");

                return new SuccessResponse<User>(UserResult, "Kullanıcı bulundu.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
    }
}
