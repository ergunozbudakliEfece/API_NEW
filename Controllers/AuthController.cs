using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SQL_API.Models;
using SQL_API.Wrappers.Abstract;
using SQL_API.Wrappers.Concrete;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SQL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<string> Get()
        {
            DataTable table = new DataTable();


            string query = @"EXEC SP_AUTHTABLE";

            string sqldataSource = _configuration.GetConnectionString("Con")!;
            SqlDataReader sqlreader;
            await using(SqlConnection mycon = new SqlConnection(sqldataSource))
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
            
            return JsonConvert.SerializeObject(table);
        }
        
        [HttpGet("ozelyetkiler")]
        public async Task<string> GetOzelYetki()
        {
            DataTable table = new DataTable();
            string query = @"EXEC SP_PRIVATEAUTHTABLE";
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
            return JsonConvert.SerializeObject(table);
        }
        [HttpGet("rolozelyetkiler")]
        public async Task<string> GetRolOzelYetki()
        {
            DataTable table = new DataTable();
            string query = @"EXEC SP_ROLEPRIVATEAUTHTABLE";
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
            return JsonConvert.SerializeObject(table);
        }
        [HttpGet("ozel/detay")]
        public async Task<string> GetOZEL()
        {
            DataTable table = new DataTable();


            string query = @"SELECT * FROM TBL_PRIVATEAUTHDETAILS";

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
            return JsonConvert.SerializeObject(table);
        }
        [HttpGet("ozelyetki")]
        public async Task<string> GetOZELYETKILER()
        {
            DataTable table = new DataTable();


            string query = @"EXEC SP_PRIVATEAUTHTABLE";

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
            return JsonConvert.SerializeObject(table);
        }
        [HttpGet("Kontrol/{userid}/{moduleid}")]
        public async Task<string> GetKontrol(int userid, int moduleid)
        {
            DataTable table = new DataTable();


            string query = @"EXEC SP_AUTHCONTROL " + userid + "," + moduleid;

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
            return JsonConvert.SerializeObject(table);
        }
        [HttpGet("Ekle/{userid}/{moduleid}/{insid}")]
        public string GetEkle(int userid, int moduleid,int insid)
        {
            try
            {


                string query = @"INSERT INTO TBL_AUTH(USER_ID,MODULE_ID,INS_USER_ID)  VALUES(" + userid + "," + moduleid + ","+insid+")";

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

            }
            catch (System.Exception e)
            {
                var m = e.Message;
                return "BAŞARISIZ";
            }
            return "BAŞARILI";
        }
        [HttpGet("Add/{userid}/{moduleid}/{insid}")]
        public string Add(int userid, int moduleid, int insid)
        {
            try
            {


                string query = $"EXEC SP_AUTHONMAIL {userid},{moduleid},{insid}";

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

            }
            catch (System.Exception e)
            {
                var m = e.Message;
                return "BAŞARISIZ";
            }
            return "BAŞARILI";
        }
        [HttpGet("copyrole/{userid}/{id}/{insid}")]
        public string GetCopy(int userid, int id,int insid)
        {
            try
            {


                string query = @"SP_COPYAUTHROLE " + userid + "," + id+","+insid;

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

            }
            catch (System.Exception e)
            {
                var m = e.Message;
                return "BAŞARISIZ";
            }
            return "BAŞARILI";
        }
        [HttpGet("ozelyetki/{moduleid}/{userid}")]
        public async Task<IResponse> OzelKontrol(int moduleid, int userid)
        {
            try
            {
                DataTable table = new DataTable();


                string query = @"EXEC SP_GETPRIVATEAUTH " + moduleid + "," + userid;

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
        [HttpGet("copyuser/{userid1}/{userid2}/{insid}")]
        public string GetCopyUser(int userid1, int userid2,int insid)
        {
            try
            {


                string query = @"SP_COPYAUTHUSER " + userid1 + "," + userid2+","+insid;

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

            }
            catch (System.Exception e)
            {
                var m = e.Message;
                return "BAŞARISIZ";
            }
            return "BAŞARILI";
        }
        [HttpGet("Sil/{userid}/{moduleid}")]
        public string GetSil(int userid, int moduleid)
        {
            try
            {


                string query = @"DELETE FROM TBL_AUTH WHERE USER_ID= " + userid + " AND MODULE_ID=" + moduleid;

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

            }
            catch (System.Exception)
            {

                return "BAŞARISIZ";
            }
            return "BAŞARILI";
        }
        [HttpGet("ozel/Ekle/{userid}/{moduleid}/{insid}")]
        public string GetOZELEkle(int userid, int moduleid,int insid)
        {
            try
            {
                string query = @"INSERT INTO TBL_PRIVATEAUTH(USER_ID,MODULE_ID,INS_USER_ID)  VALUES(" + userid + "," + moduleid + ","+insid+")";

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

            }
            catch (System.Exception e)
            {
                var m = e.Message;
                return "BAŞARISIZ";
            }
            return "BAŞARILI";
        }
        [HttpGet("ozelrol/Ekle/{roleid}/{moduleid}/{insid}")]
        public string GetOZELRolEkle(int roleid, int moduleid, int insid)
        {
            try
            {
                string query = @"INSERT INTO TBL_ROLEPRIVATEAUTH(ID,MODULE_ID,INS_USER_ID)  VALUES(" + roleid + "," + moduleid + "," + insid + ")";

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

            }
            catch (System.Exception e)
            {
                var m = e.Message;
                return "BAŞARISIZ";
            }
            return "BAŞARILI";
        }
       
        [HttpGet("ozelrol/Sil/{roleid}/{moduleid}")]
        public string GetOZELRolSil(int roleid, int moduleid)
        {
            try
            {
                string query = @"DELETE FROM TBL_ROLEPRIVATEAUTH WHERE ID= " + roleid + " AND MODULE_ID=" + moduleid;

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

            }
            catch (System.Exception e)
            {
                var m = e.Message;
                return "BAŞARISIZ";
            }
            return "BAŞARILI";
        }
        [HttpGet("ozel/Sil/{userid}/{moduleid}")]
        public string GetOZELSil(int userid, int moduleid)
        {
            try
            {


                string query = @"DELETE FROM TBL_PRIVATEAUTH WHERE USER_ID= " + userid + " AND MODULE_ID=" + moduleid;

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

            }
            catch (System.Exception)
            {

                return "BAŞARISIZ";
            }
            return "BAŞARILI";
        }
    }
}
