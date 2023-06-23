using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly BookingDbContext _context;

    public RoomRepository(BookingDbContext context)
    {
        _context = context;
    }

    //GetAll
    public ICollection<Room> GetAll()
    {
        return _context.Set<Room>().ToList();
    }

    //GetById
    public Room? GetByGuid(Guid guid)
    {
        return _context.Set<Room>().Find(guid);
    }

    //Create
    public Room Create(Room room)
    {
        try
        {
            _context.Set<Room>().Add(room);
            _context.SaveChanges();
            return room;
        }
        catch
        {
            return new Room();
        }
    }

    //Update
    public bool Update(Room room)
    {
        try
        {
            _context.Set<Room>().Update(room);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    //Delete
    public bool Delete(Guid guid)
    {
        try
        {
            var room = GetByGuid(guid);
            if (room is null)
            {
                return false;
            }

            _context.Set<Room>().Remove(room);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
