using API.Contracts;
using API.DTOs.Bookings;
using API.Models;
using API.Utilities.Enums;

namespace API.Services;

public class BookingService
{
    private readonly IBookingRepository _bookingRepository;

    public BookingService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public IEnumerable<BookingDto>? GetBooking()
    {
        var bookings = _bookingRepository.GetAll();
        if (!bookings.Any())
        {
            return null;
        }

        var toDto = bookings.Select(booking =>
                                            new BookingDto
                                            {
                                                Guid = booking.Guid,
                                                StartDate = booking.StartDate,
                                                EndDate = booking.EndDate,
                                                Status = booking.Status,
                                                Remarks = booking.Remarks,
                                                RoomGuid = booking.RoomGuid,
                                                EmployeeGuid = booking.EmployeeGuid
                                            }).ToList();

        return toDto;
    }

    public BookingDto? GetBooking(Guid guid)
    {
        var booking = _bookingRepository.GetByGuid(guid);
        if (booking is null)
        {
            return null;
        }

        var toDto = new BookingDto
        {
            Guid = booking.Guid,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            Status = booking.Status,
            Remarks = booking.Remarks,
            RoomGuid = booking.RoomGuid,
            EmployeeGuid = booking.EmployeeGuid
        };

        return toDto;
    }

    public BookingDto? CreateBooking(NewBookingDto newBookingDto)
    {
        var booking = new Booking
        {
            Guid = new Guid(),
            StartDate = newBookingDto.StartDate,
            EndDate = newBookingDto.EndDate,
            Remarks = newBookingDto.Remarks,
            RoomGuid = newBookingDto.RoomGuid,
            EmployeeGuid = newBookingDto.EmployeeGuid,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdBooking = _bookingRepository.Create(booking);
        if (createdBooking is null)
        {
            return null;
        }

        var toDto = new BookingDto
        {
            Guid = createdBooking.Guid,
            StartDate = createdBooking.StartDate,
            EndDate = createdBooking.EndDate,
            Remarks = createdBooking.Remarks,
            RoomGuid = createdBooking.RoomGuid,
            EmployeeGuid = createdBooking.EmployeeGuid,
        };

        return toDto;
    }

    public int UpdateBooking(BookingDto updateBookingDto)
    {
        var isExist = _bookingRepository.IsExist(updateBookingDto.Guid);
        if (!isExist)
        {
            return -1;
        }

        var getBooking = _bookingRepository.GetByGuid(updateBookingDto.Guid);

        var booking = new Booking
        {
            Guid = updateBookingDto.Guid,
            StartDate = updateBookingDto.StartDate,
            EndDate = updateBookingDto.EndDate,
            Status = updateBookingDto.Status,
            Remarks = updateBookingDto.Remarks,
            RoomGuid = updateBookingDto.RoomGuid,
            EmployeeGuid = updateBookingDto.EmployeeGuid,
            ModifiedDate = DateTime.Now,
            CreatedDate = getBooking!.CreatedDate
        };

        var isUpdate = _bookingRepository.Update(booking);
        if (!isUpdate)
        {
            return 0;
        }

        return 1;
    }

    public int DeleteBooking(Guid guid)
    {
        var isExist = _bookingRepository.IsExist(guid);
        if (!isExist)
        {
            return -1;
        }

        var booking = _bookingRepository.GetByGuid(guid);
        var isDelete = _bookingRepository.Delete(booking!);
        if (!isDelete)
        {
            return 0;
        }

        return 1;
    }
}