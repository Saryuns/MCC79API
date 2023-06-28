using API.Contracts;
using API.DTOs.AccountRoles;
using API.Models;

namespace API.Services;

public class AccountRoleService
{
    private readonly IAccountRoleRepository _accountroleRepository;

    public AccountRoleService(IAccountRoleRepository accountroleRepository)
    {
        _accountroleRepository = accountroleRepository;
    }

    public IEnumerable<AccountRoleDto>? GetAccountRole()
    {
        var accountroles = _accountroleRepository.GetAll();
        if (!accountroles.Any())
        {
            return null;
        }

        var toDto = accountroles.Select(accountrole =>
                                            new AccountRoleDto
                                            {
                                                Guid = accountrole.Guid,
                                                AccountGuid = accountrole.AccountGuid,
                                                RoleGuid = accountrole.RoleGuid
                                            }).ToList();

        return toDto;
    }

    public AccountRoleDto? GetAccountRole(Guid guid)
    {
        var accountrole = _accountroleRepository.GetByGuid(guid);
        if (accountrole is null)
        {
            return null;
        }

        var toDto = new AccountRoleDto
        {
            Guid = accountrole.Guid,
            AccountGuid = accountrole.AccountGuid,
            RoleGuid = accountrole.RoleGuid
        };

        return toDto;
    }

    public AccountRoleDto? CreateAccount(AccountRoleDto newAccountRoleDto)
    {
        var accountrole = new AccountRole
        {
            Guid = new Guid(),
            AccountGuid = newAccountRoleDto.AccountGuid,
            RoleGuid = newAccountRoleDto.RoleGuid,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdAccountRole = _accountroleRepository.Create(accountrole);
        if (createdAccountRole is null)
        {
            return null;
        }

        var toDto = new AccountRoleDto
        {
            Guid = createdAccountRole.Guid,
            AccountGuid = createdAccountRole.AccountGuid,
            RoleGuid = createdAccountRole.RoleGuid
        };

        return toDto;
    }

    public int UpdateAccountRole(AccountRoleDto updateAccountRoleDto)
    {
        var isExist = _accountroleRepository.IsExist(updateAccountRoleDto.Guid);
        if (!isExist)
        {
            return -1;
        }

        var getAccountRole = _accountroleRepository.GetByGuid(updateAccountRoleDto.Guid);

        var accountrole = new AccountRole
        {
            Guid = updateAccountRoleDto.Guid,
            AccountGuid = updateAccountRoleDto.AccountGuid,
            RoleGuid = updateAccountRoleDto.RoleGuid,
            ModifiedDate = DateTime.Now,
            CreatedDate = getAccountRole!.CreatedDate
        };

        var isUpdate = _accountroleRepository.Update(accountrole);
        if (!isUpdate)
        {
            return 0;
        }

        return 1;
    }

    public int DeleteAccountRole(Guid guid)
    {
        var isExist = _accountroleRepository.IsExist(guid);
        if (!isExist)
        {
            return -1;
        }

        var accountrole = _accountroleRepository.GetByGuid(guid);
        var isDelete = _accountroleRepository.Delete(accountrole!);
        if (!isDelete)
        {
            return 0;
        }

        return 1;
    }
}