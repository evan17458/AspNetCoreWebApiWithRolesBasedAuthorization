using System.Text.Json.Serialization;

namespace WebApiWithRoleAuthentication.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }

        [JsonIgnore]
        public ICollection<LineItemDto>? OrderItems { get; set; }
        public string? State { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public string? TransactionMetadata { get; set; }
    }
}