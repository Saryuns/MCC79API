using System.Security.Claims;
using API.Contracts;
using API.Data;
using API.DTOs.Accounts;
using API.Models;
using API.Utilities.Handlers;
using API.Utilities.Handler;

namespace API.Services;

public class AccountService
{
    private readonly BookingDbContext _bookingDbContext;
    private readonly IAccountRepository _accountRepository;
    private readonly IUniversityRepository _universityRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly IAccountRoleRepository _accountRoleRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ITokenHandler _tokenHandler;
    private readonly IEmailHandler _emailHandler;

    public AccountService(BookingDbContext bookingDbContext,
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
        _bookingDbContext = bookingDbContext;
        _tokenHandler = tokenHandler;
    }

    //Register
    public RegisterDto? Register(RegisterDto registerDto)
    {

        if (registerDto.Password != registerDto.ConfirmPassword)
        {
            return null;
        }

        EmployeeService employeeService = new EmployeeService(_employeeRepository,
                                                              _educationRepository,
                                                              _universityRepository);

        using var transaction = _bookingDbContext.Database.BeginTransaction();
        try
        {
            Employee employee = new Employee
            {
                Guid = new Guid(),
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
            employee.NIK = GenerateNik.NIK(_employeeRepository.GetLastEmployeeNik());

            var createdEmployee = _employeeRepository.Create(employee);
            if (createdEmployee is null)
            {
                return null;
            }

            var universityEntity = _universityRepository.GetByCodeandName(registerDto.UniversityCode, registerDto.UniversityName);
            if (universityEntity is null)
            {
                var university = new University
                {
                    Code = registerDto.UniversityCode,
                    Name = registerDto.UniversityName,
                    Guid = new Guid(),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };
                universityEntity = _universityRepository.Create(university);
            }

            Education education = new Education
            {
                Guid = employee.Guid,
                Major = registerDto.Major,
                Degree = registerDto.Degree,
                GPA = registerDto.GPA,
                UniversityGuid = universityEntity.Guid
            };

            var createdEducation = _educationRepository.Create(education);
            if (createdEducation is null)
            {
                return null;
            }


            Account account = new Account
            {
                Guid = employee.Guid,
                Password = Hashing.Hash(registerDto.Password)
            };

            var createdAccount = _accountRepository.Create(account);
            if (createdAccount is null)
            {
                return null;
            }

            var getRoleUser = _roleRepository.GetByName("User");
            _accountRoleRepository.Create(new AccountRole
            {
                Guid = new Guid(),
                AccountGuid = account.Guid,
                RoleGuid = getRoleUser.Guid
            });


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
                UniversityCode = universityEntity.Code,
                UniversityName = universityEntity.Name,
            };


            transaction.Commit();
            return toDto;
        }
        catch (Exception)
        {
            transaction.Rollback();
            return null;
        }
    }

    //Login
    public string Login(LoginDto loginDto)
    {
        var employee = _employeeRepository.GetEmployeeByEmail(loginDto.Email);
        if (employee == null)
        {
            return "0";
        }

        var password = _accountRepository.GetByGuid(employee.Guid);
        var isPasswordValid = Hashing.Validate(loginDto.Password, password!.Password);
        if (!isPasswordValid)
        {
            return "-1";
        }


        try
        {
            var claims = new List<Claim>()
                {
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
            return getToken;
        }
        catch (Exception)
        {

            return "-2";
        }
    }

    //Change password
    public int ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var isExist = _employeeRepository.GetEmployeeByEmail(changePasswordDto.Email);
        if (isExist is null)
            return -1;

        var getAccount = _accountRepository.GetByGuid(isExist.Guid);
        if (getAccount.OTP != changePasswordDto.OTP)
            return 0;

        if (getAccount.IsUsed == true)
            return 1;

        if (getAccount.ExpiredTime < DateTime.Now)
            return 2;

        var account = new Account
        {
            Guid = getAccount.Guid,
            IsUsed = getAccount.IsUsed,
            IsDeleted = getAccount.IsDeleted,
            ModifiedDate = DateTime.Now,
            CreatedDate = getAccount!.CreatedDate,
            OTP = changePasswordDto.OTP,
            ExpiredTime = getAccount.ExpiredTime,
            Password = Hashing.Hash(changePasswordDto.NewPassword)
        };
        var isUpdated = _accountRepository.Update(account);
        if (!isUpdated)
        {
            return 0;
        }
        return 3;
    }

    //Forgot password
    public int ForgotPassword(ForgotPasswordDto forgotPassword)
    {
        var employee = _employeeRepository.GetEmployeeByEmail(forgotPassword.Email);
        if (employee == null)
        {
            return -1;
        }

        Random rand = new Random();
        HashSet<int> uniqueDigits = new HashSet<int>();

        while (uniqueDigits.Count < 6)
        {
            int digit = rand.Next(0, 9);
            uniqueDigits.Add(digit);
        }

        int generateOTP = uniqueDigits.Aggregate(0, (acc, digit) => acc * 10 + digit);

        var relatedAccount = GetAccount(employee.Guid);

        var updateAccountDto = new AccountDto
        {
            Guid = relatedAccount.Guid,
            Password = relatedAccount.Password,
            IsDeleted = relatedAccount.IsDeleted,
            OTP = generateOTP,
            IsUsed = false,
            ExpiredTime = DateTime.Now.AddMinutes(5)
        };
        var updateResult = UpdateAccount(updateAccountDto);
        if (updateResult == 0)
        {
            return 0;
        }

        _emailHandler.SendEmail(forgotPassword.Email,
            "Forgot Password",
            $"Your OTP is {updateAccountDto.OTP}");

        return 1;
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
                Guid = account.Guid,
                Password = account.Password,
                IsDeleted = account.IsDeleted,
                OTP = account.OTP,
                IsUsed = account.IsUsed,
                ExpiredTime = account.ExpiredTime
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
