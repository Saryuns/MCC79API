using API.Contracts;
using API.DTOs.Educations;
using API.Models;
using API.Utilities.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace API.Services;

public class EducationService
{
    private readonly IEducationRepository _educationRepository;

    public EducationService(IEducationRepository educationRepository)
    {
        _educationRepository = educationRepository;
    }

    public IEnumerable<EducationDto>? GetEducation()
    {
        var educations = _educationRepository.GetAll();
        if (!educations.Any())
        {
            return null;
        }

        var toDto = educations.Select(education =>
                                            new EducationDto
                                            {
                                                Guid = education.Guid,
                                                Major = education.Major,
                                                Degree = education.Degree,
                                                GPA = education.GPA,
                                                UniversityGuid = education.UniversityGuid
                                            }).ToList();

        return toDto;
    }

    public EducationDto? GetEducation(Guid guid)
    {
        var education = _educationRepository.GetByGuid(guid);
        if (education is null)
        {
            return null;
        }

        var toDto = new EducationDto
        {
            Guid = education.Guid,
            Major = education.Major,
            Degree = education.Degree,
            GPA = education.GPA,
            UniversityGuid = education.UniversityGuid
        };

        return toDto;
    }

    public EducationDto? CreateEducation(EducationDto newEducationDto)
    {
        var education = new Education
        {
            Guid = newEducationDto.Guid,
            Major = newEducationDto.Major,
            Degree = newEducationDto.Degree,
            GPA = newEducationDto.GPA,
            UniversityGuid = newEducationDto.UniversityGuid,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdEducation = _educationRepository.Create(education);
        if (createdEducation is null)
        {
            return null;
        }

        var toDto = new EducationDto
        {
            Guid = createdEducation.Guid,
            Major = createdEducation.Major,
            Degree = createdEducation.Degree,
            GPA = createdEducation.GPA,
            UniversityGuid = createdEducation.UniversityGuid
        };

        return toDto;
    }

    public int UpdateEducation(EducationDto updateEducationDto)
    {
        var isExist = _educationRepository.IsExist(updateEducationDto.Guid);
        if (!isExist)
        {
            return -1;
        }

        var getEducation = _educationRepository.GetByGuid(updateEducationDto.Guid);

        var education = new Education
        {
            Guid = updateEducationDto.Guid,
            Major = updateEducationDto.Major,
            Degree = updateEducationDto.Degree,
            GPA = updateEducationDto.GPA,
            UniversityGuid = updateEducationDto.UniversityGuid,
            ModifiedDate = DateTime.Now,
            CreatedDate = getEducation!.CreatedDate
        };

        var isUpdate = _educationRepository.Update(education);
        if (!isUpdate)
        {
            return 0;
        }

        return 1;
    }

    public int DeleteEducation(Guid guid)
    {
        var isExist = _educationRepository.IsExist(guid);
        if (!isExist)
        {
            return -1;
        }

        var education = _educationRepository.GetByGuid(guid);
        var isDelete = _educationRepository.Delete(education!);
        if (!isDelete)
        {
            return 0;
        }

        return 1;
    }
}