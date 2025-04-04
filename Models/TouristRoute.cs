using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApiWithRoleAuthentication.Enums;

namespace WebApiWithRoleAuthentication.Models
{
    public class TouristRoute
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Title { get; set; }
        [Required]
        [MaxLength(1500)]
        public string? Description { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal OriginalPrice { get; set; }
        [Range(0.0, 1.0)]
        public double? DiscountPresent { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
        public DateTime UpdateTime { get; set; }
        public DateTime? DepartureTime { get; set; }
        [MaxLength]
        public string? Features { get; set; }
        [MaxLength]
        public string? Fees { get; set; }
        [MaxLength]
        public string? Notes { get; set; }
        public double? Rating { get; set; }

        [JsonIgnore]
        public ICollection<TouristRoutePicture> TouristRoutePictures { get; set; }
        = new List<TouristRoutePicture>();
        public TravelDays TravelDays { get; set; } = TravelDays.One;
        public TripType TripType { get; set; } = TripType.HotelAndAttractions;
        public DepartureCity DepartureCity { get; set; } = DepartureCity.Beijing;
    }
}
