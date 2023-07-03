using System.Security.Claims;
using API.Contracts;
using API.Data;
using API.DTOs.AccountRoles;
using API.DTOs.Accounts;
using API.DTOs.Educations;
using API.DTOs.Employees;
using API.DTOs.Universities;
using API.Models;
using API.Utilities.Handlers;
using API.Utilities.Enums;

namespace API.Services;

public class AccountService
{
    private readonly BookingDbContext _context;
    private readonly IAccountRepository _accountRepository;
    private readonly IUniversityRepository _universityRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly IAccountRoleRepository _accountRoleRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ITokenHandler _tokenHandler;
    private readonly IEmailHandler _emailHandler;

    public AccountService(BookingDbContext context,
                          IAccountRepository accountRepository,
                          IUniversityRepository universityRepository,
                          IEmployeeRepository employeeRepository,
                          IEducationRepository educationRepository,
                          IAccountRoleRepository accountRoleRepository,
                          IRoleRepository roleRepository,
                          IEmailHandler emailHandler,
                          ITokenHandler tokenHandler)
    {
        _accountRepository = accountRepository;
        _universityRepository = universityRepository;
        _employeeRepository = employeeRepository;
        _educationRepository = educationRepository;
        _accountRoleRepository = accountRoleRepository;
        _roleRepository = roleRepository;
        _emailHandler = emailHandler;
        _context = context;
        _tokenHandler = tokenHandler;
    }

    //Change Password
    public int ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var employee = _employeeRepository.GetByEmail(changePasswordDto.Email);
        if (employee is null)
            return 0; // Email not found

        var account = _accountRepository.GetByGuid(employee.Guid);
        if (account is null)
            return 0; // Email not found

        if (account.IsUsed)
            return -1; // OTP is used

        if (account.OTP != changePasswordDto.OTP)
            return -2; // OTP is incorrect

        if (account.ExpiredTime < DateTime.Now)
            return -3; // OTP is expired

        var isUpdated = _accountRepository.Update(new Account
        {
            Guid = account.Guid,
            Password = Hashing.Hash(changePasswordDto.NewPassword),
            IsDeleted = account.IsDeleted,
            OTP = account.OTP,
            ExpiredTime = account.ExpiredTime,
            IsUsed = true,
            CreatedDate = account.CreatedDate,
            ModifiedDate = DateTime.Now
        });

        return isUpdated ? 1    // Success
                         : -4;  // Database Error
    }

    //Forgot Password
    public int ForgotPassword(ForgotPasswordDto forgotPassword)
    {
        var employee = _employeeRepository.GetByEmail(forgotPassword.Email);
        if (employee is null)
            return 0; // Email not found

        var account = _accountRepository.GetByGuid(employee.Guid);
        if (account is null)
            return -1;

        var otp = new Random().Next(111111, 999999);
        var isUpdated = _accountRepository.Update(new Account
        {
            Guid = account.Guid,
            Password = account.Password,
            IsDeleted = account.IsDeleted,
            OTP = otp,
            ExpiredTime = DateTime.Now.AddMinutes(5),
            IsUsed = false,
            CreatedDate = account.CreatedDate,
            ModifiedDate = DateTime.Now
        });

        if (!isUpdated)
            return -1;

        _emailHandler.SendEmail(forgotPassword.Email,
                                "Forgot Password",
                                $"Your OTP is {otp}");

        return 1;
    }

    //Register Account
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
            Password = Hashing.Hash(registerDto.Password),
            //Add Roles
            Roles = new List<RoleType> { RoleType.User }
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
    /*
    public RegisterDto? RegisterCheck(RegisterDto newEntity)
    {
        using var transaction = _dBContext.Database.BeginTransaction();
        try
        {
            var employee = new Employee
            {
                Guid = new Guid(),
                FirstName = newEntity.FirstName,
                LastName = newEntity.LastName ?? "",
                Gender = newEntity.Gender,
                HiringDate = newEntity.HiringDate,
                Email = newEntity.Email,
                NIK = GenerateNIK(newEntity.NIK),
                PhoneNumber = newEntity.PhoneNumber,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            _employeeRepository.Create(employee);

            var account = new Account
            {
                Guid = Employee.Guid,
                Password = Hashing.HashPassword(newEntity.Password),
                IsDeleted = false,
                IsUsed = false,
                OTP = 0,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                ExpiredDate = DateTime.Now.AddYears(5),
            };
            _accountRepository.Create(account);

            var universityEntity = _universityRepository.GetCodeandName(newEntity.UniversityCode, newEntity.UniversityName);
            if (universityEntity == null)
            {
                var university = new University
                {
                    Code = newEntity.UniversityCode,
                    Name = newEntity.UniversityName,
                    Guid = new Guid(),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };
                universityEntity = _universityRepository.Create(university);
            }

            var education = new Education
            {
                Guid = Employee.Guid,
                Degree = newEntity.Degree,
                Major = newEntity.Major,
                GPA = newEntity.GPA,
                UniversityGuid = universityEntity.Guid,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            _educationRepository.Create(education);

            var dto = new GetRegisterDto
            {
                Guid = Employee.Guid,
                Email = Employee.Email
            };
            transaction.Commit();
            return dto;
        }
        catch
        {
            transaction.Rollback();
            return null;
        }
    }*/

    //Login Account
    public LoginDto Login(LoginDto loginDto)
    {
        var employee = _employeeRepository.GetByEmail(loginDto.Email);
        if (employee == null)
        {
            throw new Exception("Account not found");
        }

        var account = _accountRepository.GetByGuid(employee.Guid);
        var isPasswordValid = Hashing.Validate(loginDto.Password, account.Password);
        if (!isPasswordValid)
        {
            throw new Exception("Password is invalid");
        }

        var toDto = new LoginDto
        {
            Email = loginDto.Email,
            Password = loginDto.Password
        };
        return toDto;

        try
        {
            var claims = new List<Claim>() {
                new Claim("NIK", employee.NIK),
                new Claim("FullName", $"{employee.FirstName} {employee.LastName}"),
                new Claim("Email", loginDto.Email)
            };

            var getAccountRole = _accountRoleRepository.GetAccountRolesByAccountGuid(employee.Guid);
            var getRoleNameByAccountRole = from ar in getAccountRole
                                       join r in _roleRepository.GetAll() on ar.RoleGuid equals r.Guid
                                       select r.Name;

            claims.AddRange(getRoleNameByAccountRole.Select(role => new Claim(ClaimTypes.Role, role)));

            var getToken = _tokenHandler.GenerateToken(claims);
            return null;
        }
        catch
        {
            return null;
        }
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
            Password = Hashing.Hash(newAccountDto.Password),
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
            Password = Hashing.Hash(updateAccountDto.Password),
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
