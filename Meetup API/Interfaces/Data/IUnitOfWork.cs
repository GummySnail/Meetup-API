namespace Meetup_API.Interfaces.Data;

public interface IUnitOfWork
{
    IMeetupRepository MeetupRepository { get; }
    ITagRepository TagRepository { get; }
    IUserRepository UserRepository { get; }
    Task<bool> CompleteAsync();
}
