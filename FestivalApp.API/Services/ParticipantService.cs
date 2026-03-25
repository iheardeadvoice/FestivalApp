using AutoMapper;
using FestivalAppAPI.DTOs.Participant;
using FestivalAppAPI.Interfaces;

namespace FestivalAppAPI.Services
{
    public class ParticipantService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileStorageService _fileStorage;
        private readonly IMapper _mapper;

        public ParticipantService(IUserRepository userRepository, IFileStorageService fileStorage, IMapper mapper)
        {
            _userRepository = userRepository;
            _fileStorage = fileStorage;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ParticipantDto>> GetParticipantsAsync(string? name, int? competencyId, string? category, int? regionId)
        {
            var users = await _userRepository.GetParticipantsAsync(name, competencyId, category, regionId);
            var dtos = _mapper.Map<IEnumerable<ParticipantDto>>(users);
            foreach (var dto in dtos)
            {
                var user = users.First(u => $"{u.LastName} {u.FirstName} {u.Patronymic}".Trim() == dto.FullName);
                dto.PhotoUrl = !string.IsNullOrEmpty(user.PhotoPath) ? _fileStorage.GetFileUrl(user.PhotoPath) : null;
            }
            return dtos;
        }
    }
}