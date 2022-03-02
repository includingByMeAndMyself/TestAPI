using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Timesheet.Domain.Interfaces.IRepository;
using Timesheet.Domain.Models;

namespace Timesheet.DAL.MSSQL.Repositories
{
    public class TimesheetRepository : ITimesheetRepository
    {
        private readonly TimesheetContext _context;
        private readonly IMapper _mapper;

        public TimesheetRepository(TimesheetContext context, IMapper mappper)
        {
            _context = context;
            _mapper = mappper;
        }
        public void Add(TimeLog timeLog)
        {
            var timeLogEntity = _mapper.Map<Entities.TimeLog>(timeLog);

            _context.TimeLogs.Add(timeLogEntity);
            _context.SaveChanges();
        }

        public TimeLog[] GetTimeLogs(string lastName)
        {
            var timeLogs = _context.TimeLogs
                .AsNoTracking()
                .Where(x => x.LastName == lastName)
                .ToArray();

            return _mapper.Map<TimeLog[]>(timeLogs);
        }
    }
}
