using API.Models;

namespace API.Contracts;

public interface IRoleRepository : IGeneralRepository<Role>
{
    //add
    Role? GetByName(string name);
}