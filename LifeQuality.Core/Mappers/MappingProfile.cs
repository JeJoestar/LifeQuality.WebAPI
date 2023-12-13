using AutoMapper;
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
                .ForMember(dest => dest.Recommendations, opt => opt.MapFrom(src => src.Reports.Select(r => r.ReportContext)))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfileImageUrl))
                .ForMember(dest => dest.PatientStatusDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Analysis, opt => opt.MapFrom(src => src.BloodAnalysisDatas))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.Name))
                .ReverseMap();

            CreateMap<Doctor, DoctorProfileDto>().ReverseMap();

            CreateMap<Sensor, SensorStatusDTO>().ReverseMap();

            CreateMap<BloodAnalysisData, SmallAnalysisDto>()
                .ForMember(dest => dest.ReceivedAt, opt => opt.MapFrom(src => src.ReceivedAt))
                .ForMember(dest => dest.IsRegular, opt => opt.MapFrom(src => src.Sensor.ReadingType == ReadingType.Scheduled))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.AnalysisType, opt => opt.MapFrom(src => src.Sensor.Type))
                .ReverseMap();
            CreateMap<Report, ShortRecommendationDto>()
                .ForMember(dest => dest.ReceivedAt, opt => opt.MapFrom(src => src.Analisis.ReceivedAt))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.ReportContext))
                .ForMember(dest => dest.Analysis, opt => opt.MapFrom(src => src.Analisis))
                .ReverseMap();
        }
    }
}
