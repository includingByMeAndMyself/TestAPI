﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timesheet.Domain.Interfaces.IService
{
    public interface IAuthService
    {
        bool? Login(string lastName);
    }
}