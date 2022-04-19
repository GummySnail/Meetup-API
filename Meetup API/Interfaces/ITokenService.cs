using Meetup_API.Entities;

namespace Meetup_API.Interfaces;

public interface ITokenService
{
    Task<string> CreateTokenAsync(User user);
}
