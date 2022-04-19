using AutoMapper;
using Meetup_API.Dtos.User;
using Meetup_API.Entities;
using Meetup_API.Helpers;
using Meetup_API.Interfaces;
using Meetup_API.Interfaces.Data;
using Microsoft.AspNetCore.Mvc;

namespace Meetup_API.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AuthController(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(UserRegistrationDto userRegistrationDto)
        {
            if (await _unitOfWork.UserRepository.UserExistsAsync(userRegistrationDto.Username))
                return BadRequest("Введённый Username уже используется");

            _unitOfWork.UserRepository.AddUser(userRegistrationDto);

            if (!(await _unitOfWork.CompleteAsync()))
                return BadRequest("Ошибка добавления пользователя");

            var user = await _unitOfWork.UserRepository.GetUserByUserNameAsync(userRegistrationDto.Username);

            return new UserDto
            {
                Username = user.Username,
                Gender = user.Gender,
                Company = user.Company,
                DateOfBirth = user.DateOfBirth,
                Token = await _tokenService.CreateTokenAsync(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(UserLoginDto userLoginDto)
        {
            if ( !( await _unitOfWork.UserRepository.UserExistsAsync(userLoginDto.Username)) ||
                !(await _unitOfWork.UserRepository.VerifyPasswordAsync(userLoginDto)))
                return BadRequest("Не верные данные Username или Password");

            var user = await _unitOfWork.UserRepository.GetUserByUserNameAsync(userLoginDto.Username);

            return new UserDto
            {
                Username = user.Username,
                Gender = user.Gender,
                Company = user.Company,
                DateOfBirth = user.DateOfBirth,
                Token = await _tokenService.CreateTokenAsync(user)
            };
        }
    }
}
