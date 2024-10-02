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
    public class AttendanceController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly NOVAEFECEDbContext _context;

        public AttendanceController(IConfiguration configuration, NOVAEFECEDbContext Context)
        {
            _configuration = configuration;
            _context = Context;
        }

        [HttpGet("{UserID}")]
        public async Task<IResponse> GetAttendance(int UserID)
        {
            try
            {
                DataTable table = new DataTable();

                string query = $@"EXEC SP_ATTENDANCE {UserID}";

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

        [HttpGet("Puantaj")]
        public async Task<IResponse> GetPuantaj()
        {
            try
            {
                DataTable table = new DataTable();

                string query = $@"EXEC SP_ATTENDANCEVIEW";

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

        [HttpPost("Create")]
        public async Task<IResponse> Create([FromBody] List<AttendanceCreateRequest> CreateRequests)
        {
            try
            {
                DataTable table = new DataTable();

                foreach (AttendanceCreateRequest CreateRequest in CreateRequests) 
                {
                    string query = $@"EXEC SP_ATTENDANCE {CreateRequest.USER_ID}, '{CreateRequest.CHECK_IN_OUT}', '{CreateRequest.DATE.ToString("yyyy-MM-dd HH:mm:ss.fff")}', {CreateRequest.INS_USER_ID} ";

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
                }

                return new SuccessResponse<string>(JsonConvert.SerializeObject(table), "Başarılı");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }

        [HttpPut("Update")]
        public async Task<IResponse> Update([FromBody] AttendanceUpdateRequest UpdateRequest) 
        {
            try
            {
                AttendanceModel? Attendance = await _context.TBL_ATTENDANCE.Where(x => x.INCKEY == UpdateRequest.INCKEY).FirstOrDefaultAsync();

                if (Attendance is not null)
                {
                    Attendance.DATE = UpdateRequest.DATE;

                    await _context.SaveChangesAsync();
                }
                else
                {
                    return new ErrorResponse("Güncellenecek kayıt bulunamadı.");
                }

                return new SuccessResponse<string>("", "");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IResponse> Delete(int Id)
        {
            try
            {
                AttendanceModel? Attendance = await _context.TBL_ATTENDANCE.Where(x => x.INCKEY == Id).FirstOrDefaultAsync();

                if (Attendance is not null)
                {
                    _context.Remove(Attendance);
                    await _context.SaveChangesAsync();
                }
                else 
                {
                    return new ErrorResponse("Silinecek kayıt bulunamadı.");
                }

                return new SuccessResponse<string>("", "");
            }
            catch (Exception Ex) 
            {
                return new ErrorResponse(Ex);
            }
        }


        [HttpDelete("DeleteRange")]
        public async Task<IResponse> DeleteRange([FromBody] List<int> IDList)
        {
            try
            {
                List<int> DeletedIDList = new List<int>();
                foreach (int Id in IDList) 
                {
                    AttendanceModel? Attendance = await _context.TBL_ATTENDANCE.Where(x => x.INCKEY == Id).FirstOrDefaultAsync();

                    if (Attendance is not null)
                    {
                        _context.Remove(Attendance);

                        DeletedIDList.Add(Id);
                    }
                }

                await _context.SaveChangesAsync();

                return new SuccessResponse<List<int>>(DeletedIDList, "");
            }
            catch (Exception Ex)
            {
                return new ErrorResponse(Ex);
            }
        }

        [HttpPost("Condition")]
        public async Task<IResponse> Condition([FromBody] AttendanceConditionRequest ConditionRequest)
        {
            try
            {
                DataTable table = new DataTable();

                string query = $@"EXEC SP_ATTENDANCECOND {ConditionRequest.USER_ID}, '{ConditionRequest.DATE.ToString("yyyy-MM-dd HH:mm")}' ${(ConditionRequest.UPDATE ? ", 1" : "")}";

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