using API.Models;

namespace API.Contracts;

public interface IEmployeeRepository
{
    ICollection<Employee> GetAll();
    Employee? GetByGuid(Guid guid);
    Employee Create(Employee employee);
    bool Update(Employee employee);
    bool Delete(Guid guid);
}