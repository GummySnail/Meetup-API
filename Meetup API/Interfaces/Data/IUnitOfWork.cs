namespace Meetup_API.Interfaces.Data;

public interface IUnitOfWork
{
    IMeetupRepository MeetupRepository { get; }
    IUserRepository UserRepository { get; }
    Task<bool> CompleteAsync();
}
