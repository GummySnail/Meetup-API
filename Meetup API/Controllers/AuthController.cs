using AutoMapper;
using Meetup_API.Dtos.User;
using Meetup_API.Entities;
using Meetup_API.Helpers;
using Meetup_API.Interfaces;
using Meetup_API.Interfaces.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Meetup_API.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOptions<AuthOptions> _authOptions;

        public AuthController(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AuthOptions> authOptions)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authOptions = authOptions;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(UserRegistrationDto userRegistrationDto)
        {
            if (await _unitOfWork.UserRepository.UserExistsAsync(userRegistrationDto.Username))
            { 
                return BadRequest("Введённый Username уже используется"); 
            }

            _unitOfWork.UserRepository.AddUser(userRegistrationDto);

            if (!(await _unitOfWork.CompleteAsync()))
            {
                return BadRequest("Ошибка добавления пользователя");
            }

            var user = await _unitOfWork.UserRepository.GetUserByUserNameAsync(userRegistrationDto.Username);

            return new UserDto
            {
                Username = user.Username,
                Gender = user.Gender,
                Company = user.Company,
                DateOfBirth = user.DateOfBirth,
                AccessToken = GenerateJWT(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(UserLoginDto userLoginDto)
        {
            if ( !( await _unitOfWork.UserRepository.UserExistsAsync(userLoginDto.Username)) ||
                !(await _unitOfWork.UserRepository.VerifyPasswordAsync(userLoginDto)))
            {
                return BadRequest("Не верные данные Username или Password");
            }

            var user = await _unitOfWork.UserRepository.GetUserByUserNameAsync(userLoginDto.Username);

            return new UserDto
            {
                Username = user.Username,
                Gender = user.Gender,
                Company = user.Company,
                DateOfBirth = user.DateOfBirth,
                AccessToken = GenerateJWT(user)
            };
        }

        private string GenerateJWT(User user)
        {
            var authParams = _authOptions.Value;

            var securityKey = authParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
            };

            claims.Add(new Claim("role", user.Role.ToString()));

            var token = new JwtSecurityToken(authParams.Issuer,
                authParams.Audience,
                claims,
                expires: DateTime.UtcNow.AddSeconds(authParams.TokenLifeTime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
    }
}
