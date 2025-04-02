using WebApiWithRoleAuthentication.Dtos;

using System.ComponentModel.DataAnnotations;

namespace WebApiWithRoleAuthentication.ValidationAttributes
{
    public class TouristRouteTitleMustBeDifferentFromDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object? value,
            ValidationContext validationContext
        )
        {
            var touristRouteDto = (TouristRouteForCreationDto)validationContext.ObjectInstance;
            if (touristRouteDto.Title == touristRouteDto.Description)
            {
                return new ValidationResult(
                    "路線名稱必须和路線描述不同",
                    new[] { "TouristRouteForCreationDto" }
                );
            }
            return ValidationResult.Success!;
        }
    }
}
