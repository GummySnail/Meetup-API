using Meetup_API.Dtos.Meetup;
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

    public async Task<Meetup> DeleteMeetupAsync(int id)
    {
        var Meetup = await _dataContext.Meetups.SingleOrDefaultAsync(m => m.Id == id);

        if (Meetup == null)
        {
            return Meetup;
        }

        _dataContext.Remove(Meetup);
        return Meetup;
    }
    public async Task<bool> IsOwner(int ownerId)
    {
        return await _dataContext.Meetups.AnyAsync(m => m.OwnerId == ownerId);
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

    public async Task<Meetup> SignUpForMeetupAsync(SignUpForMeetupDto request)
    {
        var User = await _dataContext.Users
            .Where(u => u.Id == request.UserId)
            .Include(u => u.Meetups)
            .FirstOrDefaultAsync();

        if(User == null)
        {
            return null;
        }

        var Meetup = await _dataContext.Meetups.FindAsync(request.MeetupId);

        if(Meetup == null)
        {
            return null;
        }

        User.Meetups.Add(Meetup);

        return Meetup;
    }

    public async Task<Meetup> UpdateMeetupAsync(Meetup request)
    {
        var Meetup = await _dataContext.Meetups.AsNoTracking().SingleOrDefaultAsync(m => m.Id == request.Id);

        if (Meetup == null)
        {
            return Meetup;
        }

        Meetup.Street = request.Street;
        Meetup.City = request.City;
        Meetup.Tags = request.Tags;
        Meetup.StartMeetupDateTime = request.StartMeetupDateTime;
        Meetup.EndMeetupDateTime = request.EndMeetupDateTime;
        Meetup.Description = request.Description;
        Meetup.HomeNumber = request.HomeNumber;
        Meetup.Name = request.Name;

        _dataContext.Meetups.Update(Meetup);

        return Meetup;
    }

}
