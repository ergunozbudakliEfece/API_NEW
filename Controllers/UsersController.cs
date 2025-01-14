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
using SQL_API.Helper;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        [HttpGet("SalesPersons/{TYPE}")]
        public async Task<IResponse> GetSalesPersonByType(int TYPE)
        {
            try
            {
                DataTable table = new DataTable();

                string query = @$"SELECT
                            SP.USER_ID,
                            (UD.USER_FIRSTNAME + ' ' + UD.USER_LASTNAME) AS FULL_NAME
                        FROM
                        NOVA_EFECE..TBL_CRMSALESPEOPLE AS SP WITH(NOLOCK) INNER JOIN
                        NOVA_YENI..TBL_USERDATA UD WITH(NOLOCK) ON SP.USER_ID=UD.USER_ID
                        WHERE SP.TYPE={TYPE}";

                string sqldataSource = _configuration.GetConnectionString("Con")!;
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
                return new SuccessResponse<string>(JsonConvert.SerializeObject(table), "Başarılı.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }

        }
        [HttpGet("usernames")]
        public string GetAUserByNames()
        {
            DataTable table = new DataTable();


            string query = @"SELECT USER_ID,USER_NAME,FULL_NAME=USER_FIRSTNAME+' '+USER_LASTNAME FROM TBL_USERDATA WHERE ACTIVE=1";

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
        [HttpGet("lastActivity")]
        public string LastActivityTime()
        {
            DataTable table = new DataTable();


            string query = @"EXEC SP_LASTACTIVETIME";

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
        [HttpGet("plasiyerler")]
        public string GetPlasiyerler()
        {
            DataTable table = new DataTable();


            string query = @"SELECT USER_ID,USER_NAME,FULL_NAME=USER_FIRSTNAME+' '+USER_LASTNAME FROM TBL_USERDATA WHERE ACTIVE=1 AND USER_ID IN (SELECT USER_ID FROM TBL_AUTH WHERE MODULE_ID=17)";

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
        [HttpGet("plasiyer/{id}")]
        public async Task<IResponse> GetPlasiyerlerAsync(int id)
        {
            try
            {
                DataTable table = new DataTable();


                string query = $"NOVA_EFECE..SP_PLASIYERAUTH {id}";

                string sqldataSource = _configuration.GetConnectionString("Con")!;
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
                return new SuccessResponse<string>(JsonConvert.SerializeObject(table), "Başarılı.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }

        }
        [HttpGet("plasiyer/usernames")]
        public async Task<IResponse> GetPlasiyerUsernames()
        {
            try
            {
                DataTable table = new DataTable();


                string query = $"SELECT USER_ID,USER_NAME,FULL_NAME=USER_FIRSTNAME+' '+USER_LASTNAME FROM TBL_USERDATA WHERE ACTIVE=1";

                string sqldataSource = _configuration.GetConnectionString("Con")!;
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
                return new SuccessResponse<string>(JsonConvert.SerializeObject(table), "Başarılı.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }

        }
        [HttpGet("usernames/All")]
        public string GetAUserByNamesALL()
        {
            DataTable table = new DataTable();


            string query = @"SELECT UD.USER_ID,USER_NAME,NAME FROM TBL_USERDATA UD WITH(NOLOCK) LEFT JOIN TBL_ROLESDETAILS RD WITH(NOLOCK) ON RD.ID=UD.ROLE_ID";

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
        [HttpGet("usernames/Actives")]
        public string GetAUserByNamesActives()
        {
            DataTable table = new DataTable();


            string query = @"SELECT UD.USER_ID,USER_NAME,NAME FROM TBL_USERDATA UD WITH(NOLOCK) LEFT JOIN TBL_ROLESDETAILS RD WITH(NOLOCK) ON RD.ID=UD.ROLE_ID WHERE ACTIVE=1";

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
		[HttpGet("actives")]
		public string GetAllActivesUsers()
		{

			DataTable table = new DataTable();


			string query = @"SELECT USER_ID,USER_NAME,USER_FIRSTNAME,USER_LASTNAME,LOGIN_ACTIVE AS ACTIVE FROM TBL_USERDATA WHERE ACTIVE=1 AND USER_TYPE=0";

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


            string query = @"SELECT * FROM (SELECT USER_ID,USER_NAME,USER_FIRSTNAME,USER_LASTNAME,LOGIN_ACTIVE AS ACTIVE,LASTTIME=(SELECT MAX(LAST_ACTIVITY_DATE) FROM TBL_LOGIN TL WHERE TL.USER_ID=UD.USER_ID GROUP BY TL.USER_ID) FROM TBL_USERDATA UD WHERE LOGIN_ACTIVE=1 AND USER_TYPE=0)Q ORDER BY LASTTIME DESC,USER_FIRSTNAME";

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
        [HttpGet("chatyetkisiz")]
        public string GetYetkisiz()
        {

            DataTable table = new DataTable();


            string query = @"SELECT * FROM TBL_USERDATA WITH(NOLOCK) WHERE LOGIN_ACTIVE=1 AND USER_ID NOT IN (SELECT USER_ID FROM TBL_PRIVATEAUTH WHERE MODULE_ID=9)";

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
        [HttpGet("All")]
        public string GetAllUsers()
        {

            DataTable table = new DataTable();


            string query = @"SP_GETALLACTIVEUSERS";

            string sqldataSource = _configuration.GetConnectionString("NOVA_EFECE")!;
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


            string query = $"SP_USERDATA {PersonaID}";

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

                UserResult.LOGIN_ACTIVE = true;
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
        [HttpGet("UserCheck/{UserID}")]
        public async Task<IResponse> UserCheck(int UserID)
        {
            try
            {
                User UserResult = await _Context.USERS.Where(x => x.USER_ID == UserID).FirstOrDefaultAsync();

                if (UserResult is null)
                    return new ErrorResponse("Kullanıcı bulunamadı.");

                return new SuccessResponse<User>(UserResult, "Kullanıcı bulundu.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
        [HttpGet("GetByUsername/{UserName}")]
        public async Task<IResponse> GetByUsername(string UserName)
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
        [HttpGet("Profil/{UserID}/{Type}")]
        public async Task<IResponse> GetProfil(int UserID,string Type)
        {
            try
            {
                
                    List<ProfilModel> list = await _Context.Database.SqlQueryRaw<ProfilModel>($"EXEC SP_USERDATA @language='{Type}'")!.ToListAsync();
                    return new SuccessResponse<List<ProfilModel>>(list.Where(x => x.USER_ID == UserID).ToList(), "Başarılı.");
                
                
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }

        [HttpGet("sube/{UserID}")]
        public async Task<IResponse> GetSubeProfil(int UserID)
        {
            try
            {

                List<SubeUserModel> list = await _Context.Database.SqlQueryRaw<SubeUserModel>($"EXEC SP_GETFROMSUBSTATION {UserID}")!.ToListAsync();
                return new SuccessResponse<List<SubeUserModel>>(list, "Başarılı.");


            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }

        [Authorize, HttpGet("Employees/{UserID}/{Tumu}")]
        public async Task<IResponse> GetEmployees(int UserID, int Tumu)
        {
            try
            {
                var Result = "[]";

                if(Tumu == 1) 
                {
                    DataTable table = new DataTable();

                    string query = @"SELECT * FROM (SELECT USER_ID,USER_NAME,USER_FIRSTNAME + ' ' +USER_LASTNAME AS NAME,ACTIVE,LASTTIME=(SELECT MAX(LAST_ACTIVITY_DATE) FROM TBL_LOGIN TL WHERE TL.USER_ID=UD.USER_ID GROUP BY TL.USER_ID) FROM TBL_USERDATA UD WHERE USER_TYPE=0)Q";

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

                    Result = JsonConvert.SerializeObject(table);
                }
                else if(Tumu == 0)
                {
                    List<SubeUserModel> list = await _Context.Database.SqlQueryRaw<SubeUserModel>($"EXEC SP_GETFROMSUBSTATION {UserID}")!.ToListAsync();

                    Result = JsonConvert.SerializeObject(list);
                }

                return new SuccessResponse<string>(Result, "Başarılı.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }

        [HttpPost("UpdatePersonel")]
        public IResponse UpdateUser([FromBody] PersonalModel personel)
        {
            
            try
            {
                     var p = _Context.PERSONAL.FirstOrDefault(x => x.USER_ID == personel.USER_ID);
                     p.CINSIYET=personel.CINSIYET;
                     p.SUBE=personel.SUBE;
                     p.TCKN=personel.TCKN;
                     p.DOGUM_TARIHI= personel.DOGUM_TARIHI!=null?((DateTime)personel.DOGUM_TARIHI).AddHours(3) : personel.DOGUM_TARIHI;
                     p.DOGUM_YERI_IL=personel.DOGUM_YERI_IL;
                     p.DOGUM_YERI_ILCE=personel.DOGUM_YERI_ILCE;
                     p.OGRENIM_DURUMU=personel.OGRENIM_DURUMU;
                     p.MEZUN_OKUL=personel.MEZUN_OKUL;
                     p.MEZUN_BOLUM=personel.MEZUN_BOLUM;
                     p.MEZUN_YIL=personel.MEZUN_YIL;
                     p.IKAMETGAH_ADRES=personel.IKAMETGAH_ADRES;
                     p.IKAMETGAH_IL=personel.IKAMETGAH_IL;
                     p.IKAMETGAH_ILCE=personel.IKAMETGAH_ILCE;
                     p.MEDENI_HAL=personel.MEDENI_HAL;
                     p.ESIN_ADI=personel.ESIN_ADI;
                     p.ES_CALISMA_DURUMU=personel.ES_CALISMA_DURUMU;
                     p.ES_CALISMA_FIRMA=personel.ES_CALISMA_FIRMA;
                     p.ES_UNVANI=personel.ES_UNVANI;
                     p.COCUK_SAYI=personel.COCUK_SAYI;
                     p.ARAC_PLAKA=personel.ARAC_PLAKA;
                     p.EHLIYET_SINIF=personel.EHLIYET_SINIF;
                     p.CALISILAN_BIRIM=personel.CALISILAN_BIRIM;
                     p.GOREV=personel.GOREV;
                     p.ILK_IS_TARIH= personel.ILK_IS_TARIH !=null ? ((DateTime)personel.ILK_IS_TARIH).AddHours(3) : personel.ILK_IS_TARIH; 
                     p.KAN_GRUP=personel.KAN_GRUP;
                     p.VARSA_SUREKLI_HAST=personel.VARSA_SUREKLI_HAST;
                     p.VARSA_ENGEL_DURUM=personel.VARSA_ENGEL_DURUM;
                     p.VARSA_SUREKLI_KULL_ILAC=personel.VARSA_SUREKLI_KULL_ILAC;
                     p.ILETISIM_OZEL_TEL=personel.ILETISIM_OZEL_TEL;
                     p.ILETISIM_SIRKET_TEL = personel.ILETISIM_SIRKET_TEL;
                     p.ILETISIM_BILGI_MAIL=personel.ILETISIM_BILGI_MAIL;
                     p.ACIL_DURUM_KISI=personel.ACIL_DURUM_KISI;
                     p.ACIL_DURUM_KISI_ILETISIM=personel.ACIL_DURUM_KISI_ILETISIM;
                     p.ACIL_DURUM_KISI2=personel.ACIL_DURUM_KISI2;
                     p.ACIL_DURUM_KISI_ILETISIM2=personel.ACIL_DURUM_KISI_ILETISIM2;
                     p.MEVCUT_IS_ILK_TARIH = personel.MEVCUT_IS_ILK_TARIH!=null ? ((DateTime)personel.MEVCUT_IS_ILK_TARIH).AddHours(3) : personel.MEVCUT_IS_ILK_TARIH;
                     p.MEVCUT_IS_ILK_TARIH2 = personel.MEVCUT_IS_ILK_TARIH2!=null ? ((DateTime)personel.MEVCUT_IS_ILK_TARIH2).AddHours(3) : personel.MEVCUT_IS_ILK_TARIH2;
                     p.MEVCUT_IS_ILK_TARIH3 = personel.MEVCUT_IS_ILK_TARIH3!=null ? ((DateTime)personel.MEVCUT_IS_ILK_TARIH3).AddHours(3) : personel.MEVCUT_IS_ILK_TARIH3;
                     p.IS_CIKIS_TARIH= personel.IS_CIKIS_TARIH!=null ? ((DateTime)personel.IS_CIKIS_TARIH).AddHours(3) : personel.IS_CIKIS_TARIH; 
                     p.IS_CIKIS_TARIH2= personel.IS_CIKIS_TARIH2!=null? ((DateTime)personel.IS_CIKIS_TARIH2).AddHours(3) : personel.IS_CIKIS_TARIH2; 
                     p.IS_CIKIS_TARIH3= personel.IS_CIKIS_TARIH3!=null? ((DateTime)personel.IS_CIKIS_TARIH3).AddHours(3) : personel.IS_CIKIS_TARIH3; 
                     p.UPD_USER_ID=personel.UPD_USER_ID;
                     p.UPD_DATE= ((DateTime)personel.UPD_DATE).AddHours(3);
                     p.ARAC_MARKA_MODEL = personel.ARAC_MARKA_MODEL;
                     p.PC_MARKA_MODEL = personel.PC_MARKA_MODEL;
                     p.PC_SERI_NO = personel.PC_SERI_NO;
                     p.SIRKET_TEL_MARKA_MODEL = personel.SIRKET_TEL_MARKA_MODEL;
                     p.SIRKET_TEL_IMEI = personel.SIRKET_TEL_IMEI;
                     p.DAHILI_NO = personel.DAHILI_NO;
                     p.DAHILI_MARKA_MODEL = personel.DAHILI_MARKA_MODEL;
                     p.DAHILI_IPEI_NO = personel.DAHILI_IPEI_NO;
                     p.ACIL_DURUM_KISI_YAKINLIK = personel.ACIL_DURUM_KISI_YAKINLIK;
                     p.ACIL_DURUM_KISI2_YAKINLIK = personel.ACIL_DURUM_KISI2_YAKINLIK;
                     p.COCUK1 = personel.COCUK1==""?null:personel.COCUK1;
                     p.COCUK2 = personel.COCUK2;
                     p.COCUK3 = personel.COCUK3;
                     p.COCUK4 = personel.COCUK4;
                     p.COCUK5 = personel.COCUK5;
                     p.COCUK1_DOGUM = personel.COCUK1_DOGUM != null ? ((DateTime)personel.COCUK1_DOGUM).AddHours(3) : personel.COCUK1_DOGUM;
                     p.COCUK2_DOGUM = personel.COCUK2_DOGUM != null ? ((DateTime)personel.COCUK2_DOGUM).AddHours(3) : personel.COCUK2_DOGUM;
                     p.COCUK3_DOGUM = personel.COCUK3_DOGUM != null ? ((DateTime)personel.COCUK3_DOGUM).AddHours(3) : personel.COCUK3_DOGUM;
                     p.COCUK4_DOGUM = personel.COCUK4_DOGUM != null ? ((DateTime)personel.COCUK4_DOGUM).AddHours(3) : personel.COCUK4_DOGUM;
                     p.COCUK5_DOGUM = personel.COCUK5_DOGUM != null ? ((DateTime)personel.COCUK5_DOGUM).AddHours(3) : personel.COCUK5_DOGUM;
                p.SAGLIK_TARAMASI_TARIH = personel.SAGLIK_TARAMASI_TARIH != null ? ((DateTime)personel.SAGLIK_TARAMASI_TARIH).AddHours(3) : personel.SAGLIK_TARAMASI_TARIH;
                p.TETANOZ_ASI_TARIH = personel.TETANOZ_ASI_TARIH != null ? ((DateTime)personel.TETANOZ_ASI_TARIH).AddHours(3) : personel.TETANOZ_ASI_TARIH;
                p.ALERJI = personel.ALERJI;
                _Context.Update(p);
                var user = _Context.USERS.FirstOrDefault(x => x.USER_ID == p.USER_ID);
                user!.USER_FIRSTNAME = personel.USER_FIRSTNAME;
                user!.USER_LASTNAME = personel.USER_LASTNAME;

                var entry = _Context.Update(user);
                entry.Property(x => x.USER_PASSWORD).IsModified = false;
                
                
                
                _Context.SaveChanges();
                return new SuccessResponse<string>("Başarılı", "Başarılı.");


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
                var LOGIN_ACTIVE = user.LOGIN_ACTIVE == true ? 1 : 0;
                var query = $"INSERT INTO TBL_USERDATA(USER_NAME,USER_FIRSTNAME,USER_LASTNAME,LOGIN_ACTIVE,ACTIVE,USER_MAIL,USER_TYPE,INS_USER_ID,ROLE_ID) VALUES('{user.USER_NAME}',N'{user.USER_FIRSTNAME}',N'{user.USER_LASTNAME}',{LOGIN_ACTIVE},{active},'{user.USER_MAIL}','{user.USER_TYPE}',{user.INS_USER_ID},{user.ROLE_ID})";
                    await _Context.Database.ExecuteSqlRawAsync(query)!;
                var newuser= await _Context.USERS.FirstOrDefaultAsync(x => x.USER_NAME == user.USER_NAME);
                string Token = AESEncryption.Encrypt($"{newuser!.USER_ID};{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}");
                string Url = $"{Request.Scheme}://192.168.2.13/UpdatePassword?Token={Token}";
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

                if (user.LOGIN_ACTIVE)
                {
					Email PasswordMail = new Email()
					{
						From = "sistem@efecegalvaniz.com",
						To = (user.USER_MAIL == null || user.USER_MAIL == "" ? "sistem@efecegalvaniz.com" : user.USER_MAIL),
						Subject = "Nova | Hoşgeldiniz",
						Body = "Merhaba " + newuser.USER_FIRSTNAME + " " + newuser.USER_LASTNAME + ",</br></br>Nova, Efece'deki çalışma hayatınızda işlerinizi kolaylaştırmak ve verimliliğinizi artırmak üzere İş ve Süreç Geliştirme Departmanı tarafından geliştirilmiş web tabanlı bir uygulamadır.</br></br>Nova uygulamasına giriş yapabileceğiniz kullanıcı oluşturulmuştur. Aşağıdaki butona tıklayarak şifrenizi belirleyebilirsiniz.</br></br>Şifrenizi oluşturduktan sonra uygulama giriş ekranına yönlendirileceksiniz.</br></br>" +
						 "<div><!--[if mso]><v:roundrect xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:w=\"urn:schemas-microsoft-com:office:word\" href=\"" + Url + "\"style=\"height:35px;v-text-anchor:middle;width:200px;\" arcsize=\"71.42857142857143%\" stroke=\"f\" fillcolor=\"#0052A3\"><w:anchorlock/><center><![endif]--><a href=\"\"+Url+\"\" style=\"background-color:#0052A3;border-radius:25px;color:#e4e4e4;display:inline-block;font-family:sans-serif;font-size:13px;font-weight:bold;line-height:35px;text-align:center;text-decoration:none;width:200px;-webkit-text-size-adjust:none;\">Şifre Oluştur</a><!--[if mso]></center></v:roundrect><![endif]--></div><br>Her türlü görüş, öneri veya sorunlarınız için "+ "<a href=\"mailto:surecgelistirme@efecegalvaniz.com\">İş ve Süreç Geliştirme Departmanı</a>" + " ile iletişime geçebilirsiniz.</br><br>" +
						 "İyi çalışmalar dileriz.</br></br>",
						Signature = MailSignature.SYSTEM_SIGNATURE
					};

					EmailService.SendEmail(PasswordMail);
				}

                return new SuccessResponse<string>(newuser.USER_ID.ToString(), "Başarılı.");
                
               
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
                var LOGIN_ACTIVE = user.LOGIN_ACTIVE == true ? 1 : 0;
                var role = user.ROLE_ID == null ? "NULL" : user.ROLE_ID.ToString();
                var query = $"UPDATE TBL_USERDATA SET USER_NAME='{user.USER_NAME}',USER_FIRSTNAME=N'{user.USER_FIRSTNAME}',USER_LASTNAME=N'{user.USER_LASTNAME}',ACTIVE={active},LOGIN_ACTIVE={LOGIN_ACTIVE},USER_MAIL='{user.USER_MAIL}',USER_TYPE='{user.USER_TYPE}',UPD_USER_ID={user.UPD_USER_ID},ROLE_ID={role} WHERE USER_ID={user.USER_ID}";
                await _Context.Database.ExecuteSqlRawAsync(query)!;
                return new SuccessResponse<string>("Başarılı", "Başarılı.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }

        [HttpGet("GetUsageStatistic/{Culture}/{UserID}")]
        public async Task<IResponse> GetUsageStatistic(string Culture, int UserID = 0)
        {
            try
            {
                List<UsageStatisticModel> list = await _Context.Database.SqlQueryRaw<UsageStatisticModel>($"EXEC SP_USAGESTATISTICS {UserID}, '{Culture}'")!.ToListAsync();
                return new SuccessResponse<List<UsageStatisticModel>>(list, "Başarılı.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
    }
}
