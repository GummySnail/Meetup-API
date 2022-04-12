using Meetup_API.Data;
using Meetup_API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meetup_API.Controllers;

public class MeetupController : BaseApiController
{
    private readonly DataContext _context;
    public MeetupController(DataContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult> AddMeetup(Meetup meetup)
    {
        _context.Meetups.Add(meetup);
        await _context.SaveChangesAsync();
        return Ok(await _context.Meetups.ToListAsync());
    }

    [HttpGet]
    public async Task<ActionResult<List<Meetup>>> GetMeetups()
    {
        return Ok(await _context.Meetups.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Meetup>> GetMeetupById(int id)
    {
        var meetup = await _context.Meetups.FindAsync(id);
        if (meetup == null)
            return BadRequest("Meetup not found");
        return Ok(meetup);
    }

    [HttpPut]
    public async Task<ActionResult<List<Meetup>>> UpdateMeetup(Meetup request)
    {
        var meetup = await _context.Meetups.FindAsync(request.Id);
        if (meetup == null)
            return BadRequest("Meetup not found");

        meetup.Street = request.Street;
        meetup.City = request.City;
        meetup.Tag = request.Tag;
        meetup.StartMeetupDateTime = request.StartMeetupDateTime;
        meetup.EndMeetupDateTime = request.EndMeetupDateTime;
        meetup.OwnerName = request.OwnerName;
        meetup.Description = request.Description;
        meetup.HomeNumber = request.HomeNumber;
        meetup.Name = request.Name;

        await _context.SaveChangesAsync();

        return Ok(await _context.Meetups.ToListAsync());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMeetup(int id)
    {
        var meetup = await _context.Meetups.FindAsync(id);
        if (meetup == null)
            return BadRequest("Meetup not found");

        _context.Meetups.Remove(meetup);
        await _context.SaveChangesAsync();

        return Ok(await _context.Meetups.ToListAsync());
    }
}
