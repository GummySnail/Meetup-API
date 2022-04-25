using Meetup_API.Dtos.User;
using Meetup_API.Entities;

namespace Meetup_API.Interfaces.Data;

public interface IUserRepository
{
    void AddUser(UserRegistrationDto user);
    Task<bool> UserExistsAsync(string userName);
    Task<bool> VerifyPasswordAsync(UserLoginDto userLoginDto);
    Task<User> GetUserByUserNameAsync(string userName);
}
