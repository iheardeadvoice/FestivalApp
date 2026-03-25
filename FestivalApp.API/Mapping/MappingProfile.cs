using AutoMapper;
using FestivalAppAPI.DTOs.Application;
using FestivalAppAPI.DTOs.Auth;
using FestivalAppAPI.DTOs.Competency;
using FestivalAppAPI.DTOs.Participant;
using FestivalAppAPI.DTOs.User;
using FestivalAppAPI.Entities;

namespace FestivalAppAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.RegionName, opt => opt.MapFrom(src => src.Region != null ? src.Region.Name : null))
                .ForMember(dest => dest.PhotoUrl, opt => opt.Ignore());

            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PhotoPath, opt => opt.Ignore());

            // Competency
            CreateMap<Competency, CompetencyDto>()
                .ForMember(dest => dest.AssignmentFileUrl, opt => opt.Ignore());

            // Application
            CreateMap<Application, ApplicationDto>()
                .ForMember(dest => dest.CompetencyTitle, opt => opt.MapFrom(src => src.Competency != null ? src.Competency.Title : null));

            // Participant
            CreateMap<User, ParticipantDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.LastName} {src.FirstName} {src.Patronymic}".Trim()))
                .ForMember(dest => dest.Competency, opt => opt.MapFrom(src => GetApprovedCompetencyTitle(src)))
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Region != null ? src.Region.Name : string.Empty))
                .ForMember(dest => dest.PhotoUrl, opt => opt.Ignore());
        }

        private static string GetApprovedCompetencyTitle(User user)
        {
            var approvedApp = user.Applications.FirstOrDefault(a => a.Status == "approved");
            if (approvedApp == null) return string.Empty;
            return approvedApp.Competency?.Title ?? string.Empty;
        }
    }
}