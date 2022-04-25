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
        var meetup = await _dataContext.Meetups.SingleOrDefaultAsync(m => m.Id == id);

        if (meetup == null)
        {
            return meetup;
        }

        _dataContext.Remove(meetup);
        return meetup;
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
        var query = _dataContext.Meetups
            .Where(m => m.City.ToLower().Contains(meetupParams.City.ToLower()))
            .Where(m => m.Name.ToLower().Contains(meetupParams.Name.ToLower()));

        query = meetupParams.OrderByDateTime switch
        {
            "Upcoming" => query.OrderBy(query => query.StartMeetupDateTime).Include(t => t.Tags),
            _ => query.OrderByDescending(query => query.EndMeetupDateTime).Include(t => t.Tags)
        };

        return await PagedList<Meetup>
            .CreateAsync(query, meetupParams.PageNumber, meetupParams.PageSize);
    }

    public async Task<Meetup> SignUpForMeetupAsync(SignUpForMeetupDto request)
    {
        var user = await _dataContext.Users
            .Where(u => u.Id == request.UserId)
            .Include(u => u.Meetups)
            .FirstOrDefaultAsync();

        if(user == null)
        {
            return null;
        }

        var meetup = await _dataContext.Meetups.FindAsync(request.MeetupId);

        if(meetup == null)
        {
            return null;
        }

        user.Meetups.Add(meetup);

        return meetup;
    }

    public async Task<Meetup> UpdateMeetupAsync(Meetup request)
    {
        var meetup = await _dataContext.Meetups.AsNoTracking().SingleOrDefaultAsync(m => m.Id == request.Id);

        if (meetup == null)
        {
            return meetup;
        }

        meetup.Street = request.Street;
        meetup.City = request.City;
        meetup.Tags = request.Tags;
        meetup.StartMeetupDateTime = request.StartMeetupDateTime;
        meetup.EndMeetupDateTime = request.EndMeetupDateTime;
        meetup.Description = request.Description;
        meetup.HomeNumber = request.HomeNumber;
        meetup.Name = request.Name;

        _dataContext.Meetups.Update(meetup);

        return meetup;
    }

}
