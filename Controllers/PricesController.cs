using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SQL_API.Context;
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
        public PricesController(ApplicationDbContext Context, IConfiguration configuration)
        {
            _Context = Context;
            _configuration = configuration;
        }
        [HttpGet("conditions")]
        public async Task<IResponse> FiyatDurum()
        {
            try
            {
                DataTable table = new DataTable();


                string query = $@"EXEC SP_PRICECONDITION";

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




            string query1 = @"EXEC UPDATESTOKFIAT '" + fiyatkodu + "','" + fiyat2 + "','" + fiyat3 + "','" + listekodu + "'," + userid;

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


                string query = $@"EXEC SP_PRICES";

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
    }
}
