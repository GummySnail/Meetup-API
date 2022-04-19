using Meetup_API.Entities;
using Meetup_API.Interfaces.Data;

namespace Meetup_API.Data.Repositories;

public class TagRepository : ITagRepository
{
    private readonly DataContext _dataContext;

    public TagRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public void AddTag(Tag tag)
    {
        _dataContext.Tags.Add(tag);
    }
}
