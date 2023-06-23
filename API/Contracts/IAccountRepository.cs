using API.Models;

namespace API.Contracts;

public interface IAccountRepository
{
    ICollection<Account> GetAll();
    Account? GetByGuid(Guid guid);
    Account Create(Account account);
    bool Update(Account account);
    bool Delete(Guid guid);
}