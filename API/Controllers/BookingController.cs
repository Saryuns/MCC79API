using API.Contracts;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingController : GeneralController<IBookingRepository, Booking>
{
    public BookingController(IBookingRepository repository) : base(repository)
    {
    }
}