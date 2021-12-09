
namespace WebBlazor.Client.Services.ModelDTOs.Annotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public sealed class LongitudeCoordinate : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext) =>
        (!double.TryParse(value.ToString(), out var coordinate) || (coordinate < -180 || coordinate > 180))
            ? new("Longitude must be between -180 and 180 degrees inclusive.", new[] { validationContext.MemberName })
            : ValidationResult.Success;
}
