using API.Contracts;
using API.DTOs.Roles;
using API.Models;

namespace API.Services;

public class RoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public IEnumerable<RoleDto>? GetRole()
    {
        var roles = _roleRepository.GetAll();
        if (!roles.Any())
        {
            return null;
        }

        var toDto = roles.Select(role =>
                                            new RoleDto
                                            {
                                                Guid = role.Guid,
                                                Name = role.Name
                                            }).ToList();

        return toDto;
    }

    public RoleDto? GetRole(Guid guid)
    {
        var role = _roleRepository.GetByGuid(guid);
        if (role is null)
        {
            return null;
        }

        var toDto = new RoleDto
        {
            Guid = role.Guid,
            Name = role.Name
        };

        return toDto;
    }

    public RoleDto? CreateRole(NewRoleDto newRoleDto)
    {
        var role = new Role
        {
            Guid = new Guid(),
            Name = newRoleDto.Name,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdRole = _roleRepository.Create(role);
        if (createdRole is null)
        {
            return null;
        }

        var toDto = new RoleDto
        {
            Guid = createdRole.Guid,
            Name = createdRole.Name
        };

        return toDto;
    }

    public int UpdateRole(RoleDto updateRoleDto)
    {
        var isExist = _roleRepository.IsExist(updateRoleDto.Guid);
        if (!isExist)
        {
            return -1;
        }

        var getRole = _roleRepository.GetByGuid(updateRoleDto.Guid);

        var role = new Role
        {
            Guid = updateRoleDto.Guid,
            Name = updateRoleDto.Name,
            ModifiedDate = DateTime.Now,
            CreatedDate = getRole!.CreatedDate
        };

        var isUpdate = _roleRepository.Update(role);
        if (!isUpdate)
        {
            return 0;
        }

        return 1;
    }

    public int DeleteRole(Guid guid)
    {
        var isExist = _roleRepository.IsExist(guid);
        if (!isExist)
        {
            return -1;
        }

        var role = _roleRepository.GetByGuid(guid);
        var isDelete = _roleRepository.Delete(role!);
        if (!isDelete)
        {
            return 0;
        }

        return 1;
    }
}