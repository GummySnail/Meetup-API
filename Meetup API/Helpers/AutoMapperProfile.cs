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
        CreateMap<RequestMeetupDto, Meetup>()
            .ForMember(rmd => rmd.OwnerId, opt => opt.Ignore());
        CreateMap<MeetupAddDto, Meetup>()
            .ForMember(ma => ma.Id, opt => opt.Ignore())
            .ForMember(ma => ma.OwnerId, opt => opt.Ignore());
        CreateMap<TagDto, Tag>()
            .ForMember(t => t.Id, opt => opt.Ignore())
            .ForMember(t => t.MeetupId, opt => opt.Ignore());
        CreateMap<UserRegistrationDto, User>()
            .ForMember(ur => ur.Id, opt => opt.Ignore())
            .ForMember(ur => ur.PasswordHash, opt => opt.Ignore())
            .ForMember(ur => ur.PasswordSalt, opt => opt.Ignore());
        CreateMap<SignUpForMeetupDto, UserMeetup>();
        CreateMap<ResponseMeetupDto, Meetup>();

        /*--   Entity to DTO   --*/
        CreateMap<Meetup, RequestMeetupDto>();
        CreateMap<Meetup, MeetupAddDto>();
        CreateMap<Tag, TagDto>();
        CreateMap<User, UserRegistrationDto>();
        CreateMap<UserMeetup, SignUpForMeetupDto>();
        CreateMap<Meetup, ResponseMeetupDto>();
    }

}
