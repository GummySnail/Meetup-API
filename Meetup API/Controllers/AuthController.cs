using Meetup_API.Dtos.User;
using Meetup_API.Entities;
using Meetup_API.Helpers;
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
        private readonly IOptions<AuthOptions> _authOptions;

        public AuthController(IUnitOfWork unitOfWork, IOptions<AuthOptions> authOptions)
        {
            _unitOfWork = unitOfWork;
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

            var user = await _unitOfWork.UserRepository.GetUserByUserNameAsync(userRegistrationDto.UserName);

            return new UserDto
            {
                UserName = user.UserName,
                Gender = user.Gender,
                Company = user.Company,
                DateOfBirth = user.DateOfBirth,
                AccessToken = GenerateJWT(user)
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

            var user = await _unitOfWork.UserRepository.GetUserByUserNameAsync(userLoginDto.UserName);

            return new UserDto
            {
                UserName = user.UserName,
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
                };

            claims.Add(new Claim("role", user.Role.ToString()));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(authParams.TokenLifeTime),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}
