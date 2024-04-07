using AutoMapper;
using EmployeeApi.Infrastructure.Entities;

namespace EmployeeApi.Core.EmployeesService.Models.MappingProfiles
{
    internal class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeCreateRequest, Employee>();
        }
    }
}
