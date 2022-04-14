using Meetup_API.Entities;

namespace Meetup_API.Interfaces.Data;

public interface IMeetupRepository
{
    void AddMeetup(Meetup meetup);
    Task<List<Meetup>> GetMeetups();
    Task<Meetup> GetMeetupById(int id);
    Task<Meetup> UpdateMeetup(Meetup request);
    Task<Meetup> DeleteMeetup(int id);
}
