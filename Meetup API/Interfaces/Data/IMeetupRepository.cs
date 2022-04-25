using Meetup_API.Dtos.Meetup;
using Meetup_API.Entities;
using Meetup_API.Helpers;

namespace Meetup_API.Interfaces.Data;

public interface IMeetupRepository
{
    void AddMeetup(Meetup meetup, int ownerId);
    Task<PagedList<Meetup>> GetMeetupsAsync(MeetupParams meetupParams);
    Task<Meetup> GetMeetupAsync(int id);
    Task<Meetup> UpdateMeetupAsync(Meetup request, int ownerId);
    Task<Meetup> DeleteMeetupAsync(int id, int ownerId);
    Task<Meetup> SignUpForMeetupAsync(SignUpForMeetupDto request);
}
