using API.Models;

namespace API.Contracts;

public interface IUniversityRepository : IGeneralRepository<University>
{
    IEnumerable<University> GetByName(string name);

    University? GetByCodeandName(string code, string name);
    
    University? CreateWithDuplicateCheck(University university);
}