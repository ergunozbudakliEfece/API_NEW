using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SQL_API.Context;
using SQL_API.Wrappers.Abstract;
using SQL_API.Wrappers.Concrete;
using System.Data;
using System.Data.SqlClient;

namespace SQL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext _Context;
        private readonly IConfiguration _configuration;
        public StockController(ApplicationDbContext Context, IConfiguration configuration)
        {
            _Context = Context;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IResponse> GetStocks()
        {   
            try
            {
                DataTable table = new DataTable();


                string query = $@"EXEC SP_STOCKS";

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
        [HttpGet("importexport")]
        public async Task<IResponse> GetImportExport()
        {
            try
            {
                DataTable table = new DataTable();


                string query = $@"SP_LAST3IMPORTEXPORT";

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
        [HttpGet("pricedetails")]
        public async Task<IResponse> GetOzel()
        {
            try
            {
                DataTable table = new DataTable();


                string query = @"SELECT * FROM TBL_PRICEPRIVATECONDITION WITH(NOLOCK)";

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
        [HttpGet("prices")]
        public async Task<IResponse> GetStokFiat()
        {
            try
            {
                DataTable table = new DataTable();


                string query = $@"SELECT * FROM EFECE2023..NOVA_VW_MEVCUT_STOK_FIYAT WITH(NOLOCK)";

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
        [HttpGet("Groups")]
        public async Task<IResponse> GetGroups()
        {
            try
            {
                DataTable table = new DataTable();


                string query = $@"SELECT GRUP_KOD AS GRUP_KODU,EFECE2023.DBO.TRK(GRUP_ISIM) AS GRUP_ISIM FROM EFECE2023..TBLSTGRUP WITH (NOLOCK) WHERE GRUP_ISIM IS NOT NULL AND GRUP_KOD IN(SELECT DISTINCT(GRUP_KODU) FROM EFECE2023..TBLSTSABIT ST WITH (NOLOCK) INNER JOIN EFECE2023..TBLSTHAR AS HAR WITH (NOLOCK) ON HAR.STOK_KODU = ST.STOK_KODU AND HAR.DEPO_KODU IN (1, 5, 8, 45) WHERE GRUP_KODU IS NOT NULL) AND GRUP_KOD NOT IN('0137','0135','0006') ORDER BY EFECE2023.DBO.TRK(GRUP_ISIM)";

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
