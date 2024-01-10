
namespace WebBlazor.Client.Services.ModelDTOs.Annotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public sealed class LatitudeCoordinate : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(validationContext);

        return (!double.TryParse(value.ToString(), out var coordinate) || (coordinate < -90 || coordinate > 90))
            ? new("Latitude must be between -90 and 90 degrees inclusive.", new[] { validationContext.MemberName })
            : ValidationResult.Success;
    }
}
