using Meetup_API.Entities;
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

    public async Task<List<Meetup>> GetMeetupsAsync()
    {
        return await _dataContext.Meetups.AsNoTracking().ToListAsync();
    }

    public async Task<Meetup> UpdateMeetupAsync(Meetup request)
    {
        var meetup = await _dataContext.Meetups.AsNoTracking().SingleOrDefaultAsync(m => m.Id == request.Id);

        if (meetup == null)
            return meetup;

        meetup.Street = request.Street;
        meetup.City = request.City;
       
        meetup.StartMeetupDateTime = request.StartMeetupDateTime;
        meetup.EndMeetupDateTime = request.EndMeetupDateTime;
        
        meetup.Description = request.Description;
        meetup.HomeNumber = request.HomeNumber;
        meetup.Name = request.Name;

        _dataContext.Meetups.Update(meetup);

        return meetup;
    }
}
