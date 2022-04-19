using AutoMapper;
using Meetup_API.Dtos.Meetup;
using Meetup_API.Dtos.Tag;
using Meetup_API.Dtos.User;
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
        CreateMap<TagDto, Tag>()
            .ForMember(t=> t.Id, opt => opt.Ignore());
        CreateMap<UserRegistrationDto, User>()
            .ForMember(ur => ur.Id, opt => opt.Ignore())
            .ForMember(ur => ur.PasswordHash, opt => opt.Ignore())
            .ForMember(ur => ur.PasswordSalt, opt => opt.Ignore());


        /*--   Entity to DTO   --*/
        CreateMap<Meetup, MeetupDto>();
        CreateMap<Meetup, MeetupAddDto>();
        CreateMap<Tag, TagDto>();
        CreateMap<User, UserRegistrationDto>();
    }

}
