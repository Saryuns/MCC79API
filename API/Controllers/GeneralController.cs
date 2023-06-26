using API.Contracts;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
public class GeneralController<TRepository, TEntity> : ControllerBase
    where TRepository : IGeneralRepository<TEntity>
    where TEntity : class
{
    protected readonly TRepository _repository;

    public GeneralController(TRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _repository.GetAll();

        if (!entities.Any())
        {
            return NotFound();
        }

        return Ok(entities);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var entity = _repository.GetByGuid(id);
        if (entity is null)
        {
            return NotFound();
        }

        return Ok(entity);
    }

    [HttpPost]
    public IActionResult Create(TEntity entity)
    {
        var createdEntity = _repository.Create(entity);
        return Ok(createdEntity);
    }

    [HttpPut("{id}")]
    public IActionResult Update(TEntity entity)
    {
        var isUpdated = _repository.Update(entity);
        if (!isUpdated)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var isDeleted = _repository.Delete(id);
        if (!isDeleted)
        {
            return NotFound();
        }

        return Ok();
    }
}