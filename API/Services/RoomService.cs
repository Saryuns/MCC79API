using API.Contracts;
using API.DTOs.Rooms;
using API.Models;
using API.Utilities.Enums;

namespace API.Services;

public class RoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IBookingRepository _bookingRepository;

    public RoomService(IRoomRepository roomRepository,
                       IBookingRepository bookingRepository)
    {
        _roomRepository = roomRepository;
        _bookingRepository = bookingRepository;
    }

    public IEnumerable<RoomDto>? GetRoom()
    {
        var rooms = _roomRepository.GetAll();
        if (!rooms.Any())
        {
            return null;
        }

        var toDto = rooms.Select(room =>
                                            new RoomDto
                                            {
                                                Guid = room.Guid,
                                                Name = room.Name,
                                                Floor = room.Floor,
                                                Capacity = room.Capacity
                                            }).ToList();

        return toDto;
    }

    public RoomDto? GetRoom(Guid guid)
    {
        var room = _roomRepository.GetByGuid(guid);
        if (room is null)
        {
            return null;
        }

        var toDto = new RoomDto
        {
            Guid = room.Guid,
            Name = room.Name,
            Floor = room.Floor,
            Capacity = room.Capacity
        };

        return toDto;
    }

    public IEnumerable<EmptyRoomDto> GetEmptyRoom()
    {
        var rooms = _roomRepository.GetAll();

        var bookings = _bookingRepository.GetAll();

        var usedRooms = (from room in rooms
                         join booking in bookings on room.Guid equals booking.RoomGuid
                         where booking.Status == StatusLevel.OnGoing
                         select new EmptyRoomDto
                         {
                             RoomGuid = room.Guid,
                             RoomName = room.Name,
                             Floor = room.Floor,
                             Capacity = room.Capacity
                         }).ToList();
        List<Room> tmpRooms = new List<Room>(rooms);

        foreach (var room in rooms)
        {
            foreach (var usedRoom in usedRooms)
            {
                if (room.Guid == usedRoom.RoomGuid)
                {
                    tmpRooms.Remove(room);
                    break;
                }
            }
        }

        var emptyRooms = from room in tmpRooms
                          select new EmptyRoomDto
                          {
                              RoomGuid = room.Guid,
                              RoomName = room.Name,
                              Floor = room.Floor,
                              Capacity = room.Capacity
                          };
        return emptyRooms;
    }

    public RoomDto? CreateRoom(NewRoomDto newRoomDto)
    {
        var room = new Room
        {
            Guid = new Guid(),
            Name = newRoomDto.Name,
            Floor = newRoomDto.Floor,
            Capacity = newRoomDto.Capacity,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdRoom = _roomRepository.Create(room);
        if (createdRoom is null)
        {
            return null;
        }

        var toDto = new RoomDto
        {
            Guid = room.Guid,
            Name = room.Name,
            Floor = room.Floor,
            Capacity = room.Capacity
        };

        return toDto;
    }

    public int UpdateRoom(RoomDto updateRoomDto)
    {
        var isExist = _roomRepository.IsExist(updateRoomDto.Guid);
        if (!isExist)
        {
            return -1;
        }

        var getRoom = _roomRepository.GetByGuid(updateRoomDto.Guid);

        var room = new Room
        {
            Guid = updateRoomDto.Guid,
            Name = updateRoomDto.Name,
            Floor = updateRoomDto.Floor,
            Capacity = updateRoomDto.Capacity,
            ModifiedDate = DateTime.Now,
            CreatedDate = getRoom!.CreatedDate
        };

        var isUpdate = _roomRepository.Update(room);
        if (!isUpdate)
        {
            return 0;
        }

        return 1;
    }

    public int DeleteRoom(Guid guid)
    {
        var isExist = _roomRepository.IsExist(guid);
        if (!isExist)
        {
            return -1;
        }

        var room = _roomRepository.GetByGuid(guid);
        var isDelete = _roomRepository.Delete(room!);
        if (!isDelete)
        {
            return 0;
        }

        return 1;
    }
}