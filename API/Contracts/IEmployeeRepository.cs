using API.Models;

namespace API.Contracts
{
    public interface IEmployeeRepository : IGeneralRepository<Employee>
    {
        Employee? GetEmployeeByEmail(string email);

        string? GetLastEmployeeNik();

        Employee? GetByEmailAndPhoneNumber(string nama);

        bool IsDuplicateValue(string value);
    }
}