using AutoMapper;
using Meetup_API.Dtos.Meetup;
using Meetup_API.Entities;

namespace Meetup_API.Helpers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        /*--   DTO to Entity   --*/
        CreateMap<MeetupDto, Meetup>();
        CreateMap<MeetupAddDto, Meetup>()
            .ForMember(ma => ma.Id, opt => opt.Ignore());

        /*--   Entity to DTO   --*/
        CreateMap<Meetup, MeetupDto>();
        CreateMap<Meetup, MeetupAddDto>();
    }

}
