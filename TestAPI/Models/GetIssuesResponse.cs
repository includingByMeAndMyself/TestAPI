using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timesheet.API.Models
{
    public class GetIssuesResponse
    {
        public IssueDto[] Issues { get; set; }
    }
}
