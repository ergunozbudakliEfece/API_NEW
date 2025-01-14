using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    public class PermitController : ControllerBase
    {
        private readonly ApplicationDbContext _Context;
        private readonly IConfiguration _configuration;
        private readonly NOVAEFECEDbContext _efeceDB;
        public PermitController(ApplicationDbContext Context, IConfiguration configuration, NOVAEFECEDbContext efeceDB)
        {
            _Context = Context;
            _configuration = configuration;
            _efeceDB = efeceDB;
        }
        [HttpGet("{UserID?}")]
        public async Task<IResponse> GetPermits(string? UserID)
        {
            try
            {
                DataTable table = new DataTable();


                string query = $@"SP_PERMITREQUEST {UserID}";
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
        [HttpPost("Accept")]
        public async Task<IResponse> SetAccept([FromBody]AcceptModel accept)
        {
            
            try
            {
                string query = $@"UPDATE TBL_PERMITACCEPT SET ACCEPT='{accept.ACCEPT}' WHERE PERMIT_ID={accept.PERMIT_ID} AND ACCEPT_ID={accept.ACCEPT_ID}";
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
                return new SuccessResponse<string>("Başarılı", "Başarılı");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }
        [HttpGet("ByID/{PermitID?}")]
        public async Task<IResponse> GetPermitsByID(string? PermitID)
        {
            try
            {
                DataTable table = new DataTable();


                string query = $@"SELECT * FROM TBL_PERMITACCEPT WHERE PERMIT_ID={PermitID}";
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
        [HttpGet("Managers")]
        public async Task<IResponse> GetManagers()
        {
            try
            {
                DataTable table = new DataTable();


                string query = $@"SELECT ACCEPT_ID,NAME=(SELECT USER_FIRSTNAME+' '+USER_LASTNAME FROM NOVA_YENI..TBL_USERDATA UD WHERE PD.ACCEPT_ID=UD.USER_ID) FROM [TBL_PERMITACCEPTDIST] PD GROUP BY ACCEPT_ID";
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
        [HttpGet("Emps/{USER_ID}")]
        public async Task<IResponse> GetEmps(int USER_ID)
        {
            try
            {
                DataTable table = new DataTable();


                string query = $@"SELECT USER_ID,NAME=(SELECT USER_FIRSTNAME+' '+USER_LASTNAME FROM NOVA_YENI..TBL_USERDATA UD WHERE PG.USER_ID=UD.USER_ID) FROM [NOVA_EFECE].[dbo].TBL_PERMITUSERGROUP PG WHERE GROUP_ID IN(SELECT GROUP_ID FROM [NOVA_EFECE].[dbo].[TBL_PERMITACCEPTDIST] PD WHERE ACCEPT_ID={USER_ID} GROUP BY GROUP_ID)";
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
        [HttpGet("PermitControl/{PermitID}/{Priority}")]
        public async Task<IResponse> GetPermitsByID(int PermitID,int Priority)
        {
            try
            {
                DataTable table = new DataTable();


                string query = $@" SP_PERMITCONTROL {PermitID},{Priority}";
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
        [HttpPost]
        public async Task<IResponse> SetPermits(PermitModel model)
        {
            try
            {


                DataTable table = new DataTable();
                string query = $@"SP_INSERTPERMIT {model.USER_ID},{model.SUBS_ID},'{model.BASLANGIC_TARIHI}','{model.BITIS_TARIHI}','{model.ISE_DONUS}',{model.PERMIT_TYPE},{model.OFFDAY},N'{model.EXP}'";
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
        [HttpGet("Type/{type}")]
        public async Task<IResponse> GetType(int type)
        {
            try
            {


                DataTable table = new DataTable();
                string query = $@"SELECT * FROM TBL_PERMITTYPES WHERE TYPE_ID={type}";
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
        [HttpGet("Types")]
        public async Task<IResponse> GetTypes()
        {
            try
            {


                DataTable table = new DataTable();
                string query = $@"SELECT * FROM TBL_PERMITTYPES";
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
        [HttpPost("hourly")]
        public async Task<IResponse> SetHourlyPermits(PermitModel model)
        {
            try
            {


                DataTable table = new DataTable();
                string query = $@"SP_INSERTPERMITHR {model.USER_ID},'{model.BASLANGIC_TARIHI}','{model.BITIS_TARIHI}','{model.PERMIT_TYPE}',N'{model.EXP}'";
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
