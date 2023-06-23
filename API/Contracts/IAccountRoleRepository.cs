using API.Models;

namespace API.Contracts;

public interface IAccountRoleRepository
{
    ICollection<AccountRole> GetAll();
    AccountRole? GetByGuid(Guid guid);
    AccountRole Create(AccountRole accountrole);
    bool Update(AccountRole accountrole);
    bool Delete(Guid guid);
}