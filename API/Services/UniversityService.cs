using API.Contracts;
using API.DTOs.Universities;
using API.Models;

namespace API.Services;

public class UniversityService
{
    private readonly IUniversityRepository _universityRepository;

    public UniversityService(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public IEnumerable<UniversityDto>? GetUniversity()
    {
        var universities = _universityRepository.GetAll();
        if (!universities.Any())
        {
            return null;
        }

        var toDto = universities.Select(university =>
                                            new UniversityDto
                                            {
                                                Guid = university.Guid,
                                                Code = university.Code,
                                                Name = university.Name
                                            }).ToList();

        return toDto;
    }

    public IEnumerable<UniversityDto>? GetUniversity(string name)
    {
        var universities = _universityRepository.GetByName(name);
        if (!universities.Any())
        {
            return null;
        }

        var toDto = universities.Select(university =>
                                            new UniversityDto
                                            {
                                                Guid = university.Guid,
                                                Code = university.Code,
                                                Name = university.Name
                                            }).ToList();

        return toDto;
    }

    public UniversityDto? GetUniversity(Guid guid)
    {
        var university = _universityRepository.GetByGuid(guid);
        if (university is null)
        {
            return null;
        }

        var toDto = new UniversityDto
        {
            Guid = university.Guid,
            Code = university.Code,
            Name = university.Name
        };

        return toDto;
    }

    public UniversityDto? CreateUniversity(NewUniversityDto newUniversityDto)
    {
        var university = new University
        {
            Code = newUniversityDto.Code,
            Name = newUniversityDto.Name,
            Guid = new Guid(),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdUniversity = _universityRepository.Create(university);
        if (createdUniversity is null)
        {
            return null;
        }

        var toDto = new UniversityDto
        {
            Guid = createdUniversity.Guid,
            Code = createdUniversity.Code,
            Name = createdUniversity.Name
        };

        return toDto;
    }

    public int UpdateUniversity(UniversityDto updateUniversityDto)
    {
        var isExist = _universityRepository.IsExist(updateUniversityDto.Guid);
        if (!isExist)
        {
            return -1;
        }

        var getUniversity = _universityRepository.GetByGuid(updateUniversityDto.Guid);

        var university = new University
        {
            Guid = updateUniversityDto.Guid,
            Code = updateUniversityDto.Code,
            Name = updateUniversityDto.Name,
            ModifiedDate = DateTime.Now,
            CreatedDate = getUniversity!.CreatedDate
        };

        var isUpdate = _universityRepository.Update(university);
        if (!isUpdate)
        {
            return 0;
        }

        return 1;
    }

    public int DeleteUniversity(Guid guid)
    {
        var isExist = _universityRepository.IsExist(guid);
        if (!isExist)
        {
            return -1;
        }

        var university = _universityRepository.GetByGuid(guid);
        var isDelete = _universityRepository.Delete(university!);
        if (!isDelete)
        {
            return 0;
        }

        return 1;
    }
}