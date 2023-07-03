namespace API.Contracts;

public interface IGeneralRepository<TEntity>
{
    //add
    IEnumerable<TEntity> GetAll();

    TEntity? GetByGuid(Guid guid);
    TEntity? Create(TEntity entity);
    bool Update(TEntity entity);
    bool Delete(TEntity entity);
    bool IsExist(Guid guid);
}