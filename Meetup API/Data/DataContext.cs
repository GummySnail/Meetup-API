using Meetup_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Meetup_API.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options){}
    
    public DbSet<Meetup> Meetups { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<UserMeetup> UserMeetups { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserMeetup>()
            .HasKey(um => new {um.MeetupId, um.UserId});

        modelBuilder.Entity<UserMeetup>()
            .HasOne(um => um.User)
            .WithMany(u => u.UserMeetups)
            .HasForeignKey(um => um.UserId);

        modelBuilder.Entity<UserMeetup>()
            .HasOne(um => um.Meetup)
            .WithMany(m => m.UserMeetups)
            .HasForeignKey(um => um.MeetupId);

    }
}
