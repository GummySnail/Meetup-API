using Meetup_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Meetup_API.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options){}
    public DbSet<Meetup> Meetups { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
