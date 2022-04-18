using Meetup_API.Entities;
using Meetup_API.Helpers;

namespace Meetup_API.Interfaces.Data;

public interface IMeetupRepository
{
    void AddMeetup(Meetup meetup);
    Task<PagedList<Meetup>> GetMeetupsAsync(MeetupParams meetupParams);
    Task<Meetup> GetMeetupAsync(int id);
    Task<Meetup> UpdateMeetupAsync(Meetup request);
    Task<Meetup> DeleteMeetupAsync(int id);
}
