using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly BookingDbContext _context;

    public EmployeeRepository(BookingDbContext context)
    {
        _context = context;
    }

    public ICollection<Employee> GetAll()
    {
        return _context.Set<Employee>().ToList();
    }

    public Employee? GetByGuid(Guid guid)
    {
        return _context.Set<Employee>().Find(guid);
    }

    public Employee Create(Employee employee)
    {
        try
        {
            _context.Set<Employee>().Add(employee);
            _context.SaveChanges();
            return employee;
        }
        catch
        {
            return new Employee();
        }
    }

    public bool Update(Employee employee)
    {
        try
        {
            _context.Set<Employee>().Update(employee);
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
            var employee = GetByGuid(guid);
            if (employee is null)
            {
                return false;
            }

            _context.Set<Employee>().Remove(employee);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}