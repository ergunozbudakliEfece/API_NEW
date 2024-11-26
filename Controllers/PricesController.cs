using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SQL_API.Context;
using SQL_API.Models;
using SQL_API.Wrappers.Abstract;
using SQL_API.Wrappers.Concrete;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace SQL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricesController : ControllerBase
    {
        private readonly ApplicationDbContext _Context;
        private readonly IConfiguration _configuration;
        private readonly NOVAEFECEDbContext _efeceDB;
        public PricesController(ApplicationDbContext Context, IConfiguration configuration, NOVAEFECEDbContext efeceDB)
        {
            _Context = Context;
            _configuration = configuration;
            _efeceDB = efeceDB;
        }
        [HttpPost]
        public IActionResult Create(List<FiyatModel> item)
        {

            using (SqlConnection connection = new SqlConnection(
           _configuration.GetConnectionString("NOVA_EFECE")!))
            {
                SqlCommand command = new SqlCommand("DELETE FROM TBL_PRICEORDER", connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();
                for (int i = 0; i < item.Count(); i++)
                {
                    SqlCommand command1 = new SqlCommand("INSERT INTO TBL_PRICEORDER (SIRA_NO,FIYATKODU) VALUES(" + item[i].SIRA_NO + ",'" + item[i].FIYATKODU + "')", connection);
                    command1.Connection.Open();
                    command1.ExecuteNonQuery();
                    command1.Connection.Close();

                }
            }



            return new NoContentResult();
        }
        [HttpPost("insert/{priceCode}")]
        public async Task<IResponse> Insert(string priceCode)
        {
            try
            {
                DataTable table = new DataTable();


                string query = $"DECLARE @max int;SET @max=(SELECT MAX(SIRA_NO)+1 FROM [TBL_PRICEORDER]);INSERT INTO TBL_PRICEORDER VALUES(@max,'{priceCode}');INSERT INTO NOVA_EFECE..TBL_PRICEHISTORY  ([FIYATKODU],[OLCUBR],[FIYATDOVIZTIPI],[FIYAT1],[FIYAT2],[FIYAT3],[FIYAT4],[TARIH],[KAYIT_KULL_ID]) SELECT DISTINCT(FIYATKODU),OLCUBR=1,FIYATDOVIZTIPI,FIYAT1,FIYAT2,FIYAT3,FIYAT4,CASE WHEN DEGISIKTAR IS NULL THEN KAYITTAR ELSE DEGISIKTAR END AS TARIH ,KAYIT_KULL_ID=10001 FROM EFECE2023..TBLSTOKFIAT LEFT OUTER JOIN EFECE2023..TBLSTSABIT ON TBLSTOKFIAT.STOKKODU=TBLSTSABIT.STOK_KODU WHERE FIYATKODU='{priceCode}'";

                string sqldataSource = _configuration.GetConnectionString("NOVA_EFECE")!;
                SqlDataReader sqlreader;
                await using (SqlConnection mycon = new SqlConnection(sqldataSource))
                {
                    mycon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, mycon))
                    {
                        sqlreader = await myCommand.ExecuteReaderAsync();
                        sqlreader.Close();
                        mycon.Close();
                    }
                }
                return new SuccessResponse<string>("Kayıt başarılı!", "Başarılı");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
        [HttpGet("conditions")]
        public async Task<IResponse> FiyatDurum()
        {
            try
            {
                DataTable table = new DataTable();


                string query = @"EXEC SP_PRICECONDITION";

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
        [HttpGet("dovizler")]
        public async Task<IResponse> Dovizler()
        {
            try
            {
                DataTable table = new DataTable();


                string query = @"EXEC SP_GETDOV";

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
        [HttpGet("privateconditions")]
        public async Task<IResponse> OzelKosullar()
        {
            try
            {
                DataTable table = new DataTable();


                string query = $@"SELECT* FROM TBL_PRICEPRIVATECONDITION WITH(NOLOCK)";

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
        [HttpGet("{fiyatkodu}/{fiyat2}/{fiyat3}/{listekodu}/{userid}")]
        public IEnumerable? Update(string fiyatkodu, string fiyat2, string fiyat3, string listekodu, int userid)
        {




            string query1 = @"EXEC SP_UPDATESTOCKPRICES '" + fiyatkodu + "','" + fiyat2 + "','" + fiyat3 + "','" + listekodu + "'," + userid;

            string sqldataSource1 = _configuration.GetConnectionString("NOVA_EFECE")!;
            SqlDataReader sqlreader1;
            using (SqlConnection mycon1 = new SqlConnection(sqldataSource1))
            {
                mycon1.Open();
                using (SqlCommand myCommand1 = new SqlCommand(query1, mycon1))
                {
                    sqlreader1 = myCommand1.ExecuteReader();
                    sqlreader1.Close();
                    mycon1.Close();
                }
            }



            return null;
        }
        [HttpGet]
        public async Task<IResponse> Fiyatlar()
        {
            try
            {
                DataTable table = new DataTable();


                string query = @"EXEC SP_PRICES";

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
        [HttpGet("setted")]
        public async Task<IResponse> GetSetted()
        {
            try
            {
                var data=await _efeceDB.SETTED.ToListAsync();
                return new SuccessResponse<string>(JsonConvert.SerializeObject(data), "Başarılı");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
        [HttpGet("aciklama/{fiyatkodu}/{aciklama}")]
        public IEnumerable UpdateAciklama(string fiyatkodu, string aciklama)
        {
            string query = @"SELECT * FROM TBL_PRICEPRIVATECONDITION WHERE FIYATKODU='" + fiyatkodu + "'";
            List<FiatAciklamaModel> stoklist = null;
            string sqldataSource = _configuration.GetConnectionString("NOVA_EFECE")!;
            SqlDataReader sqlreader;
            using (SqlConnection mycon = new SqlConnection(sqldataSource))
            {
                mycon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, mycon))
                {
                    sqlreader = myCommand.ExecuteReader();
                    stoklist = DataReaderMapToList<FiatAciklamaModel>(sqlreader);

                    sqlreader.Close();
                    mycon.Close();
                }
            }
            string query1 = "";
            if (stoklist.Count == 0)
            {

                if (aciklama == "null")
                {
                    query1 = @"INSERT INTO TBL_PRICEPRIVATECONDITION VALUES('" + fiyatkodu + "')";

                }
                else
                {
                    query1 = @"INSERT INTO TBL_PRICEPRIVATECONDITION VALUES('" + fiyatkodu + "','" + aciklama + "')";
                }

                string sqldataSource1 = _configuration.GetConnectionString("NOVA_EFECE")!;
                SqlDataReader sqlreader1;
                using (SqlConnection mycon1 = new SqlConnection(sqldataSource1))
                {
                    mycon1.Open();
                    using (SqlCommand myCommand1 = new SqlCommand(query1, mycon1))
                    {
                        sqlreader1 = myCommand1.ExecuteReader();
                        sqlreader1.Close();
                        mycon1.Close();
                    }
                }
            }
            else
            {
                if (aciklama == "null")
                {
                    query1 = @"UPDATE TBL_PRICEPRIVATECONDITION SET OZELKOSULLAR = NULL WHERE FIYATKODU='" + fiyatkodu + "'";

                }
                else
                {
                    query1 = @"UPDATE TBL_PRICEPRIVATECONDITION SET OZELKOSULLAR='" + aciklama + "' WHERE FIYATKODU='" + fiyatkodu + "'";
                }

                string sqldataSource1 = _configuration.GetConnectionString("NOVA_EFECE")!;
                SqlDataReader sqlreader1;
                using (SqlConnection mycon1 = new SqlConnection(sqldataSource1))
                {
                    mycon1.Open();
                    using (SqlCommand myCommand1 = new SqlCommand(query1, mycon1))
                    {
                        sqlreader1 = myCommand1.ExecuteReader();
                        sqlreader1.Close();
                        mycon1.Close();
                    }
                }
            }





            return null;
        }
        public static List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!object.Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }
        public class FiatAciklamaModel
        {
            public string FIYATKODU { set; get; }
            public string OZELKOSULLAR { set; get; }
        }
    }
}
