using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories;

public class AccountRoleRepository : IAccountRoleRepository
{
    private readonly BookingDbContext _context;

    public AccountRoleRepository(BookingDbContext context)
    {
        _context = context;
    }

    public ICollection<AccountRole> GetAll()
    {
        return _context.Set<AccountRole>().ToList();
    }

    public AccountRole? GetByGuid(Guid guid)
    {
        return _context.Set<AccountRole>().Find(guid);
    }

    public AccountRole Create(AccountRole accountrole)
    {
        try
        {
            _context.Set<AccountRole>().Add(accountrole);
            _context.SaveChanges();
            return accountrole;
        }
        catch
        {
            return new AccountRole();
        }
    }

    public bool Update(AccountRole accountrole)
    {
        try
        {
            _context.Set<AccountRole>().Update(accountrole);
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
            var accountrole = GetByGuid(guid);
            if (accountrole is null)
            {
                return false;
            }

            _context.Set<AccountRole>().Remove(accountrole);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}