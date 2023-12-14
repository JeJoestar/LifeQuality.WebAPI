using AutoMapper;
using LifeQuality.Core.DTOs;
using LifeQuality.Core.DTOs.Analysis;
using LifeQuality.Core.DTOs.Notifications;
using LifeQuality.Core.DTOs.Recommendations;
using LifeQuality.Core.DTOs.Sensors;
using LifeQuality.Core.DTOs.Users;
using LifeQuality.DataContext.Enums;
using LifeQuality.DataContext.Model;
using LifeQuality.WebAPI.DTOs.Analysis;

namespace LifeQuality.WebAPI.Mappers
{

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientInfoDto>()
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfileImageUrl))
                .ForMember(dest => dest.PatientStatusDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Analysis, opt => opt.MapFrom(src => src.BloodAnalysisDatas))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.Name)).ReverseMap();

            CreateMap<Patient, PatientDto>().ReverseMap();

            CreateMap<Patient, PatientProfileDto>()
                .ForMember(dest => dest.Recommendations, opt => opt.MapFrom(src => src.Recomendations))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfileImageUrl))
                .ForMember(dest => dest.PatientStatusDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Analysis, opt => opt.MapFrom(src => src.BloodAnalysisDatas))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.Name))
                .ReverseMap();

            CreateMap<Doctor, DoctorProfileDto>()
                .ForMember(dest => dest.DoctorSpeciality, opt => opt.MapFrom(src => src.Speciality))
                .ReverseMap();

            CreateMap<Sensor, SensorStatusDTO>().ReverseMap();
            CreateMap<Patient, FastEntityDto>().ReverseMap();
            CreateMap<Notification, NotificationDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.RawText)).ReverseMap();

            CreateMap<BloodAnalysisData, SmallAnalysisDto>()
                .ForMember(dest => dest.ReceivedAt, opt => opt.MapFrom(src => src.ReceivedAt))
                .ForMember(dest => dest.IsRegular, opt => opt.MapFrom(src => src.IsRegular))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.AnalysisType, opt => opt.MapFrom(src => src.Sensor.Type))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.Name))
                .ReverseMap();
            CreateMap<Recomendation, ShortRecommendationDto>()
                .ForMember(dest => dest.ReceivedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.Analysis, opt => opt.MapFrom(src => src.Analysis))
                .ReverseMap();
            CreateMap<BloodAnalysisData, AnalysisInfoDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Sensor.Type))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.ReceivedAt))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.Name))
                .ForMember(dest => dest.Files, opt => opt.MapFrom(src => new string[1]))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Data))
                .ReverseMap();
        }
    }
}
