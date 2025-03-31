using AutoMapper;
using Company.Menna.DAL.Models;
using Company.Menna.PL.Dtos;

namespace Company.Menna.PL.Mapping
{
    // CLR : Common Language Runtime
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<CreateEmployeeDto,Employee>();
            CreateMap<Employee,CreateEmployeeDto>();
        }
    }
}
