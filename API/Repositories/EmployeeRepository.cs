using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories;

public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(BookingDbContext context) : base(context) { }

    public Employee? GetByEmail(string email)
    {
        /*return _context.Set<Employee>().Where(e => e.Email.Single(email));*/
        return _context.Employees.SingleOrDefault(e => e.Email == email);
    }
}