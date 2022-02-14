﻿using System.Collections.Generic;
using Timesheet.Domain.Interfaces;
using Timesheet.Domain.Models;

namespace Timesheet.Application.Services
{
    public class TimesheetService : ITimesheetService
    {
        public bool TrackTime(TimeLog timeLog)
        {
            bool isValid = timeLog.WorkingHours > 0 
                           && timeLog.WorkingHours <= 24
                           && !string.IsNullOrWhiteSpace(timeLog.LastName);

            isValid = UserSession.Sessions.Contains(timeLog.LastName);

            if (isValid == false)
            {
                return false;
            }

            Timesheets.TimeLogs.Add(timeLog);
            return true;
        } 
    }

    public static class Timesheets
    {
        public static List<TimeLog> TimeLogs { get; set; } = new List<TimeLog>();
    }
}