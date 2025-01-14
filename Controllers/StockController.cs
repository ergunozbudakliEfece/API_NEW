using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SQL_API.Context;
using SQL_API.Models;
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
        [HttpGet("All")]
        public async Task<IResponse> GetAllStocks()
        {
            try
            {
                DataTable table = new DataTable();


                string query = @"EXEC SP_GETALLSTOCKS";

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
        [HttpGet]
        public async Task<IResponse> GetStocks()
        {   
            try
            {
                DataTable table = new DataTable();


                string query = @"EXEC SP_STOCKS";

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
        [HttpGet("deger")]
        public async Task<IResponse> GetDeger()
        {
            try
            {
                DataTable table = new DataTable();


                string query = @"EXEC SP_STOKDEGER";

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


                string query = @"SP_LAST3IMPORTEXPORT";

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
        [HttpGet("counting")]
        public async Task<IResponse> GetStokCounting()
        {
            try
            {
                DataTable table = new DataTable();


                string query = $@"SP_STOCKCOUNTING";

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
        [HttpPost("addStock/counting")]
        public async Task<IResponse> AddStokCounting(StockCountingModel Request)
        {
            try
            {
                DataTable table = new DataTable();


                string query = $@"INSERT TBL_COUNTINGS(STOK_KODU,MIKTAR,MIKTAR2,SAYIM_ID,KAYIT_YAPAN_KUL) VALUES('{Request.STOK_KODU}','{Request.MIKTAR}','{Request.MIKTAR2}',{Request.SAYIM_ID},{Request.KAYIT_KULLANICI_ID})";

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
                return new SuccessResponse<string>("Stok başarıyla eklendi.", "Başarılı");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }

        }
        [HttpPost("updStock/counting")]
        public async Task<IResponse> UpdStokCounting(StockCountingModel Request)
        {
            try
            {
                DataTable table = new DataTable();


                string query = $@"UPDATE TBL_COUNTINGS SET MIKTAR='{Request.MIKTAR}', MIKTAR2='{Request.MIKTAR2}', GUNCELLEME_YAPAN_KUL={Request.GUNCELLEME_KULLANICI_ID} WHERE ID={Request.ID}";

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
                return new SuccessResponse<string>("Stok başarıyla güncellendi.", "Başarılı");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }

        }
        [HttpGet("countings/{depo?}")]
        public async Task<IResponse> GetStokCountings(string? depo=null)
        {
            try
            {
                DataTable table = new DataTable();


                string query = $@"SP_COUNTINGS {(depo is null ? "" : $"'{depo}'")}";

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
        [HttpPost("balanced")]
        public async Task<IResponse> GetBalanced(StockBalanceModel Request)
        {
            try
            {
                DataTable table = new DataTable();


                string query = $@"SP_STOCKWHCODEBALANCE '{Request.StokKodu}',{Request.DepoKodu}";

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
        [HttpPost("balanced/wh")]
        public async Task<IResponse> GetBalancedWH(StockBalanceModel Request)
        {
            try
            {
                DataTable table = new DataTable();


                string query = $@"SP_STOCKWHORDERDETAILS '{Request.StokKodu}',{Request.DepoKodu}";

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
        [HttpGet("add/countings")]
        public async Task<IResponse> AddStokCountings(StockCountingModel Request)
        {
            try
            {
                DataTable table = new DataTable();


                string query = $@"INSERT INTO TBL_COUNTINGS(STOK_KODU,MIKTAR,MIKTAR2,SAYIM_ID,KAYIT_YAPAN_KUL) VALUES('{Request.STOK_KODU}','{Request.MIKTAR}','{Request.MIKTAR2}',{Request.SAYIM_ID},{Request.KAYIT_KULLANICI_ID})";

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
                return new SuccessResponse<string>("Başarılı", "Stok başarıyla eklendi.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }

        }
        [HttpPost("update/counting")]
        public async Task<IResponse> UpdateCounting(CountingModel Request)
        {
            try
            {
                string query = $@"UPDATE TBL_STOCKCOUNTING SET DEPO={Request.DEPO},GUNCELLEME_KULLANICI_ID={Request.GUNCELLEME_KULLANICI_ID},SAYIM_ADI='{Request.SAYIM_ADI}',TOLERANS='{Request.TOLERANS}' WHERE SAYIM_ID={Request.SAYIM_ID}";

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

                return new SuccessResponse<string>("Başarılı.", "Kayıt başarıyla güncellendi.");
            }
            catch (Exception Ex)
            {
                string Detail = $"{Ex.Message} {(Ex.InnerException is not null ? $"(Detail: {Ex.InnerException.Message})" : "")}";
                return new ErrorResponse(Detail);
            }

        }
        [HttpPost("add/counting")]
        public async Task<IResponse> AddCounting(CountingModel Request)
        {
            try
            {
                string query = $@"INSERT TBL_STOCKCOUNTING(SAYIM_ADI,KAYIT_KULLANICI_ID,DEPO,TOLERANS) VALUES('{Request.SAYIM_ADI}','{Request.KAYIT_KULLANICI_ID}','{Request.DEPO}','{Request.TOLERANS}')";

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

                return new SuccessResponse<string>("Başarılı.", "Kayıt başarılı.");
            }
            catch (Exception Ex)
            {
                string Detail = $"{Ex.Message} {(Ex.InnerException is not null ? $"(Detail: {Ex.InnerException.Message})" : "")}";
                return new ErrorResponse(Detail);
            }

        }
        [HttpPost("delete/counting/{id}")]
        public async Task<IResponse> DeleteCounting(int id)
        {
            try
            {
                string query = $"DELETE FROM TBL_STOCKCOUNTING WHERE SAYIM_ID="+ id;

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

                return new SuccessResponse<string>("Başarılı.", "Başarıyla silindi.");
            }
            catch (Exception Ex)
            {
                string Detail = $"{Ex.Message} {(Ex.InnerException is not null ? $"(Detail: {Ex.InnerException.Message})" : "")}";
                return new ErrorResponse(Detail);
            }

        }
        [HttpPost("delete/countingdetay/{id}")]
        public async Task<IResponse> DeleteDetayCounting(int id)
        {
            try
            {
                string query = $"DELETE FROM TBL_COUNTINGS WHERE ID=" + id;

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

                return new SuccessResponse<string>("Başarılı.", "Başarıyla silindi.");
            }
            catch (Exception Ex)
            {
                string Detail = $"{Ex.Message} {(Ex.InnerException is not null ? $"(Detail: {Ex.InnerException.Message})" : "")}";
                return new ErrorResponse(Detail);
            }

        }
        [HttpPost("fiyat")]
        public async Task<IResponse> SetStokFiat(List<StokFiyatModel> fiyatlar)
        {
            try
            {
                for(int i = 0; i < fiyatlar.Count; i++)
                {
                    DataTable table = new DataTable();
                    string query = "";
                    if (fiyatlar[i].FIYAT_ONCE == "-")
                    {
                        query = $"INSERT INTO TBL_SETTEDPRICES VALUES('{fiyatlar[i].STOK_KODU}',{fiyatlar[i].FIYAT_SONRA})";
                    }
                    else
                    {
                        query = $"UPDATE TBL_SETTEDPRICES SET FIYAT={fiyatlar[i].FIYAT_SONRA} WHERE STOK_KODU='{fiyatlar[i].STOK_KODU}'";
                    }
                    

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
                }
                
                return new SuccessResponse<string>("Başarılı", "Başarılı");
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


                string query = $@"SELECT GRUP_KOD AS GRUP_KODU,EFECE2023.DBO.TRK(GRUP_ISIM) AS GRUP_ISIM FROM EFECE2023..TBLSTGRUP WITH (NOLOCK) WHERE GRUP_ISIM IS NOT NULL AND GRUP_KOD IN(SELECT DISTINCT(GRUP_KODU) FROM EFECE2023..TBLSTSABIT ST WITH (NOLOCK) INNER JOIN EFECE2023..TBLSTHAR AS HAR WITH (NOLOCK) ON HAR.STOK_KODU = ST.STOK_KODU WHERE GRUP_KODU IS NOT NULL) AND GRUP_KOD NOT IN('0137','0135','0006') ORDER BY EFECE2023.DBO.TRK(GRUP_ISIM)";

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
