

using System.ComponentModel.DataAnnotations;
using WebApiWithRoleAuthentication.Enums;
using WebApiWithRoleAuthentication.ValidationAttributes;

namespace WebApiWithRoleAuthentication.Dtos
{

    [TitleMustBeDifferentFromDescriptionAttribute("Title", "Description")]
    public class TouristRouteForCreationDto //: IValidatableObject

    {
        [Required(ErrorMessage = "title 不可為空")]
        [MaxLength(100)]
        public string? Title { get; set; }
        [Required]
        [MaxLength(1500)]
        public string? Description { get; set; }
        public decimal Price { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? DepartureTime { get; set; }
        public string? Features { get; set; }
        public string? Fees { get; set; }
        public string? Notes { get; set; }
        public double? Rating { get; set; }
        public TravelDays TravelDays { get; set; } = TravelDays.One;
        public TripType TripType { get; set; } = TripType.HotelAndAttractions;
        public DepartureCity DepartureCity { get; set; } = DepartureCity.Beijing;

        public ICollection<TouristRoutePictureForCreationDto> TouristRoutePictures { get; set; }
        = new List<TouristRoutePictureForCreationDto>();

        // public IEnumerable<ValidationResult> Validate(
        //    ValidationContext validationContext)
        // {
        //     if (Title == Description)
        //     {
        //         yield return new ValidationResult(
        //             "路線名稱必须和路線描述不同",
        //             new[] { "TouristRouteForCreationDto" }
        //         );
        //     }
        // }
    }
}