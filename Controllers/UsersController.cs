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
using SQL_API.Services.MailService.Concrete;
using SQL_API.Services.MailService.Abstract;
using SQL_API.Services.MailService.Utils;
using SQL_API.Utils.Encryptions;
using System;

namespace SQL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _Context;
        private readonly IEmailService EmailService;

        public UsersController( IConfiguration configuration, ApplicationDbContext Context, IEmailService MailService)
        {
            _configuration = configuration;
            _Context = Context;
            EmailService = MailService;
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
        [HttpGet("Personal/{PersonaID}")]
        public string GetPersonal(int PersonaID)
        {

            DataTable table = new DataTable();


            string query = $"SELECT * FROM TBL_PERSONALDATA WHERE USER_ID={PersonaID}";

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
        [HttpGet("Ranks")]
        public async Task<IResponse> GetRanks()
        {
            try
            {

                List<UserRankModel> list = await _Context.Database.SqlQueryRaw<UserRankModel>("SP_USERRANK")!.ToListAsync();
                return new SuccessResponse<List<UserRankModel>>(list, "Başarılı.");

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
        [HttpGet("Units")]
        public async Task<IResponse> GetUnits()
        {
            try
            {

                var list = await _Context.UNITS.ToListAsync();
                return new SuccessResponse<List<UnitsModel>>(list, "Başarılı.");


            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
        [HttpGet("Titles")]
        public async Task<IResponse> GetTitles()
        {
            try
            {

                var list = await _Context.TITLES.ToListAsync();
                return new SuccessResponse<List<WorkTitleModel>>(list, "Başarılı.");


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
                var query = $"INSERT INTO TBL_USERDATA(USER_NAME,USER_FIRSTNAME,USER_LASTNAME,ACTIVE,USER_MAIL,USER_TYPE,INS_USER_ID,ROLE_ID) VALUES('{user.USER_NAME}',N'{user.USER_FIRSTNAME}',N'{user.USER_LASTNAME}',{active},'{user.USER_MAIL}','{user.USER_TYPE}',{user.INS_USER_ID},{user.ROLE_ID})";
                    await _Context.Database.ExecuteSqlRawAsync(query)!;
                var newuser= await _Context.USERS.FirstOrDefaultAsync(x => x.USER_NAME == user.USER_NAME);
                string Token = AESEncryption.Encrypt($"{newuser!.USER_ID};{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}");
                string Url = $"{Request.Scheme}://192.168.2.13:86/UpdatePassword?Token={Token}";
                try
                {
                    Link NewLink = new()
                    {
                        TYPE = 1,
                        SITUATION = true,
                        DURATION = 5,
                        TOKEN = Token
                    };

                    var LinkEntry = _Context.LINKS.AddAsync(NewLink);

                    if (!LinkEntry.IsCompletedSuccessfully)
                        return new ErrorResponse("Beklenmeyen bir hata oluştu.");

                    await _Context.SaveChangesAsync();

                }
                catch (Exception Ex)
                {
                    return new ErrorResponse(Ex);
                }
                Email PasswordMail = new Email()
                {
                    From = "sistem@efecegalvaniz.com",
                    To = (user.USER_MAIL?? "sistem@efecegalvaniz.com"),
                    Subject = "NOVA | Şifre Belirleme",
                    Body = "Merhaba " + newuser.USER_FIRSTNAME + ",</br></br>Nova üzerinde kullanıcınız oluşturulmuştur. Aşağıdakı butona tıklayarak şifrenizi belirleyebilirsiniz.</br></br>" +
                         "<div><!--[if mso]><v:roundrect xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:w=\"urn:schemas-microsoft-com:office:word\" href=\"" + Url + "\"style=\"height:35px;v-text-anchor:middle;width:200px;\" arcsize=\"71.42857142857143%\" stroke=\"f\" fillcolor=\"#0052A3\"><w:anchorlock/><center><![endif]--><a href=\"\"+Url+\"\" style=\"background-color:#0052A3;border-radius:25px;color:#e4e4e4;display:inline-block;font-family:sans-serif;font-size:13px;font-weight:bold;line-height:35px;text-align:center;text-decoration:none;width:200px;-webkit-text-size-adjust:none;\">Şifremi Oluştur</a><!--[if mso]></center></v:roundrect><![endif]--></div></br>" +
                         "İyi çalışmalar dileriz.</br></br>",
                    Signature = MailSignature.SYSTEM_SIGNATURE
                };
                query = @"SP_COPYAUTHROLE " + newuser!.USER_ID + "," + user.ROLE_ID + "," + user.INS_USER_ID;

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

                EmailService.SendEmail(PasswordMail);

                return new SuccessResponse<string>("Başarılı", "Başarılı.");
                
               
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
        [HttpPost, Route("sil/{userid}")]
        public async Task<IResponse> RemoveUser(int userid)
        {
            try
            {
                _=await _Context.USERS.Where(x => x.USER_ID == userid).ExecuteDeleteAsync();
                _=await _Context.SaveChangesAsync();
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
                var query = $"UPDATE TBL_USERDATA SET USER_NAME='{user.USER_NAME}',USER_FIRSTNAME=N'{user.USER_FIRSTNAME}',USER_LASTNAME=N'{user.USER_LASTNAME}',ACTIVE={active},USER_MAIL='{user.USER_MAIL}',USER_TYPE='{user.USER_TYPE}',UPD_USER_ID={user.UPD_USER_ID},ROLE_ID={user.ROLE_ID} WHERE USER_ID={user.USER_ID}";
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
