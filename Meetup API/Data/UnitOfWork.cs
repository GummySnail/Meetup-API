using AutoMapper;
using Meetup_API.Data.Repositories;
using Meetup_API.Interfaces.Data;

namespace Meetup_API.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public UnitOfWork(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public IMeetupRepository MeetupRepository => new MeetupRepository(_dataContext);
    public ITagRepository TagRepository => new TagRepository(_dataContext);
    public IUserRepository UserRepository => new UserRepository(_dataContext, _mapper);

    public async Task<bool> CompleteAsync() => await _dataContext.SaveChangesAsync() > 0;
}
