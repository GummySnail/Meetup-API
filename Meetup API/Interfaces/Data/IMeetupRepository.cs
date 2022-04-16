using Meetup_API.Dtos.Meetup;
using Meetup_API.Entities;

namespace Meetup_API.Interfaces.Data;

public interface IMeetupRepository
{
    void AddMeetup(Meetup meetup);
    Task<List<Meetup>> GetMeetupsAsync();
    Task<Meetup> GetMeetupAsync(int id);
    Task<Meetup> UpdateMeetupAsync(Meetup request);
    Task<Meetup> DeleteMeetupAsync(int id);
}
