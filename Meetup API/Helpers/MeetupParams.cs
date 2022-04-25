namespace Meetup_API.Helpers;

public class MeetupParams
{
    private const int MaxPageSize = 10;
    public int PageNumber { get; set; } = 1;
    private int _pageSize = 3;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
    public string City { get; set; } = "";
    public string Name { get; set; } = "";
    public string OrderByDateTime { get; set; } = "Upcoming";
}
