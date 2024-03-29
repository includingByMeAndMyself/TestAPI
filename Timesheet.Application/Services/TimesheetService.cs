﻿using System;
using Timesheet.Domain.Interfaces.IRepository;
using Timesheet.Domain.Interfaces.IService;
using Timesheet.Domain.Models;

namespace Timesheet.BussinessLogic.Services
{
    public class TimesheetService : ITimesheetService
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public TimesheetService(ITimesheetRepository timesheetRepository, IEmployeeRepository employeeRepository)
        {
            _timesheetRepository = timesheetRepository;
            _employeeRepository = employeeRepository;
        }

        public bool TrackTime(TimeLog timeLog, string lastName)
        {
            var employee = _employeeRepository.Get(lastName);

            bool isValid = employee != null ? employee.CheckInputLog(timeLog) : false;

            if (!isValid)
            {
                return false;
            }

            _timesheetRepository.Add(timeLog);

            return true;
        }
    }
}
