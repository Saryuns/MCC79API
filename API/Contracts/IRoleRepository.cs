using API.Models;

namespace API.Contracts;

public interface IRoleRepository : IGeneralRepository<Role>
{

}

/*
namespace API.Contracts;

public interface IRepository<TEntity> where TEntity : class
{
    IEnumerable<TEntity> GetAll();
    TEntity GetById(Guid id);
    TEntity Create(TEntity entity);
    bool Update(Guid id, TEntity entity);
    bool Delete(Guid id);
}
*/