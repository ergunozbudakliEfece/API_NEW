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
        [HttpGet("usernames/All")]
        public string GetAUserByNamesALL()
        {
            DataTable table = new DataTable();


            string query = @"SELECT USER_ID,USER_NAME FROM TBL_USERDATA";

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

                LinkResult.SITUATION = false;

                _Context.LINKS.Update(LinkResult);
                _Context.Database.ExecuteSqlRaw($"UPDATE TBL_USERDATA SET USER_PASSWORD='{Request.USER_PASSWORD}' WHERE USER_ID={Request.USER_ID}");
                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı.", "Şifre başarılı şekilde değiştirildi.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
        [HttpPost, Route("MakeActive")]
        public async Task<IResponse> MakeActive(PasswordUpdateRequest Request)
        {
            try
            {
                User UserResult = await _Context.USERS.Where(x => x.USER_ID == Request.USER_ID).FirstOrDefaultAsync();
                Link LinkResult = await _Context.LINKS.Where(x => x.TOKEN == Request.TOKEN).FirstOrDefaultAsync();

                if (UserResult is null)
                    return new ErrorResponse("Kullanıcı bulunamadı.");

                UserResult.ACTIVE = true;
                UserResult.LOGIN_LIMIT = 5;
                LinkResult!.SITUATION = false;
                _Context.Database.ExecuteSqlRaw($"Update TBL_USERDATA SET ACTIVE=1,LOGIN_LIMIT=5 WHERE USER_ID={Request.USER_ID}");
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
        [HttpGet("UserCheck/{UserName}")]
        public async Task<IResponse> UserCheck(string UserName)
        {
            try
            {
                User UserResult = await _Context.USERS.Where(x => x.USER_NAME == UserName).FirstOrDefaultAsync();

                if (UserResult is null)
                    return new ErrorResponse("Kullanıcı bulunamadı.");

                return new SuccessResponse<User>(UserResult, "Kullanıcı bulundu.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
        [Authorize]
        [HttpGet("Birthday/{UserID}")]
        public async Task<IResponse> GetBirthdays(int UserID)
        {
            try
            {
                if (UserID != 0)
                {
                    List<BirthDayModel> list = await _Context.Database.SqlQueryRaw<BirthDayModel>("EXEC SP_USERDATA")!.ToListAsync();
                    return new SuccessResponse<List<BirthDayModel>>(list.Where(x => x.USER_ID == UserID && x.ACTIVE==true).ToList(), "Başarılı.");
                }
                else
                {
                    List<BirthDayModel> list = await _Context.Database.SqlQueryRaw<BirthDayModel>("EXEC SP_USERDATA")!.ToListAsync();
                    return new SuccessResponse<List<BirthDayModel>>(list.Where(x => x.ACTIVE == true).ToList(), "Başarılı.");
                }
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
        [Authorize]
        [HttpGet("Profil/{UserID}")]
        public async Task<IResponse> GetProfil(int UserID)
        {
            try
            {
                
                    List<ProfilModel> list = await _Context.Database.SqlQueryRaw<ProfilModel>("EXEC SP_USERDATA")!.ToListAsync();
                    return new SuccessResponse<List<ProfilModel>>(list.Where(x => x.USER_ID == UserID).ToList(), "Başarılı.");
                
                
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
        [Authorize]
        [HttpGet("DateOfStart/{UserID}")]
        public async Task<IResponse> GetStartWorkDay(int UserID)
        {
            try
            {
               
                    List<StartWorkDayModel> list = await _Context.Database.SqlQueryRaw<StartWorkDayModel>("EXEC SP_USERDATA")!.ToListAsync();
                    return new SuccessResponse<List<StartWorkDayModel>>(list.Where(x=>x.USER_ID==UserID&&x.ACTIVE==true).ToList(), "Başarılı.");
                
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
        [HttpGet("Types")]
        public async Task<IResponse> GetTypes()
        {
            try
            {
               
                    var list = await _Context.TYPES.ToListAsync();
                    return new SuccessResponse<List<TypeModel>>(list, "Başarılı.");
                
               
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
        [HttpPost, Route("ekle")]
        public async Task<IResponse> AddUser([FromBody] InsertUserModel user)
        {
            try
            {
                var active = user.ACTIVE == true ? 1 : 0;
                var query = $"INSERT INTO TBL_USERDATA(USER_NAME,USER_FIRSTNAME,USER_LASTNAME,ACTIVE,USER_MAIL,USER_TYPE,INS_USER_ID,ROLE_ID) VALUES('{user.USER_NAME}','{user.USER_FIRSTNAME}','{user.USER_LASTNAME}',{active},'{user.USER_MAIL}','{user.USER_TYPE}',{user.INS_USER_ID},{user.ROLE_ID})";
                    await _Context.Database.ExecuteSqlRawAsync(query)!;
                    return new SuccessResponse<string>("Başarılı", "Başarılı.");
                
               
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
        [HttpPost("Update")]
        public async Task<IResponse> UserUpdate([FromBody] UpdateUserModel user)
        {
            try
            {
                
                var active = user.ACTIVE == true ? 1 : 0;
                var query = $"UPDATE TBL_USERDATA SET USER_NAME='{user.USER_NAME}',USER_FIRSTNAME='{user.USER_FIRSTNAME}',USER_LASTNAME='{user.USER_LASTNAME}',ACTIVE={active},USER_MAIL='{user.USER_MAIL}',USER_TYPE='{user.USER_TYPE}',UPD_USER_ID={user.UPD_USER_ID},ROLE_ID={user.ROLE_ID} WHERE USER_ID={user.USER_ID}";
                await _Context.Database.ExecuteSqlRawAsync(query)!;
                return new SuccessResponse<string>("Başarılı", "Başarılı.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }

        [HttpGet("GetUsageStatistic/{UserID}")]
        public async Task<IResponse> GetUsageStatistic(int UserID = 0)
        {
            try
            {

                List<UsageStatisticModel> list = await _Context.Database.SqlQueryRaw<UsageStatisticModel>($"EXEC SP_USAGESTATISTICS {UserID}")!.ToListAsync();
                return new SuccessResponse<List<UsageStatisticModel>>(list, "Başarılı.");


            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
    }
}
