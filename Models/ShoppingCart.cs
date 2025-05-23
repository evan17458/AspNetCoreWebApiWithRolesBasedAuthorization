using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApiWithRoleAuthentication.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            // 在建構函式中初始化集合
            ShoppingCartItems = new List<LineItem>();
        }
        [Key]
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        [JsonIgnore]
        public ApplicationUser? User { get; set; }
        public ICollection<LineItem>? ShoppingCartItems { get; set; }
    }
}