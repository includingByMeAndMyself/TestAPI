using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timesheet.DAL.CSV.Infrastructure
{
    public class CsvSettings
    {
        public CsvSettings(string delimeter, string path)
        {
            Delimeter = delimeter;
            Path = path;
        }
        public string Delimeter { get; }
        public string Path { get;  }
    }
}
