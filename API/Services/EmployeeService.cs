using API.Contracts;
using API.DTOs.Employees;
using API.Models;
using API.Utilities.Handler;

namespace API.Services;

public class EmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly IUniversityRepository _universityRepository;

    public EmployeeService(IEmployeeRepository employeeRepository,
                           IEducationRepository educationRepository,
                           IUniversityRepository universityRepository)
    {
        _employeeRepository = employeeRepository;
        _educationRepository = educationRepository;
        _universityRepository = universityRepository;
    }

    public IEnumerable<EmployeeDto>? GetEmployee()
    {
        var employees = _employeeRepository.GetAll();
        if (!employees.Any())
        {
            return null;
        }

        var toDto = employees.Select(employee =>
                                            new EmployeeDto
                                            {
                                                Guid = employee.Guid,
                                                NIK = employee.NIK,
                                                FirstName = employee.FirstName,
                                                LastName = employee.LastName,
                                                BirthDate = employee.BirthDate,
                                                Gender = employee.Gender,
                                                HiringDate = employee.HiringDate,
                                                Email = employee.Email,
                                                PhoneNumber = employee.PhoneNumber
                                            }).ToList();

        return toDto;
    }

    public EmployeeDto? GetEmployee(Guid guid)
    {
        var employee = _employeeRepository.GetByGuid(guid);
        if (employee is null)
        {
            return null;
        }

        var toDto = new EmployeeDto
        {
            Guid = employee.Guid,
            NIK = employee.NIK,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            BirthDate = employee.BirthDate,
            Gender = employee.Gender,
            HiringDate = employee.HiringDate,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber
        };

        return toDto;
    }

    // GetAll Data Master Employee
    public IEnumerable<EmployeeEducationDto>? GetAllMaster()
    {
        var master = (from e in _employeeRepository.GetAll()
                      join education in _educationRepository.GetAll() on e.Guid equals education.Guid
                      join u in _universityRepository.GetAll() on education.UniversityGuid equals u.Guid
                      select new EmployeeEducationDto
                      {
                          Guid = e.Guid,
                          FullName = e.FirstName + " " + e.LastName,
                          Nik = e.NIK,
                          BirthDate = e.BirthDate,
                          Email = e.Email,
                          Gender = e.Gender,
                          HiringDate = e.HiringDate,
                          PhoneNumber = e.PhoneNumber,
                          Major = education.Major,
                          Degree = education.Degree,
                          Gpa = education.GPA,
                          UniversityName = u.Name
                      }).ToList();
        if (!master.Any())
        {
            return null;
        }

        return master;
    }

    // GetByGuid data master
    public EmployeeEducationDto? GetMasterByGuid(Guid guid)
    {
        var master = GetAllMaster();
        var masterByGuid = master.FirstOrDefault(master => master.Guid == guid);

        return masterByGuid;
    }

    public EmployeeDto? CreateEmployee(NewEmployeeDto newEmployeeDto)
    {
        var employee = new Employee
        {
            Guid = new Guid(),
            FirstName = newEmployeeDto.FirstName,
            LastName = newEmployeeDto.LastName,
            BirthDate = newEmployeeDto.BirthDate,
            Gender = newEmployeeDto.Gender,
            HiringDate = newEmployeeDto.HiringDate,
            Email = newEmployeeDto.Email,
            PhoneNumber = newEmployeeDto.PhoneNumber,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
        employee.NIK = GenerateNik.NIK(_employeeRepository.GetLastEmployeeNik());
        _employeeRepository.Create(employee);

        var createdEmployee = _employeeRepository.Create(employee);
        if (createdEmployee is null)
        {
            return null;
        }

        var toDto = new EmployeeDto
        {
            Guid = employee.Guid,
            NIK = employee.NIK,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            BirthDate = employee.BirthDate,
            Gender = employee.Gender,
            HiringDate = employee.HiringDate,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
        };

        return toDto;
    }

    public int UpdateEmployee(EmployeeDto updateEmployeeDto)
    {
        var isExist = _employeeRepository.IsExist(updateEmployeeDto.Guid);
        if (!isExist)
        {
            return -1;
        }

        var getEmployee = _employeeRepository.GetByGuid(updateEmployeeDto.Guid);

        var employee = new Employee
        {
            Guid = updateEmployeeDto.Guid,
            NIK =  updateEmployeeDto.NIK,
            FirstName = updateEmployeeDto.FirstName,
            LastName = updateEmployeeDto.LastName,
            BirthDate = updateEmployeeDto.BirthDate,
            Gender = updateEmployeeDto.Gender,
            HiringDate = updateEmployeeDto.HiringDate,
            Email = updateEmployeeDto.Email,
            PhoneNumber = updateEmployeeDto.PhoneNumber,
            ModifiedDate = DateTime.Now,
            CreatedDate = getEmployee!.CreatedDate
        };

        var isUpdate = _employeeRepository.Update(employee);
        if (!isUpdate)
        {
            return 0;
        }

        return 1;
    }

    public int DeleteEmployee(Guid guid)
    {
        var isExist = _employeeRepository.IsExist(guid);
        if (!isExist)
        {
            return -1;
        }

        var employee = _employeeRepository.GetByGuid(guid);
        var isDelete = _employeeRepository.Delete(employee!);
        if (!isDelete)
        {
            return 0;
        }

        return 1;
    }
}