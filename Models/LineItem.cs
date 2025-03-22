
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApiWithRoleAuthentication.Models;

namespace FWebApiWithRoleAuthentication.Models
{
    public class LineItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("TouristRouteId")]
        public Guid TouristRouteId { get; set; }

        [JsonIgnore]
        public TouristRoute? TouristRoute { get; set; }
        public Guid? ShoppingCartId { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal OriginalPrice { get; set; }
        [Range(0.0, 1.0)]
        public double? DiscountPresent { get; set; }

    }
}