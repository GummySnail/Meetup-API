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

    private IMeetupRepository _meetupRepository;
    private IUserRepository _userRepository;
    public IMeetupRepository MeetupRepository
    {
        get
        {
            if (_meetupRepository == null)
            {
                _meetupRepository = new MeetupRepository(_dataContext);
            }
            return _meetupRepository;
        }
    }
    public IUserRepository UserRepository
    {
        get
        {
            if(_userRepository == null)
            {
                _userRepository = new UserRepository(_dataContext, _mapper);
            }
            return _userRepository;
        }
    }
    public async Task<bool> CompleteAsync() => await _dataContext.SaveChangesAsync() > 0;
}
