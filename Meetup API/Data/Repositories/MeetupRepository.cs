using Meetup_API.Entities;
using Meetup_API.Helpers;
using Meetup_API.Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace Meetup_API.Data.Repositories;

public class MeetupRepository : IMeetupRepository
{
    private readonly DataContext _dataContext;
    public MeetupRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public void AddMeetup(Meetup meetup, int ownerId)
    {
        meetup.OwnerId = ownerId;
        _dataContext.Meetups.Add(meetup);
    }

    public async Task<Meetup> DeleteMeetupAsync(int id, int ownerId)
    {
        var meetup = await _dataContext.Meetups.SingleOrDefaultAsync(m => m.Id == id);

        if (meetup == null)
        {
            return meetup;
        }

        if (meetup.OwnerId != ownerId)
        {
            return null;
        }

        _dataContext.Remove(meetup);
        return meetup;
    }

    public async Task<Meetup> GetMeetupAsync(int id)
    {
        return await _dataContext.Meetups.AsNoTracking().Include(m => m.Tags).SingleOrDefaultAsync(m => m.Id == id);
    }

    public async Task<PagedList<Meetup>> GetMeetupsAsync(MeetupParams meetupParams)
    {
        var Query = _dataContext.Meetups
            .Where(m => m.City.ToLower().Contains(meetupParams.City.ToLower()))
            .Where(m => m.Name.ToLower().Contains(meetupParams.Name.ToLower()));

        Query = meetupParams.OrderByDateTime switch
        {
            "Upcoming" => Query.OrderBy(query => query.StartMeetupDateTime).Include(t => t.Tags),
            _ => Query.OrderByDescending(query => query.EndMeetupDateTime).Include(t => t.Tags)
        };

        return await PagedList<Meetup>
            .CreateAsync(Query, meetupParams.PageNumber, meetupParams.PageSize);
    }

    public async Task<UserMeetup> SigUpForMeetupAsync(UserMeetup userMeetup)
    {
        var meetupIsNull = await _dataContext.Meetups.SingleOrDefaultAsync(m => m.Id == userMeetup.MeetupId);
        var userIsNull = await _dataContext.Users.SingleOrDefaultAsync(u => u.Id == userMeetup.UserId);

        if (meetupIsNull == null || userIsNull == null)
        {
            return null;
        }
     
        _dataContext.UserMeetups.Add(userMeetup);

        return userMeetup;
    }

    public async Task<Meetup> UpdateMeetupAsync(Meetup request, int ownerId)
    {
        var meetup = await _dataContext.Meetups.AsNoTracking().SingleOrDefaultAsync(m => m.Id == request.Id);

        if (meetup == null)
        {
            return meetup;
        }

        if (ownerId != meetup.OwnerId)
        {
            return null;
        }

        meetup.Street = request.Street;
        meetup.City = request.City;
        meetup.Tags = request.Tags;
        meetup.StartMeetupDateTime = request.StartMeetupDateTime;
        meetup.EndMeetupDateTime = request.EndMeetupDateTime;
        meetup.Description = request.Description;
        meetup.HomeNumber = request.HomeNumber;
        meetup.Name = request.Name;
        meetup.OwnerId = ownerId;

        _dataContext.Meetups.Update(meetup);

        return meetup;
    }

}
