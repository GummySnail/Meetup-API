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
            if (await _unitOfWork.UserRepository.UserExistsAsync(userRegistrationDto.UserName))
            { 
                return BadRequest("Введённый Username уже используется"); 
            }

            _unitOfWork.UserRepository.AddUser(userRegistrationDto);

            if (!(await _unitOfWork.CompleteAsync()))
            {
                return BadRequest("Ошибка добавления пользователя");
            }

            var User = await _unitOfWork.UserRepository.GetUserByUserNameAsync(userRegistrationDto.UserName);

            return new UserDto
            {
                UserName = User.UserName,
                Gender = User.Gender,
                Company = User.Company,
                DateOfBirth = User.DateOfBirth,
                AccessToken = GenerateJWT(User)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(UserLoginDto userLoginDto)
        {
            if ( !( await _unitOfWork.UserRepository.UserExistsAsync(userLoginDto.UserName)) ||
                !(await _unitOfWork.UserRepository.VerifyPasswordAsync(userLoginDto)))
            {
                return BadRequest("Не верные данные Username или Password");
            }

            var User = await _unitOfWork.UserRepository.GetUserByUserNameAsync(userLoginDto.UserName);

            return new UserDto
            {
                UserName = User.UserName,
                Gender = User.Gender,
                Company = User.Company,
                DateOfBirth = User.DateOfBirth,
                AccessToken = GenerateJWT(User)
            };
        }

        private string GenerateJWT(User user)
        {
            var AuthParams = _authOptions.Value;

            var SecurityKey = AuthParams.GetSymmetricSecurityKey();
            var Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            var Claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            Claims.Add(new Claim("role", user.Role.ToString()));

            var token = new JwtSecurityToken(AuthParams.Issuer,
                AuthParams.Audience,
                Claims,
                expires: DateTime.UtcNow.AddSeconds(AuthParams.TokenLifeTime),
                signingCredentials: Credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
    }
}
