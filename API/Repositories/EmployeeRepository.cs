using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories;

public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(BookingDbContext context) : base(context) { }

    public Employee? GetEmployeeByEmail(string email)
    {
        return _context.Set<Employee>().SingleOrDefault(e => e.Email == email);
    }

    public string? GetLastEmployeeNik()
    {
        return _context.Set<Employee>().ToList().Select(e => e.NIK).LastOrDefault();
    }

    public Employee? GetByEmailAndPhoneNumber(string data)
    {
        return _context.Set<Employee>().FirstOrDefault(e => e.PhoneNumber == data || e.Email == data);
    }

    
    public bool IsDuplicateValue(string value)
    {
        return _context.Set<Employee>()
                       .FirstOrDefault(e => e.Email.Contains(value) || e.PhoneNumber.Contains(value)) is null;
    }
}