using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class CustomersController : Controller
    {
        private readonly NOVAEFECEDbContext _Context;
        private readonly IConfiguration _configuration;
        public CustomersController(NOVAEFECEDbContext Context, IConfiguration configuration)
        {
            _Context = Context;
            _configuration = configuration;
        }
        [HttpGet("{type}")]
        public async Task<IResponse> GetCustomers(string? type)
        {
            try
            {
                DataTable table = new DataTable();


                string query = $@"EXEC SP_CUSTOMERS {type}";

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
        [HttpGet("exp/{type}")]
        public async Task<IResponse> GetCustomersExp(string? type)
        {
            try
            {
                DataTable table = new DataTable();


                string query = $@"EXEC SP_CUSTOMERSEXP {type}";

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
        [HttpGet("Sectors")]
        public async Task<IResponse> GetSectors()
        {
            try
            {
                 
                return new SuccessResponse<string>(JsonConvert.SerializeObject(_Context.TBL_SECTORS), "Başarılı");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
        [HttpGet("Calendar/{plasiyer?}/{type?}")]
        public async Task<IResponse> GetCalendar(int? plasiyer,string? type)
        {
            try
            {
                DataTable table = new DataTable();


                string query = $@"EXEC SP_CUSTOMERCALENDAR {(plasiyer!=null?plasiyer:0)}{(type != null ? ","+type : ",TR")}";

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
        [HttpGet("exp/Calendar/{plasiyer?}/{type?}")]
        public async Task<IResponse> GetCalendarExp(int? plasiyer, string? type)
        {
            try
            {
                DataTable table = new DataTable();


                string query = $@"EXEC SP_CUSTOMERCALENDAREXP {(plasiyer != null ? plasiyer : 0)}{(type != null ? "," + type : ",TR")}";

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
        [HttpGet("Qualification")]
        public async Task<IResponse> GetQualification()
        {
            try
            {

                return new SuccessResponse<string>(JsonConvert.SerializeObject(_Context.TBL_QUALIFICATION), "Başarılı");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
        [HttpPost, Route("Update")]
        public async Task<IResponse> UpdateCustomer(CustomerModel Request)
        {
            try
            {
                
                var Result = await _Context.TBL_CUSTOMERS.Where(x => x.MUSTERI_ID == Request.MUSTERI_ID).FirstOrDefaultAsync();

                if (Result is null)
                    return new ErrorResponse("Müşteri bulunamadı.");

                Result.MUSTERI_SEKTOR=Request.MUSTERI_SEKTOR;
                Result.MUSTERI_ADI= Request.MUSTERI_ADI;
                Result.MUSTERI_IL = Request.MUSTERI_IL;
                Result.MUSTERI_ILCE = Request.MUSTERI_ILCE;
                Result.MUSTERI_ADRES = (Request.MUSTERI_ADRES == "" ? null : Request.MUSTERI_ADRES);
                Result.DUZELTME_YAPAN_KULLANICI = Request.DUZELTME_YAPAN_KULLANICI;
                Result.MUSTERI_NOTU = (Request.MUSTERI_NOTU==""?null: Request.MUSTERI_NOTU);
                Result.MUSTERI_TEL1 = (Request.MUSTERI_TEL1 == "" ? null : Request.MUSTERI_TEL1);
                Result.MUSTERI_MAIL = (Request.MUSTERI_MAIL == "" ? null : Request.MUSTERI_MAIL);
                Result.ILETISIM_KANALI = Request.ILETISIM_KANALI;
                Result.MUSTERI_NITELIK = Request.MUSTERI_NITELIK;
                Result.URUNLER = Request.URUNLER;
                _Context.Update(Result);
                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı.", "Müşteri güncellendi.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
        [HttpPost, Route("Add")]
        public async Task<IResponse> AddCustomer(CustomerModel Request)
        {
            try
            {
                Request.SILINDIMI = false;
                Request.YURTICIDISI = "1";
                await _Context.AddAsync(Request);
                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı.", "Müşteri eklendi.");
            }
            catch (Exception Ex)
            {
                string Detail = $"{Ex.Message} {(Ex.InnerException is not null ? $"(Detail: {Ex.InnerException.Message})" : "")}";
                return new ErrorResponse(Detail);
            }
        }
        [HttpPost, Route("exp/Add")]
        public async Task<IResponse> AddCustomerExp(CustomerModel Request)
        {
            try
            {
                Request.SILINDIMI = false;
                Request.YURTICIDISI = "2";
                await _Context.AddAsync(Request);
                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı.", "Müşteri eklendi.");
            }
            catch (Exception Ex)
            {
                string Detail = $"{Ex.Message} {(Ex.InnerException is not null ? $"(Detail: {Ex.InnerException.Message})" : "")}";
                return new ErrorResponse(Detail);
            }
        }
        [HttpPost, Route("Meeting/Add")]
        public async Task<IResponse> AddMeeting(MeetingModel Request)
        {
            try
            {
                Request.PLANLANAN_TARIH= ((DateTime)Request.PLANLANAN_TARIH!).AddHours(3);
                Request.YURTICIDISI = "1";
                await _Context.AddAsync(Request);
                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı.", "Randevu eklendi.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
        [HttpPost, Route("exp/Meeting/Add")]
        public async Task<IResponse> AddMeetingExp(MeetingModel Request)
        {
            try
            {
                Request.PLANLANAN_TARIH = ((DateTime)Request.PLANLANAN_TARIH!).AddHours(3);
                Request.YURTICIDISI = "2";
                await _Context.AddAsync(Request);
                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı.", "Randevu eklendi.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
        [HttpPost, Route("Meeting/Update")]
        public async Task<IResponse> UpdateCustomer(MeetingModel Request)
        {
            try
            {

                var Result = await _Context.TBL_CUSTOMERCALENDAR.Where(x => x.INCKEY == Request.INCKEY).FirstOrDefaultAsync();

                if (Result is null)
                    return new ErrorResponse("Müşteri bulunamadı.");

                Result.MUSTERI_ID = Request.MUSTERI_ID;
                Result.PLANLANAN_TARIH = ((DateTime)Request.PLANLANAN_TARIH!).AddHours(3);
                Result.RANDEVU_NOTU = Request.RANDEVU_NOTU;
                Result.DEGISIKLIK_YAPAN_KULLANICI_ID = Request.DEGISIKLIK_YAPAN_KULLANICI_ID;
                Result.SUREC = Request.SUREC;
                Result.PLASIYER = Request.PLASIYER;
                Result.ILETISIM_KANALI = Request.ILETISIM_KANALI;
                _Context.Update(Result);
                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı.", "Müşteri güncellendi.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
        [HttpGet("duration/{type}")]
        public async Task<IResponse> GetDuration(string type)
        {
            try
            {
                DataTable table = new DataTable();
                string query;
                if (type == "TR")
                {
                    query = "SELECT ID,NAME,COLOR FROM TBL_CUSTOMERDURATION WITH(NOLOCK)";
                }
                else
                {
                    query = "SELECT ID,RTRIM(LTRIM(NAME_EN)) AS NAME,COLOR FROM TBL_CUSTOMERDURATION WITH(NOLOCK)";
                }
                

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
        [HttpGet("contact/{type}")]
        public async Task<IResponse> GetContacts(string type)
        {
            try
            {
                DataTable table = new DataTable();

                string query;

                query = (type =="TR"? @"SELECT ID,NAME FROM TBL_CONTACTTYPE WITH(NOLOCK)" :  @"SELECT ID,NAME_EN AS NAME FROM TBL_CONTACTTYPE WITH(NOLOCK)");

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
        [HttpGet("products")]
        public async Task<IResponse> GetProducts()
        {
            try
            {
                DataTable table = new DataTable();


                string query = @"SELECT * FROM TBL_PRODUCTS WITH(NOLOCK)";

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
        [HttpPost, Route("Delete/{MusteriID}")]
        public async Task<IResponse> DeleteCustomer(int MusteriID)
        {
            try
            {

                var Result = await _Context.TBL_CUSTOMERS.Where(x => x.MUSTERI_ID == MusteriID).FirstOrDefaultAsync();
                Result!.SILINDIMI = true;
                if (Result is null)
                return new ErrorResponse("Müşteri bulunamadı.");
                _Context.Update(Result);
                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı.", "Müşteri başarılı bir şekilde silindi.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
        [HttpPost, Route("Meeting/Delete/{INCKEY}")]
        public async Task<IResponse> DeleteMeeting(int INCKEY)
        {
            try
            {

                var Result = await _Context.TBL_CUSTOMERCALENDAR.Where(x => x.INCKEY == INCKEY).FirstOrDefaultAsync();

                if (Result is null)
                    return new ErrorResponse("Randevu bulunamadı.");
                _Context.Remove(Result);
                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı.", "Randevu başarılı bir şekilde silindi.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
    }
}
