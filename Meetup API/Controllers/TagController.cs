using AutoMapper;
using Meetup_API.Dtos.Tag;
using Meetup_API.Entities;
using Meetup_API.Interfaces.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Meetup_API.Controllers;

public class TagController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TagController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpPost]
    [Authorize(Roles = "Organizer")]
    public async Task<ActionResult> AddTag(TagDto tagDto)
    {
        _unitOfWork.TagRepository.AddTag(_mapper.Map<TagDto,Tag>(tagDto));

        if (!(await _unitOfWork.CompleteAsync()))
        {
            return BadRequest("Ошибка добавления тега");
        }

        return Ok();
    }
}
