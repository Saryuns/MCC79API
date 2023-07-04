using API.Contracts;
using System.ComponentModel.DataAnnotations;

namespace API.Utilities;

public class EmployeeDuplicatePropertyAttribute : ValidationAttribute
{
    private readonly string _guidPropertyName;
    private readonly string _propertyName;

    public EmployeeDuplicatePropertyAttribute(string guidPropertyName, string propertyName)
    {
        _guidPropertyName = guidPropertyName;
        _propertyName = propertyName;
    }

    /*
     * <summary>
     * This method is used to validate the uniqueness of the email and phone number.
     * </summary>
     * <param name="value">The value of the property to validate.</param>
     * <param name="validationContext">The context information about the validation operation.</param>
     * <returns>ValidationResult</returns>
     * <remarks>
     * This method will check if email and phone number is exist or not
     * </remarks>
     */

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null) return ValidationResult.Success;

        var employeeRepository = (IEmployeeRepository)validationContext.GetService(typeof(IEmployeeRepository))!;
        var guidProperty = validationContext.ObjectType.GetProperty(_guidPropertyName);
        var guidValue = guidProperty?.GetValue(validationContext.ObjectInstance, null) as Guid?;

        var entity = employeeRepository.GetByEmailAndPhoneNumber(value.ToString());
        if (entity is null) return ValidationResult.Success;
        return (entity.Guid == guidValue ? ValidationResult.Success : new ValidationResult($"{_propertyName} '{value}' already exists."))!;
    }
}