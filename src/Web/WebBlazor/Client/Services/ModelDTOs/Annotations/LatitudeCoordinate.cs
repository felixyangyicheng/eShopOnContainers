using System;
using System.ComponentModel.DataAnnotations;

namespace WebBlazor.Client.Services.ModelDTOs.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class LatitudeCoordinate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) =>
            (!double.TryParse(value.ToString(), out var coordinate) || (coordinate < -90 || coordinate > 90))
                ? new("Latitude must be between -90 and 90 degrees inclusive.", new[] { validationContext.MemberName })
                : ValidationResult.Success;
    }
}
