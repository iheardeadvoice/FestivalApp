using AutoMapper;
using FestivalAppAPI.DTOs.Application;
using FestivalAppAPI.Entities;
using FestivalAppAPI.Enums;
using FestivalAppAPI.Interfaces;

namespace FestivalAppAPI.Services
{
    public class ApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly ICompetencyRepository _competencyRepository;
        private readonly IMapper _mapper;

        public ApplicationService(IApplicationRepository applicationRepository, ICompetencyRepository competencyRepository, IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _competencyRepository = competencyRepository;
            _mapper = mapper;
        }

        public async Task<ApplicationDto> CreateAsync(Guid userId, CreateApplicationDto dto)
        {
            if (!await _competencyRepository.ExistsAsync(dto.CompetencyId))
                throw new InvalidOperationException("Competency not found");

            var existing = await _applicationRepository.GetByUserAndCompetencyAsync(userId, dto.CompetencyId);
            if (existing != null && (existing.Status == ApplicationStatuses.Pending || existing.Status == ApplicationStatuses.Approved))
                throw new InvalidOperationException("You already have an active application for this competency");

            var application = new Application
            {
                UserId = userId,
                CompetencyId = dto.CompetencyId,
                Comment = dto.Comment,
                Status = ApplicationStatuses.Pending
            };

            await _applicationRepository.AddAsync(application);
            return _mapper.Map<ApplicationDto>(application);
        }

        public async Task<IEnumerable<ApplicationDto>> GetUserApplicationsAsync(Guid userId)
        {
            var apps = await _applicationRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<ApplicationDto>>(apps);
        }

        public async Task<IEnumerable<ApplicationDto>> GetAllApplicationsAsync()
        {
            var apps = await _applicationRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ApplicationDto>>(apps);
        }

        public async Task<ApplicationDto?> UpdateStatusAsync(int id, UpdateApplicationStatusDto dto)
        {
            var app = await _applicationRepository.GetByIdAsync(id);
            if (app == null) return null;

            if (dto.Status != ApplicationStatuses.Approved && dto.Status != ApplicationStatuses.Rejected)
                throw new InvalidOperationException("Invalid status");

            app.Status = dto.Status;
            await _applicationRepository.UpdateAsync(app);
            return _mapper.Map<ApplicationDto>(app);
        }
    }
}