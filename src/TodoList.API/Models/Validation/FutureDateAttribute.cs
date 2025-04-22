using System.ComponentModel.DataAnnotations;

namespace TodoList.API.Models.Validation;

[AttributeUsage(AttributeTargets.Property)]
public class FutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
            return true; // Null values are considered valid (use Required if needed)
            
        if (value is DateTime dateTime)
        {
            return dateTime.Date >= DateTime.UtcNow.Date;
        }
        
        return false;
    }
    
    public override string FormatErrorMessage(string name)
    {
        return $"{name} cannot be in the past";
    }
} 