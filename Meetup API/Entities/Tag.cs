﻿namespace Meetup_API.Entities;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Meetup> Meetups { get; set; }
}
