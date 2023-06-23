using API.Models;

namespace API.Contracts;

public interface IRoomRepository
{
    ICollection<Room> GetAll();
    Room? GetByGuid(Guid guid);
    Room Create(Room room);
    bool Update(Room room);
    bool Delete(Guid guid);
}