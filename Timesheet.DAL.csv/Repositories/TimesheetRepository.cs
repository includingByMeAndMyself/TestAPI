using System;
using System.Collections.Generic;
using System.IO;
using Timesheet.DAL.CSV.Infrastructure;
using Timesheet.Domain.Interfaces.IRepository;
using Timesheet.Domain.Models;

namespace Timesheet.DAL.CSV.Repositories
{
    public class TimesheetRepository : ITimesheetRepository
    {
        private readonly string _delimeter;
        private readonly string _path;

        public TimesheetRepository(CsvSettings csvSettings)
        {
            _delimeter = csvSettings.Delimeter;
            _path = csvSettings.Path + "\\timesheet.csv";
        }

        public TimeLog[] GetTimeLogs(string lastName)
        {
            var data = File.ReadAllText(_path);
            var timeLogs = new List<TimeLog>();

            foreach (var dataRow in data.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                var timeLog = new TimeLog();
                var dataMembers = dataRow.Split(_delimeter);

                timeLog.Comment = dataMembers[0];
                timeLog.Date =  DateTime.TryParse(dataMembers[1], out var date) ? date : new DateTime();
                timeLog.LastName = dataMembers[2];
                timeLog.WorkingHours = int.TryParse(dataMembers[3], out var workingHours) ? workingHours : 0;
            }

            return timeLogs.ToArray();
        }

        public void Add(TimeLog timeLog)
        {
            var dataRow = $"{timeLog.Comment}{_delimeter}" +
                          $"{timeLog.Date}{_delimeter}" +
                          $"{timeLog.LastName}{_delimeter}" +
                          $"{timeLog.WorkingHours}\n";

            File.AppendAllText(_path, dataRow);
        }

    }
}
