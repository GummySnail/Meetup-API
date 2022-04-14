using Meetup_API.Entities;
using Meetup_API.Interfaces.Data;
using Microsoft.AspNetCore.Mvc;

namespace Meetup_API.Controllers;

public class MeetupController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    public MeetupController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost("add-meetup")]
    public async Task<ActionResult> AddMeetup(Meetup meetup)
    {
        _unitOfWork.MeetupRepository.AddMeetup(meetup);

        if (!(await _unitOfWork.CompleteAsync()))
            return BadRequest("Ошибка добавления митапа");

        return Ok();
    }

    [HttpGet("get-meetup-list")]
    public async Task<ActionResult<List<Meetup>>> GetMeetups()
    {
        var meetups = await _unitOfWork.MeetupRepository.GetMeetups();

        if (meetups == null)
            return BadRequest("Не удалось получить митапы");

        return meetups;
    }

    [HttpGet("get-meetup/{id}")]
    public async Task<ActionResult<Meetup>> GetMeetupById(int id)
    {
        var meetup = await _unitOfWork.MeetupRepository.GetMeetupById(id);

        if (meetup == null)
            return BadRequest("Не удалось получить митап");

        return meetup;
    }

    [HttpPut("update-meetup")]
    public async Task<ActionResult> UpdateMeetup(Meetup request)
    {
         var meetup = await _unitOfWork.MeetupRepository.UpdateMeetup(request);

        if (meetup == null)
            return NotFound("Нет удалось найти митап");
        
        if (!(await _unitOfWork.CompleteAsync()))
            return BadRequest("Не удалось обновить данные митапа");

        return Ok();
    }

    [HttpDelete("delete-meetup/{id}")]
    public async Task<ActionResult> DeleteMeetup(int id)
    {
        var meetup = await _unitOfWork.MeetupRepository.DeleteMeetup(id);

        if (meetup == null)
            return NotFound("Не удалось найти митап");
        
        if(!(await _unitOfWork.CompleteAsync()))
            return BadRequest("Не удалось удалить митап");

        return Ok();
    }
}
