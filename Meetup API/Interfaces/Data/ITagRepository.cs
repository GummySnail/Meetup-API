using Meetup_API.Entities;

namespace Meetup_API.Interfaces.Data;

public interface ITagRepository
{
    void AddTag(Tag tag);
}
