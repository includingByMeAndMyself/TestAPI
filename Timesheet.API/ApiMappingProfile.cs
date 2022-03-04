using AutoMapper;
using Timesheet.API.Models;
using Timesheet.Domain.Models;

namespace Timesheet.API
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<CreateTimeLogRequest, TimeLog>();
            CreateMap<EmployeeReport, GetEmployeeReportResponse>();
            CreateMap<TimeLog, TimeLogDto>();
            CreateMap<Issue, IssueDto>();
        }
    }
}
