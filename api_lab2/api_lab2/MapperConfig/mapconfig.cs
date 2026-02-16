
using api_lab2.Controllers;
using api_lab2.DTOs;
using api_lab2.Models;
using AutoMapper;
namespace api_lab2.MapperConfig
{
    public class mapconfig : Profile
    {

        public mapconfig()
        {
            CreateMap<Student, StudentDTO>()
                // Map dept_id from navigation property safely
                .ForMember(dest => dest.dept_id,
                           opt => opt.MapFrom(src => src.department != null ? src.department.Id : (int?)null))

                .ReverseMap(); // ReverseMap works for dept_id as nullable int


            // Optionally, map Department ↔ DepartmentDTO if you have it
            CreateMap<Department, DepartmentDTO>()

                .ReverseMap();
        }
    }




}