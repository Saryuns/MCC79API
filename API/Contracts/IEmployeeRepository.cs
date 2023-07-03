using API.Models;

namespace API.Contracts
{
    public interface IEmployeeRepository : IGeneralRepository<Employee>
    {
        Employee? GetByEmail(string email);

        //add
        bool IsDuplicateValue(string value);
    }
}