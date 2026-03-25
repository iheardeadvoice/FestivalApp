using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using FestivalAppAPI.DTOs.Auth;
using FestivalAppAPI.DTOs.User;
using FestivalAppAPI.Entities;
using FestivalAppAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FestivalAppAPI.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileStorageService _fileStorage;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepository, IFileStorageService fileStorage, IMapper mapper, IConfiguration config)
        {
            _userRepository = userRepository;
            _fileStorage = fileStorage;
            _mapper = mapper;
            _config = config;
        }

        public async Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto)
        {
            if (await _userRepository.EmailExistsAsync(registerDto.Email))
                throw new InvalidOperationException("Email already exists");

            string? photoPath = null;
            if (registerDto.Photo != null && registerDto.Photo.Length > 0)
            {
                photoPath = await _fileStorage.SaveFileAsync(registerDto.Photo.OpenReadStream(), registerDto.Photo.FileName, "photos");
            }

            var user = _mapper.Map<User>(registerDto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            user.PhotoPath = photoPath;
            user.Role = "user";

            await _userRepository.AddAsync(user);

            var userDto = _mapper.Map<UserDto>(user);
            userDto.PhotoUrl = !string.IsNullOrEmpty(user.PhotoPath) ? _fileStorage.GetFileUrl(user.PhotoPath) : null;

            var token = GenerateJwtToken(user);

            return new AuthResponseDto { Token = token, User = userDto };
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password");

            var userDto = _mapper.Map<UserDto>(user);
            userDto.PhotoUrl = !string.IsNullOrEmpty(user.PhotoPath) ? _fileStorage.GetFileUrl(user.PhotoPath) : null;

            var token = GenerateJwtToken(user);

            return new AuthResponseDto { Token = token, User = userDto };
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var jwtKey = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
                throw new InvalidOperationException("JWT Key is not configured");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}