using Meetup_API.Data.Repositories;
using Meetup_API.Interfaces.Data;

namespace Meetup_API.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dataContext;
    public UnitOfWork(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public IMeetupRepository MeetupRepository => new MeetupRepository(_dataContext);

    public async Task<bool> CompleteAsync() => await _dataContext.SaveChangesAsync() > 0;
}
