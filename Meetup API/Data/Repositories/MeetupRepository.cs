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

    public void AddMeetup(Meetup meetup)
    {
        _dataContext.Meetups.Add(meetup);
    }

    public async Task<Meetup> DeleteMeetupAsync(int id)
    {
        var meetup = await _dataContext.Meetups.SingleOrDefaultAsync(m => m.Id == id);

        if (meetup == null)
            return meetup;
        
        _dataContext.Remove(meetup);
        return meetup;
    }

    public async Task<Meetup> GetMeetupAsync(int id)
    {
        return await _dataContext.Meetups.AsNoTracking().SingleOrDefaultAsync(m => m.Id == id);
    }

    public async Task<PagedList<Meetup>> GetMeetupsAsync(int currentPage, int pageSize)
    {
        var source = _dataContext.Meetups
            .AsQueryable()
            .OrderByDescending(m => m.StartMeetupDateTime)
            .AsNoTracking();

        return await PagedList<Meetup>
            .CreateAsync(source, currentPage, pageSize);
    }

    public async Task<Meetup> UpdateMeetupAsync(Meetup request)
    {
        var meetup = await _dataContext.Meetups.AsNoTracking().SingleOrDefaultAsync(m => m.Id == request.Id);

        if (meetup == null)
            return meetup;

        meetup.Street = request.Street;
        meetup.City = request.City;
        meetup.Tags = request.Tags;
        meetup.StartMeetupDateTime = request.StartMeetupDateTime;
        meetup.EndMeetupDateTime = request.EndMeetupDateTime;
        meetup.Description = request.Description;
        meetup.HomeNumber = request.HomeNumber;
        meetup.Name = request.Name;
        meetup.OwnerId = request.OwnerId;

        _dataContext.Meetups.Update(meetup);

        return meetup;
    }

}
