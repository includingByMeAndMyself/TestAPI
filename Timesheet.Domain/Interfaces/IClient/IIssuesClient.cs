﻿using System.Threading.Tasks;
using Timesheet.Domain.Models;

namespace Timesheet.Domain.Interfaces.IClient
{
    public interface IIssuesClient
    {
        Task<Issue[]> Get(string login);
    }
}
