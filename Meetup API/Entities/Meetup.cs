namespace Meetup_API.Entities
{
    public class Meetup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int HomeNumber { get; set; }
        public DateTimeOffset StartMeetupDateTime { get; set; }
        public DateTimeOffset EndMeetupDateTime { get; set; }
        public Tag Tag { get; set; }
        public int OwnerId { get; set; }
        public ICollection<User> Owner { get; set; }
        public ICollection<UserMeetup> UserMeetups { get; set; }
    }
}
