using API.Contracts;
using API.DTOs.Bookings;
using API.Models;
using API.Utilities.Enums;

namespace API.Services;

public class BookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IRoomRepository _roomRepository;

    public BookingService(IBookingRepository bookingRepository,
                          IEmployeeRepository employeeRepository,
                          IRoomRepository roomRepository)
    {
        _bookingRepository = bookingRepository;
        _employeeRepository = employeeRepository;
        _roomRepository = roomRepository;
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

    //Get Booking Today
    public IEnumerable<BookingTodayDto> BookingToday()
    {
        var bookings = _bookingRepository.GetAll();

        if (bookings == null)
        {
            return null;
        }

        var employees = _employeeRepository.GetAll();
        var rooms = _roomRepository.GetAll();

        var detailBookings = (from booking in bookings
                              join employee in employees on booking.EmployeeGuid equals employee.Guid
                              join room in rooms on booking.RoomGuid equals room.Guid
                              where booking.StartDate <= DateTime.Now.Date && booking.EndDate >= DateTime.Now
                              select new BookingTodayDto
                              {
                                  BookingGuid = booking.Guid,
                                  RoomName = room.Name,
                                  Status = booking.Status,
                                  Floor = room.Floor,
                                  BookedBy = employee.FirstName + " " + employee.LastName,
                              }).ToList();
        if (!detailBookings.Any())
        {
            return null;
        }

        return detailBookings;
    }

    // Get Detail Booking
    public IEnumerable<BookingDetailDto>? BookingsDetail()
    {
        var bookings = _bookingRepository.GetAll();

        if (bookings == null)
        {
            return null;
        }

        var employees = _employeeRepository.GetAll();
        var rooms = _roomRepository.GetAll();

        var detailBookings = (from booking in bookings
                              join employee in employees on booking.EmployeeGuid equals employee.Guid
                              join room in rooms on booking.RoomGuid equals room.Guid
                              select new BookingDetailDto
                              {
                                  Guid = booking.Guid,
                                  BookedNik = employee.NIK,
                                  BookedBy = employee.FirstName + "" + employee.LastName,
                                  StartDate = DateTime.Now,
                                  EndDate = booking.EndDate,
                                  RoomName = room.Name,
                                  Status = booking.Status,
                              }).ToList();
        if (!detailBookings.Any())
        {
            return null;
        }

        return detailBookings;
    }

    // Get Booking By Guid
    public BookingDetailDto? BookingDetail(Guid guid)
    {
        var booking = BookingsDetail();

        var bookingByGuid = booking!.FirstOrDefault(booking => booking.Guid == guid);

        return bookingByGuid;
    }

    // Get Booking Duration
    public IEnumerable<BookingLengthDto> BookingDuration()
    {
        var bookings = _bookingRepository.GetAll();
        var rooms = _roomRepository.GetAll();

        var entities = (from booking in bookings
                        join room in rooms on booking.RoomGuid equals room.Guid
                        select new
                        {
                            guid = room.Guid,
                            startDate = booking.StartDate,
                            endDate = booking.EndDate,
                            roomName = room.Name
                        }).ToList();

        var bookingDurations = new List<BookingLengthDto>();

        foreach (var entity in entities)
        {
            TimeSpan duration = entity.endDate - entity.startDate;

            int totalDays = (int)duration.TotalDays;
            int weekends = 0;

            for (int i = 0; i <= totalDays; i++)
            {
                var currentDate = entity.startDate.AddDays(i);
                if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    weekends++;
                }
            }

            TimeSpan bookingLength = duration - TimeSpan.FromDays(weekends);

            var bookingDurationDto = new BookingLengthDto
            {
                RoomGuid = entity.guid,
                RoomName = entity.roomName,
                BookingLength = bookingLength
            };

            bookingDurations.Add(bookingDurationDto);
        }
        return bookingDurations;

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
            Guid = booking.Guid,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            Remarks = booking.Remarks,
            RoomGuid = booking.RoomGuid,
            EmployeeGuid = booking.EmployeeGuid,
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