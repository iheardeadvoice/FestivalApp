using AutoMapper;
using FestivalAppAPI.DTOs.Competency;
using FestivalAppAPI.Entities;
using FestivalAppAPI.Interfaces;

namespace FestivalAppAPI.Services
{
    public class CompetencyService
    {
        private readonly ICompetencyRepository _competencyRepository;
        private readonly IFileStorageService _fileStorage;
        private readonly IMapper _mapper;

        public CompetencyService(ICompetencyRepository competencyRepository, IFileStorageService fileStorage, IMapper mapper)
        {
            _competencyRepository = competencyRepository;
            _fileStorage = fileStorage;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CompetencyDto>> GetAllAsync()
        {
            var competencies = await _competencyRepository.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<CompetencyDto>>(competencies);
            foreach (var dto in dtos)
            {
                var comp = competencies.First(c => c.Id == dto.Id);
                dto.AssignmentFileUrl = !string.IsNullOrEmpty(comp.AssignmentFilePath) ? _fileStorage.GetFileUrl(comp.AssignmentFilePath) : null;
            }
            return dtos;
        }

        public async Task<CompetencyDto?> GetByIdAsync(int id)
        {
            var competency = await _competencyRepository.GetByIdAsync(id);
            if (competency == null) return null;
            var dto = _mapper.Map<CompetencyDto>(competency);
            dto.AssignmentFileUrl = !string.IsNullOrEmpty(competency.AssignmentFilePath) ? _fileStorage.GetFileUrl(competency.AssignmentFilePath) : null;
            return dto;
        }

        public async Task<CompetencyDto> CreateAsync(CreateCompetencyDto dto)
        {
            var competency = new Competency
            {
                Title = dto.Title,
                Description = dto.Description
            };

            if (dto.AssignmentFile != null && dto.AssignmentFile.Length > 0)
            {
                var filePath = await _fileStorage.SaveFileAsync(dto.AssignmentFile.OpenReadStream(), dto.AssignmentFile.FileName, "assignments");
                competency.AssignmentFilePath = filePath;
            }

            await _competencyRepository.AddAsync(competency);
            return _mapper.Map<CompetencyDto>(competency);
        }

        public async Task<CompetencyDto?> UpdateAsync(int id, UpdateCompetencyDto dto)
        {
            var competency = await _competencyRepository.GetByIdAsync(id);
            if (competency == null) return null;

            competency.Title = dto.Title;
            competency.Description = dto.Description;

            if (dto.DeleteAssignment && !string.IsNullOrEmpty(competency.AssignmentFilePath))
            {
                _fileStorage.DeleteFile(competency.AssignmentFilePath);
                competency.AssignmentFilePath = null;
            }

            if (dto.AssignmentFile != null && dto.AssignmentFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(competency.AssignmentFilePath))
                    _fileStorage.DeleteFile(competency.AssignmentFilePath);

                var filePath = await _fileStorage.SaveFileAsync(dto.AssignmentFile.OpenReadStream(), dto.AssignmentFile.FileName, "assignments");
                competency.AssignmentFilePath = filePath;
            }

            await _competencyRepository.UpdateAsync(competency);
            return _mapper.Map<CompetencyDto>(competency);
        }
    }
}