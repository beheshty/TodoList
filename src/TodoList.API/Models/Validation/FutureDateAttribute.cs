using System.ComponentModel.DataAnnotations;

namespace TodoList.API.Models.Validation;

[AttributeUsage(AttributeTargets.Property)]
public class FutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        return value switch
        {
            null => true,
            DateTime dateTime => dateTime.Date >= DateTime.UtcNow.Date,
            _ => false
        };
    }
    
    public override string FormatErrorMessage(string name)
    {
        return $"{name} cannot be in the past";
    }
} 