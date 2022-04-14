namespace Meetup_API.Interfaces.Data;

public interface IUnitOfWork
{
    IMeetupRepository MeetupRepository { get; }
    Task<bool> CompleteAsync();
}
