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
        [HttpGet]
        public async Task<IResponse> GetCustomers()
        {
            try
            {
                DataTable table = new DataTable();


                string query = $@"EXEC SP_CUSTOMERS";

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
        [HttpGet("Calendar/{plasiyer?}")]
        public async Task<IResponse> GetCalendar(int? plasiyer)
        {
            try
            {
                DataTable table = new DataTable();


                string query = $@"EXEC SP_CUSTOMERCALENDAR {(plasiyer!=null?plasiyer:"")}";

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
                Result.MUSTERI_IL = Request.MUSTERI_IL;
                Result.MUSTERI_ILCE = Request.MUSTERI_ILCE;
                Result.MUSTERI_ADRES = (Request.MUSTERI_ADRES == "" ? null : Request.MUSTERI_ADRES);
                Result.DUZELTME_YAPAN_KULLANICI = Request.DUZELTME_YAPAN_KULLANICI;
                Result.MUSTERI_NOTU = (Request.MUSTERI_NOTU==""?null: Request.MUSTERI_NOTU);
                Result.MUSTERI_TEL1 = (Request.MUSTERI_TEL1 == "" ? null : Request.MUSTERI_TEL1);
                Result.MUSTERI_MAIL = (Request.MUSTERI_MAIL == "" ? null : Request.MUSTERI_MAIL);
                Result.MUSTERI_NITELIK = Request.MUSTERI_NITELIK;
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

                
                await _Context.AddAsync(Request);
                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı.", "Müşteri güncellendi.");
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

                if (Result is null)
                return new ErrorResponse("Müşteri bulunamadı.");
                _Context.Remove(Result);
                await _Context.SaveChangesAsync();

                return new SuccessResponse<string>("Başarılı.", "Müşteri başarılı bir şekilde silindi.");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
    }
}
