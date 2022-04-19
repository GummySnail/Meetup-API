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
        var user = _mapper.Map<UserRegistrationDto, User>(userRegistrationDto);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        _dataContext.Users.Add(user);
    }

    public async Task<User> GetUserByUserNameAsync(string username)
    {
        return await _dataContext.Users.SingleOrDefaultAsync(u => u.Username == username);
    }

    public async Task<bool> UserExistsAsync(string username) => await _dataContext.Users
        .AnyAsync(x => x.Username.ToLower() == username.ToLower());

    public async Task<bool> VerifyPasswordAsync(UserLoginDto userLoginDto)
    {
        var user = await _dataContext.Users.SingleOrDefaultAsync(u => u.Username == userLoginDto.Username);

        if (user == null)
            return false;

        return PasswordHasher.VerifyPasswordHash(userLoginDto.Password, user.PasswordHash, user.PasswordSalt);
    }
}
