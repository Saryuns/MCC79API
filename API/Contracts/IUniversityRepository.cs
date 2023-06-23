using API.Models;

namespace API.Contracts;

public interface IUniversityRepository
{
    ICollection<University> GetAll();
    University? GetByGuid(Guid guid);
    University Create(University university);
    bool Update(University university);
    bool Delete(Guid guid);
}