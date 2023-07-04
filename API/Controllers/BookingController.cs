using System.Net;
using API.DTOs.Bookings;
using API.Services;
using API.Utilities.Enums;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/bookings")]
[Authorize(Roles = $"{nameof(RoleType.Admin)}")]
public class BookingController : ControllerBase
{
    private readonly BookingService _service;

    public BookingController(BookingService service)
    {
        _service = service;
    }

    // GetAll Detail booking
    [HttpGet("detail-booking-today")]
    public IActionResult BookingDetailToday()
    {
        var entities = _service.BookingToday();

        if (entities == null)
        {
            return NotFound(new ResponseHandler<BookingTodayDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<BookingTodayDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = entities
        });
    }

    //Get Booking Detail
    [HttpGet("detail-booking")]
    [AllowAnonymous]
    public IActionResult BookingsDetail()
    {
        var bookings = _service.BookingsDetail();

        if (bookings == null)
        {
            return NotFound(new ResponseHandler<BookingDetailDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<BookingDetailDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = bookings
        });
    }

    // GetByGuid Booking
    [HttpGet("detail-booking/{guid}")]
    public IActionResult BookingDetailByGuid(Guid guid)
    {
        var booking = _service.BookingDetail(guid);

        if (booking == null)
        {
            return NotFound(new ResponseHandler<BookingDetailDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

        return Ok(new ResponseHandler<BookingDetailDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = booking
        });
    }

    //Get Booking Duration
    [HttpGet("bookinglength")]
    public IActionResult CalculateBookingLength()
    {
        var entities = _service.BookingDuration();
        if (entities == null || !entities.Any())
        {
            return NotFound(new ResponseHandler<BookingLengthDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<BookingLengthDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = entities
        });
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _service.GetBooking();

        if (entities == null)
        {
            return NotFound(new ResponseHandler<BookingDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<BookingDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entities
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var booking = _service.GetBooking(guid);
        if (booking is null)
        {
            return NotFound(new ResponseHandler<BookingDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        return Ok(new ResponseHandler<BookingDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = booking
        });
    }

    [HttpPost]
    public IActionResult Create(NewBookingDto newBookingDto)
    {
        var createBooking = _service.CreateBooking(newBookingDto);
        if (createBooking is null)
        {
            return BadRequest(new ResponseHandler<BookingDto>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data Not Created"
            });
        }

        return Ok(new ResponseHandler<BookingDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully Created",
            Data = createBooking
        });
    }

    [HttpPut]
    public IActionResult Update(BookingDto updateBookingDto)
    {
        var update = _service.UpdateBooking(updateBookingDto);
        if (update is -1)
        {
            return NotFound(new ResponseHandler<BookingDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "ID Not Found"
            });
        }
        if (update is 0)
        {
            return BadRequest(new ResponseHandler<BookingDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check Your Data"
            });
        }

        return Ok(new ResponseHandler<BookingDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully Updated"
        });
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var delete = _service.DeleteBooking(guid);

        if (delete is -1)
        {
            return NotFound(new ResponseHandler<BookingDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "ID Not Found"
            });
        }
        if (delete is 0)
        {
            return BadRequest(new ResponseHandler<BookingDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check Connection to Database"
            });
        }

        return Ok(new ResponseHandler<BookingDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully Deleted"
        });
    }
}