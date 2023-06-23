using API.Models;

namespace API.Contracts;

public interface IEducationRepository
{
    ICollection<Education> GetAll();
    Education? GetByGuid(Guid guid);
    Education Create(Education education);
    bool Update(Education education);
    bool Delete(Guid guid);
}
