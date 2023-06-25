using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace API.Controllers;

public class GeneralController<TEntity> : ControllerBase where TEntity : class
{
    private readonly IRepository<TEntity> _repository;

    public GeneralController(IRepository<TEntity> repository)
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
        var entity = _repository.GetById(id);
        if (entity is null)
        {
            return NotFound();
        }

        return Ok(entity);
    }

    [HttpGet("{name}")]
    public IActionResult GetByName(string name)
    {
        var entity = _repository.GetByName(name);
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
    public IActionResult Update(Guid id, TEntity entity)
    {
        var isUpdated = _repository.Update(id, entity);
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