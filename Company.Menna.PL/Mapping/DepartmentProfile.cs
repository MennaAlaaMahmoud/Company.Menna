using AutoMapper;
using Company.Menna.DAL.Models;
using Company.Menna.PL.Dtos;

namespace Company.Menna.PL.Mapping
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<CreateDepartmentDto,Department>();
            CreateMap< Department,CreateDepartmentDto>();
        }
    }
}
