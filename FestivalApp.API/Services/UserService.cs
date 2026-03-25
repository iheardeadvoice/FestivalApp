using AutoMapper;
using FestivalAppAPI.DTOs.User;
using FestivalAppAPI.Interfaces;
using FestivalAppAPI.Services;

namespace FestivalAppAPI.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileStorageService _fileStorage;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IFileStorageService fileStorage, IMapper mapper)
        {
            _userRepository = userRepository;
            _fileStorage = fileStorage;
            _mapper = mapper;
        }

        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;
            var dto = _mapper.Map<UserDto>(user);
            dto.PhotoUrl = !string.IsNullOrEmpty(user.PhotoPath) ? _fileStorage.GetFileUrl(user.PhotoPath) : null;
            return dto;
        }
    }
}