using API.Utilities.Enums;

namespace API.DTOs.Bookings;

public class BookingTodayDto
{
    public Guid BookingGuid { get; set; }
    public string RoomName { get; set; }
    public StatusLevel Status { get; set; }
    public int Floor { get; set; }
    public string BookedBy { get; set; }
}