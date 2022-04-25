using AutoMapper;
using Meetup_API.Dtos.User;
using Meetup_API.Entities;
using Meetup_API.Helpers;
using Meetup_API.Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace Meetup_API.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public UserRepository(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public void AddUser(UserRegistrationDto userRegistrationDto)
    {
        PasswordHasher.CreatePasswordHash(userRegistrationDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var User = _mapper.Map<UserRegistrationDto, User>(userRegistrationDto);

        User.PasswordHash = passwordHash;
        User.PasswordSalt = passwordSalt;

        _dataContext.Users.Add(User);
    }

    public async Task<User> GetUserByUserNameAsync(string userName)
    {
        return await _dataContext.Users.SingleOrDefaultAsync(u => u.UserName == userName);
    }

    public async Task<bool> UserExistsAsync(string userName) => await _dataContext.Users
        .AnyAsync(x => x.UserName.ToLower() == userName.ToLower());

    public async Task<bool> VerifyPasswordAsync(UserLoginDto userLoginDto)
    {
        var User = await _dataContext.Users.SingleOrDefaultAsync(u => u.UserName == userLoginDto.UserName);

        if (User == null)
        {
            return false;
        }

        return PasswordHasher.VerifyPasswordHash(userLoginDto.Password, User.PasswordHash, User.PasswordSalt);
    }
}
