using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQL_API.Context;
using SQL_API.Models;
using SQL_API.Wrappers.Abstract;
using SQL_API.Wrappers.Concrete;
using System.Globalization;

namespace SQL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly NOVAEFECEDbContext _context;

        public ShiftController(NOVAEFECEDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IResponse> GetShifts() 
        {
            try
            {
                List<ShiftModel> result = await _context.TBL_SHIFTS.ToListAsync();

                return new SuccessResponse<List<ShiftModel>>(result, "");
            }
            catch (Exception ex)
            {
                return new ErrorResponse(ex);
            }
        }

        [HttpGet("GetShiftDetails")]
        public async Task<IResponse> GetShiftDetails(int shiftId) 
        {
            try 
            {
                List<ShiftDetailModel> result = await _context.TBL_SHIFTDETAILS.Where(x => x.SHIFT_ID == shiftId).ToListAsync();

                return new SuccessResponse<List<ShiftDetailModel>>(result, "");
            }
            catch (Exception ex)
            {
                return new ErrorResponse(ex);
            }
        }

        [HttpPost("Create")]
        public async Task<IResponse> CreateShift(ShiftCreateModel createRequest) 
        {
            using (var contextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var createdShift = await _context.TBL_SHIFTS.AddAsync(new ShiftModel() { NAME = createRequest.Name, INS_USER_ID = createRequest.CreatedBy, INS_DATE = createRequest.CreateDate });

                    await _context.SaveChangesAsync();

                    int createdShiftId = createdShift.Entity.ID;

                    int workId = 0;

                    DateTime earliestDate = createRequest.Details.Min(s => DateTime.ParseExact(s.StartDate, "dd.MM.yyyy", CultureInfo.InvariantCulture));
                    DateTime latestDate = createRequest.Details.Max(s => DateTime.ParseExact(s.EndDate, "dd.MM.yyyy", CultureInfo.InvariantCulture));
                    TimeSpan startOfDay = TimeSpan.Parse("00:00");
                    TimeSpan endOfDay = new TimeSpan(23, 59, 59);

                    Dictionary<int, List<ShiftDetailCreateModel>> groupedShifts = createRequest.Details.GroupBy(x => x.TargetDay).ToDictionary(g => g.Key, g => g.ToList());

                    List<ShiftItem> workshifts = new List<ShiftItem>();

                    for (DateTime date = earliestDate; date <= latestDate; date = date.AddDays(1))
                    {
                        if (groupedShifts.ContainsKey((int)date.DayOfWeek))
                        {
                            IEnumerable<ShiftDetailCreateModel> details = groupedShifts[(int)date.DayOfWeek].Where(g => DateTime.ParseExact(g.StartDate, "dd.MM.yyyy", CultureInfo.InvariantCulture) <= date && DateTime.ParseExact(g.EndDate, "dd.MM.yyyy", CultureInfo.InvariantCulture) >= date).OrderBy(d => TimeSpan.Parse(d.StartTime));

                            bool anyOtherDayShift = details.Any(s => TimeSpan.Parse(s.StartTime) > TimeSpan.Parse(s.EndTime));

                            foreach (ShiftDetailCreateModel detail in details)
                            {
                                ShiftItem? lastShift = workshifts.LastOrDefault();

                                TimeSpan startTime = TimeSpan.Parse(detail.StartTime);
                                TimeSpan endTime = TimeSpan.Parse(detail.EndTime);

                                DateTime startDate = date.Add(startTime);
                                DateTime endDate = date.Add(endTime);

                                workId = workId + 1;

                                if (startTime > endTime)
                                {
                                    endDate = endDate.AddDays(1);

                                    DateTime endOfStartDay = startDate.Date.Add(endOfDay);
                                    DateTime startOfEndDay = endDate.Date.Add(startOfDay);

                                    if (lastShift is not null)
                                    {
                                        if (lastShift.EndDate.ToString("HH:mm:ss").Equals("23:59:59"))
                                        {
                                            workshifts.Add(new ShiftItem(0, 0, lastShift.EndDate.Date.AddDays(1), startDate));
                                        }
                                        else
                                        {
                                            workshifts.Add(new ShiftItem(0, 0, lastShift.EndDate, startDate));
                                        }
                                    }
                                    else
                                    {
                                        workshifts.Add(new ShiftItem(0, 0, startDate.Date, startDate));
                                    }

                                    workshifts.Add(new ShiftItem(1, workId, startDate, endOfStartDay));
                                    workshifts.Add(new ShiftItem(1, workId, startOfEndDay, endDate));
                                }
                                else
                                {
                                    if (anyOtherDayShift)
                                    {
                                        if (lastShift is not null)
                                        {
                                            if (lastShift.EndDate.Date != startDate.Date)
                                            {
                                                workshifts.Add(new ShiftItem(0, 0, startDate.Date, startDate));
                                            }
                                            else
                                            {
                                                workshifts.Add(new ShiftItem(0, 0, lastShift.EndDate, startDate));
                                            }
                                        }
                                        else
                                        {
                                            workshifts.Add(new ShiftItem(0, 0, startDate.Date, startDate));
                                        }

                                        workshifts.Add(new ShiftItem(1, workId, startDate, endDate));
                                    }
                                    else
                                    {
                                        DateTime endOfStartDay = startDate.Date.Add(endOfDay);
                                        DateTime startOfEndDay = endDate.Date.AddDays(1).Add(startOfDay);

                                        if (lastShift is not null)
                                        {
                                            if (lastShift.EndDate.Date != startDate.Date)
                                            {
                                                workshifts.Add(new ShiftItem(0, 0, startDate.Date, startDate));
                                            }
                                            else
                                            {
                                                workshifts.Add(new ShiftItem(0, 0, lastShift.EndDate, startDate));
                                            }
                                        }
                                        else
                                        {
                                            workshifts.Add(new ShiftItem(0, 0, startDate.Date, startDate));
                                        }

                                        workshifts.Add(new ShiftItem(1, workId, startDate, endDate));
                                        workshifts.Add(new ShiftItem(0, 0, endDate, endOfStartDay));
                                    }
                                }
                            }

                            if (!details.Any())
                            {
                                ShiftItem? lastShift = workshifts.LastOrDefault();

                                if (lastShift is not null)
                                {
                                    if (lastShift.EndDate.ToString("HH:mm:ss").Equals("23:59:59"))
                                    {
                                        workshifts.Add(new ShiftItem(0, -1, lastShift.EndDate.Date.AddDays(1), date.Date.Add(endOfDay)));
                                    }
                                    else
                                    {
                                        workshifts.Add(new ShiftItem(0, 0, lastShift.EndDate, date.Date.Add(endOfDay)));
                                    }
                                }
                                else
                                {
                                    workshifts.Add(new ShiftItem(0, -1, date.Date.Date, date.Date.Add(endOfDay)));
                                }
                            }
                        }
                        else
                        {
                            ShiftItem? lastShift = workshifts.LastOrDefault();

                            if (lastShift is not null)
                            {
                                if (lastShift.EndDate.ToString("HH:mm:ss").Equals("23:59:59"))
                                {
                                    workshifts.Add(new ShiftItem(0, -1, date.Date.Date, date.Date.Add(endOfDay)));
                                }
                                else
                                {
                                    workshifts.Add(new ShiftItem(0, 0, lastShift.EndDate, date.Date.Add(endOfDay)));
                                }
                            }
                            else
                            {
                                workshifts.Add(new ShiftItem(0, -1, date.Date.Date, date.Date.Add(endOfDay)));
                            }
                        }
                    }

                    foreach (var workshift in workshifts)
                    {
                        _context.TBL_SHIFTDETAILS.Add(new()
                        {
                            SHIFT_ID = createdShiftId,
                            START_DATE = workshift.StartDate,
                            END_DATE = workshift.EndDate,
                            TYPE = workshift.Type,
                            WORK_GROUP = workshift.Group
                        });

                        await _context.SaveChangesAsync();
                    }

                    await contextTransaction.CommitAsync();
                    return new SuccessResponse<ShiftModel>(createdShift.Entity, "");
                }
                catch (Exception ex)
                {
                    await contextTransaction.RollbackAsync();

                    return new ErrorResponse(ex);
                }
            }
        }

        [HttpDelete]
        public async Task<IResponse> DeleteShift(int shiftId) 
        {
            using (var contextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    ShiftModel? shift = await _context.TBL_SHIFTS.FirstOrDefaultAsync(s => s.ID == shiftId);

                    if (shift is not null) 
                    { 
                        _context.TBL_SHIFTS.Remove(shift);
                    }

                    List<ShiftDetailModel> shiftDetails = await _context.TBL_SHIFTDETAILS.Where(s => s.SHIFT_ID == shiftId).ToListAsync();

                    _context.TBL_SHIFTDETAILS.RemoveRange(shiftDetails);

                    await _context.SaveChangesAsync();

                    await contextTransaction.CommitAsync();
                    return new SuccessResponse<string>("", "");
                }
                catch (Exception ex)
                {
                    await contextTransaction.RollbackAsync();
                    return new ErrorResponse(ex);
                }
            }
        }
    }
}
