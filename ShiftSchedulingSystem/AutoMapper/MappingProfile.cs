using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace ShiftSchedulingSystem.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Shift, ShiftDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ShiftType, opt => opt.MapFrom(src => src.ShiftType))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.Day, opt => opt.MapFrom(src => src.Day))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime));

        CreateMap<Worker, WorkerDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
            .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department));

        CreateMap<ShiftForCreationDto, Shift>()
            .ForMember(dest => dest.ShiftType, opt => opt.MapFrom(src => src.ShiftType))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.Day, opt => opt.MapFrom(src => src.Day))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime));

        CreateMap<WorkerForCreationDto, Worker>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
            .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department)).ReverseMap();

        CreateMap<WorkerForUpdateDto, Worker>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
            .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department)).ReverseMap();

        CreateMap<ShiftForUpdateDto, Shift>()
            .ForMember(dest => dest.ShiftType, opt => opt.MapFrom(src => src.ShiftType))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.Day, opt => opt.MapFrom(src => src.Day))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime)).ReverseMap();

        CreateMap<UserForRegistrationDto, User>();
    }
}
