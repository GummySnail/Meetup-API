using AutoMapper;
using Meetup_API.Dtos.Meetup;
using Meetup_API.Entities;
using Meetup_API.Helpers;
using Meetup_API.Interfaces.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

    [HttpPost]
    [Authorize (Roles = "Organizer")]
    public async Task<ActionResult> AddMeetup(MeetupAddDto meetupDto)
    {
        var OwnerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        _unitOfWork.MeetupRepository.AddMeetup(_mapper.Map<MeetupAddDto, Meetup>(meetupDto), OwnerId);

        if (!(await _unitOfWork.CompleteAsync()))
        {
            return BadRequest("Ошибка добавления митапа");
        }

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<ResponseMeetupDto>>> GetMeetups([FromQuery]MeetupParams meetupParams)
    {
        var Meetups = await _unitOfWork.MeetupRepository.GetMeetupsAsync(meetupParams);
        
        List<ResponseMeetupDto> MeetupDtos = new List<ResponseMeetupDto>();

        foreach (var meetup in Meetups)
        {
            MeetupDtos.Add(_mapper.Map<Meetup, ResponseMeetupDto>(meetup));
        }

        return MeetupDtos;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseMeetupDto>> GetMeetup(int id)
    {
        var Meetup = await _unitOfWork.MeetupRepository.GetMeetupAsync(id);

        if (Meetup == null)
        {
            return NotFound("Не удалось получить митап");
        }     

        return _mapper.Map<Meetup, ResponseMeetupDto>(Meetup);
    }

    [HttpPut]
    [Authorize(Roles = "Organizer")]
    public async Task<ActionResult> UpdateMeetup(RequestMeetupDto request)
    {
        var UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (!(await _unitOfWork.MeetupRepository.IsOwner(UserId)))
        {
            return BadRequest("Вы не имеете права изменять чужой митап");
        }

        var Meetup = await _unitOfWork.MeetupRepository.UpdateMeetupAsync(_mapper.Map<RequestMeetupDto, Meetup>(request));

        if (Meetup == null)
        {
            return NotFound("Нет митапа с указанным Id");
        }
            
        if (!(await _unitOfWork.CompleteAsync()))
        {
            return BadRequest("Не удалось обновить данные митапа");
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Organizer")]
    public async Task<ActionResult> DeleteMeetup(int id)
    {
        var UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        if(!(await _unitOfWork.MeetupRepository.IsOwner(UserId)))
        {
            return BadRequest("Вы не имеете права удалять чужой митап");
        }

        var Meetup = await _unitOfWork.MeetupRepository.DeleteMeetupAsync(id);

        if (Meetup == null)
        {
            return NotFound("Нет митапа с указанным Id");
        }

        if(!(await _unitOfWork.CompleteAsync()))
        {
            return BadRequest("Не удалось удалить митап");
        }
     
        return Ok();
    }

    [HttpPost("sign-up")]
    [Authorize]
    public async Task<ActionResult> SignUpForMeetup(SignUpForMeetupDto request)
    {
        var UserMeetup = await _unitOfWork.MeetupRepository.SignUpForMeetupAsync(request);

        if (UserMeetup == null)
        {
            return NotFound();
        }

        if (!(await _unitOfWork.CompleteAsync()))
        {
            return BadRequest("Не удалось записаться на митап");
        }

        return Ok();
    }
}
