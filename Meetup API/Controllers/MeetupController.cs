using AutoMapper;
using Meetup_API.Dtos.Meetup;
using Meetup_API.Entities;
using Meetup_API.Helpers;
using Meetup_API.Interfaces.Data;
using Microsoft.AspNetCore.Mvc;

namespace Meetup_API.Controllers;

public class MeetupController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MeetupController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpPost("add-meetup")]
    public async Task<ActionResult> AddMeetup(MeetupAddDto meetupDto)
    {
        
        _unitOfWork.MeetupRepository.AddMeetup(_mapper.Map<MeetupAddDto, Meetup>(meetupDto));

        if (!(await _unitOfWork.CompleteAsync()))
            return BadRequest("Ошибка добавления митапа");

        return Ok();
    }

    [HttpGet("get-meetup-list")]
    public async Task<ActionResult<List<MeetupDto>>> GetMeetups([FromQuery]MeetupParams meetupParams)
    {
        var meetups = await _unitOfWork.MeetupRepository.GetMeetupsAsync(meetupParams);

        if (meetups == null)
            return BadRequest("Не удалось получить митапы");
        
        List<MeetupDto> meetupDtos = new List<MeetupDto>();

        foreach (var meetup in meetups)
            meetupDtos.Add(_mapper.Map<Meetup, MeetupDto>(meetup));
        
        return meetupDtos;
    }

    [HttpGet("get-meetup/{id}")]
    public async Task<ActionResult<MeetupDto>> GetMeetup(int id)
    {
        var meetup = await _unitOfWork.MeetupRepository.GetMeetupAsync(id);

        if (meetup == null)
            return BadRequest("Не удалось получить митап");

        return _mapper.Map<Meetup, MeetupDto>(meetup);
    }

    [HttpPut("update-meetup")]
    public async Task<ActionResult> UpdateMeetup(MeetupDto request)
    {
         var meetup = await _unitOfWork.MeetupRepository.UpdateMeetupAsync(_mapper.Map<MeetupDto, Meetup>(request));

        if (meetup == null)
            return NotFound("Нет удалось найти митап");
        
        if (!(await _unitOfWork.CompleteAsync()))
            return BadRequest("Не удалось обновить данные митапа");

        return Ok();
    }

    [HttpDelete("delete-meetup/{id}")]
    public async Task<ActionResult> DeleteMeetup(int id)
    {
        var meetup = await _unitOfWork.MeetupRepository.DeleteMeetupAsync(id);

        if (meetup == null)
            return NotFound("Не удалось найти митап");
        
        if(!(await _unitOfWork.CompleteAsync()))
            return BadRequest("Не удалось удалить митап");

        return Ok();
    }
}
