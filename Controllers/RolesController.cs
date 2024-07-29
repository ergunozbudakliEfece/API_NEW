using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

namespace SQL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        
        private readonly IConfiguration _configuration;
        public RolesController( IConfiguration configuration)
        {
            
            _configuration = configuration;
        }


        
        [HttpGet("names")]
        public string GetRoleNames()
        {
            DataTable table = new DataTable();


            string query = @"SELECT ID,NAME FROM TBL_ROLESDETAILS";

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
        [HttpGet("yetkiler")]
        public string GetRoleYetki()
        {
            DataTable table = new DataTable();


            string query = @"EXEC SP_ROLESAUTHTABLE";

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
        [HttpGet("Ekle/{id}/{moduleid}/{insid}")]
        public string GetEkle(int id, int moduleid,int insid)
        {
            try
            {


                string query = @"INSERT INTO TBL_ROLEAUTH(ROLE_ID,MODULE_ID,INS_USER_ID)  VALUES(" + id + "," + moduleid + ","+insid+")";

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
        [HttpGet("Sil/{id}/{moduleid}")]
        public string GetSil(int id, int moduleid)
        {
            try
            {


                string query = @"DELETE FROM TBL_ROLEAUTH WHERE ROLE_ID= " + id + " AND MODULE_ID=" + moduleid;

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
        

        [HttpGet("ekle/{rolName}/{insid}")]
        public string Update(string rolName,int insid)
        {
            try
            {


                string query = @"INSERT INTO TBL_ROLESDETAILS(NAME,INS_USER_ID) VALUES('" + rolName + "',"+insid+")";

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
        [HttpGet("sil/{id}")]
        public string Sil(int id)
        {
            try
            {


                string query = @"DELETE FROM TBL_ROLESDETAILS WHERE ID=" + id;

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
    }
}
