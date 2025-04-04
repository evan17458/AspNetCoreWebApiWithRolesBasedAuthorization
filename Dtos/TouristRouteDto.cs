using WebApiWithRoleAuthentication.Enums;

namespace WebApiWithRoleAuthentication.Dtos
{
    public class TouristRouteDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        // 計算方式：原價 * 折扣
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public double? DiscountPresent { get; set; }
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
        public ICollection<TouristRoutePictureDto>? TouristRoutePictures { get; set; }
    }
}
