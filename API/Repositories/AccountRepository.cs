using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly BookingDbContext _context;

    public AccountRepository(BookingDbContext context)
    {
        _context = context;
    }

    public ICollection<Account> GetAll()
    {
        return _context.Set<Account>().ToList();
    }

    public Account? GetByGuid(Guid guid)
    {
        return _context.Set<Account>().Find(guid);
    }

    public Account Create(Account account)
    {
        try
        {
            _context.Set<Account>().Add(account);
            _context.SaveChanges();
            return account;
        }
        catch
        {
            return new Account();
        }
    }

    public bool Update(Account account)
    {
        try
        {
            _context.Set<Account>().Update(account);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool Delete(Guid guid)
    {
        try
        {
            var account = GetByGuid(guid);
            if (account is null)
            {
                return false;
            }

            _context.Set<Account>().Remove(account);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}