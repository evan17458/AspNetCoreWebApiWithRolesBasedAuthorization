using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace WebApiWithRoleAuthentication.Dtos
{
    public class TouristRouteForUpdateDto
    {
        [Required(ErrorMessage = "title 不可为空")]
        [MaxLength(100)]
        public string? Title { get; set; }
        [Required]
        [MaxLength(1500)]
        public string? Description { get; set; }
        // 计算方式：原价 * 折扣
        public decimal Price { get; set; }
        //public decimal OriginalPrice { get; set; }
        //public double? DiscountPresent { get; set; }
        //public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? DepartureTime { get; set; }
        public string? Features { get; set; }
        public string? Fees { get; set; }
        public string? Notes { get; set; }
        public double? Rating { get; set; }
        [DefaultValue("one")]
        public string? TravelDays { get; set; }
        [DefaultValue("PrivateGroup")]
        public string? TripType { get; set; }

        [DefaultValue("Beijing")]
        public string? DepartureCity { get; set; }
        public ICollection<TouristRoutePictureForCreationDto> TouristRoutePictures { get; set; }
            = new List<TouristRoutePictureForCreationDto>();
    }
}