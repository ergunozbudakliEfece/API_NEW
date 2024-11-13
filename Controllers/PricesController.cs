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

    }
}
