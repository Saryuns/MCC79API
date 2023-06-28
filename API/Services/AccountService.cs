using API.Contracts;
using API.DTOs.Accounts;
using API.Models;
using API.Utilities.Enums;

namespace API.Services;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUniversityRepository _universityRepository;
    private readonly IEducationRepository _educationRepository;

    public AccountService(IAccountRepository accountRepository,
                            IEmployeeRepository employeeRepository,
                            IUniversityRepository universityRepository,
                            IEducationRepository educationRepository)
    {
        _accountRepository = accountRepository;
        _employeeRepository = employeeRepository;
        _universityRepository = universityRepository;
        _educationRepository = educationRepository;
    }

    public RegisterDto? Register(RegisterDto registerDto)
    {

        if (registerDto.Password != registerDto.ConfirmPassword)
        {
            return null;
        }

        EmployeeService employeeService = new EmployeeService(_employeeRepository);
        Employee employee = new Employee
        {
            Guid = new Guid(),
            NIK = employeeService.GenerateNIK(),
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            BirthDate = registerDto.BirthDate,
            Gender = registerDto.Gender,
            HiringDate = registerDto.HiringDate,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdEmployee = _employeeRepository.Create(employee);
        if (createdEmployee is null)
        {
            return null;
        }

        University university = new University
        {
            Guid = new Guid(),
            Code = registerDto.UniversityCode,
            Name = registerDto.UniversityName
        };

        var createdUniversity = _universityRepository.Create(university);
        if (createdUniversity is null)
        {
            return null;
        }

        Education education = new Education
        {
            Guid = employee.Guid,
            Major = registerDto.Major,
            Degree = registerDto.Degree,
            GPA = registerDto.GPA,
            UniversityGuid = university.Guid
        };

        var createdEducation = _educationRepository.Create(education);
        if (createdEducation is null)
        {
            return null;
        }

        Account account = new Account
        {
            Guid = employee.Guid,
            Password = Hashing.HashPassword(registerDto.Password),
            //ConfirmPassword = registerDto.ConfirmPassword
        };


        var createdAccount = _accountRepository.Create(account);
        if (createdAccount is null)
        {
            return null;
        }


        var toDto = new RegisterDto
        {
            FirstName = createdEmployee.FirstName,
            LastName = createdEmployee.LastName,
            BirthDate = createdEmployee.BirthDate,
            Gender = createdEmployee.Gender,
            HiringDate = createdEmployee.HiringDate,
            Email = createdEmployee.Email,
            PhoneNumber = createdEmployee.PhoneNumber,
            Password = createdAccount.Password,
            Major = createdEducation.Major,
            Degree = createdEducation.Degree,
            GPA = createdEducation.GPA,
            UniversityCode = createdUniversity.Code,
            UniversityName = createdUniversity.Name
        };

        return toDto;
    }

    public LoginDto Login(LoginDto loginDto)
    {
        var employee = _employeeRepository.GetByEmail(loginDto.Email);
        if (employee == null)
        {
            throw new Exception("Account not found");
        }

        var account = _accountRepository.GetByGuid(employee.Guid);
        var isPasswordValid = Hashing.ValidatePassword(loginDto.Password, account.Password);
        if (!isPasswordValid)
        {
            throw new Exception("Password is invalid");
        }

        var toDto = new LoginDto
        {
            Email = loginDto.Email,
            Password = loginDto.Password,
        };

        return toDto;
    }

    public IEnumerable<AccountDto>? GetAccount()
    {
        var accounts = _accountRepository.GetAll();
        if (!accounts.Any())
        {
            return null;
        }

        var toDto = accounts.Select(account =>
                                            new AccountDto
                                            {
                                                Guid = account.Guid,
                                                Password = account.Password,
                                                IsDeleted = account.IsDeleted,
                                                OTP = account.OTP,
                                                IsUsed = account.IsUsed,
                                                ExpiredTime = account.ExpiredTime
                                            }).ToList();

        return toDto;
    }

    public AccountDto? GetAccount(Guid guid)
    {
        var account = _accountRepository.GetByGuid(guid);
        if (account is null)
        {
            return null;
        }

        var toDto = new AccountDto
        {
            Guid = account.Guid,
            Password = account.Password,
            IsDeleted = account.IsDeleted,
            OTP = account.OTP,
            IsUsed = account.IsUsed,
            ExpiredTime = account.ExpiredTime
        };

        return toDto;
    }

    public AccountDto? CreateAccount(AccountDto newAccountDto)
    {
        var account = new Account
        {
            Guid = newAccountDto.Guid,
            Password = Hashing.HashPassword(newAccountDto.Password),
            IsDeleted = newAccountDto.IsDeleted,
            OTP = newAccountDto.OTP,
            IsUsed = newAccountDto.IsUsed,
            ExpiredTime = newAccountDto.ExpiredTime,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdAccount = _accountRepository.Create(account);
        if (createdAccount is null)
        {
            return null;
        }

        var toDto = new AccountDto
        {
            Guid = createdAccount.Guid,
            Password = createdAccount.Password,
            IsDeleted = createdAccount.IsDeleted,
            OTP = createdAccount.OTP,
            IsUsed = createdAccount.IsUsed,
            ExpiredTime = createdAccount.ExpiredTime
        };

        return toDto;
    }

    public int UpdateAccount(AccountDto updateAccountDto)
    {
        var isExist = _accountRepository.IsExist(updateAccountDto.Guid);
        if (!isExist)
        {
            return -1;
        }

        var getAccount = _accountRepository.GetByGuid(updateAccountDto.Guid);

        var account = new Account
        {
            Guid = updateAccountDto.Guid,
            Password = Hashing.HashPassword(updateAccountDto.Password),
            IsDeleted = updateAccountDto.IsDeleted,
            OTP = updateAccountDto.OTP,
            IsUsed = updateAccountDto.IsUsed,
            ExpiredTime = updateAccountDto.ExpiredTime,
            ModifiedDate = DateTime.Now,
            CreatedDate = getAccount!.CreatedDate
        };

        var isUpdate = _accountRepository.Update(account);
        if (!isUpdate)
        {
            return 0;
        }

        return 1;
    }

    public int DeleteAccount(Guid guid)
    {
        var isExist = _accountRepository.IsExist(guid);
        if (!isExist)
        {
            return -1;
        }

        var account = _accountRepository.GetByGuid(guid);
        var isDelete = _accountRepository.Delete(account!);
        if (!isDelete)
        {
            return 0;
        }

        return 1;
    }
}
