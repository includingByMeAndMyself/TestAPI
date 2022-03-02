using AutoMapper;
using Timesheet.Domain.Models;

namespace Timesheet.DAL.MSSQL
{
    public class DataAccessMappingProfile : Profile
    {
        public DataAccessMappingProfile()
        {
            CreateMap<Employee, Entities.Employee>()
                .ReverseMap();

            CreateMap<SuperiorEmployee, Entities.Employee>()
                .IncludeBase<Employee, Entities.Employee>()
                .ReverseMap();

            CreateMap<StaffEmployee, Entities.Employee>()
                .IncludeBase<Employee, Entities.Employee>()
                .ReverseMap();

            CreateMap<FreelancerEmployee, Entities.Employee>()
                .IncludeBase<Employee, Entities.Employee>()
                .ReverseMap();

            CreateMap<TimeLog, Entities.TimeLog>()
                .ReverseMap();
        }
    }
}
