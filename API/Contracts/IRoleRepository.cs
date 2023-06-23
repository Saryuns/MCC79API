using API.Models;

namespace API.Contracts;

public interface IRoleRepository
{
    ICollection<Role> GetAll();
    Role? GetByGuid(Guid guid);
    Role Create(Role role);
    bool Update(Role role);
    bool Delete(Guid guid);
}