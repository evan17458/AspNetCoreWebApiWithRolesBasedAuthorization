using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace WebApiWithRoleAuthentication.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Address { get; set; }
        [JsonIgnore]
        public ShoppingCart? ShoppingCart { get; set; }

        [JsonIgnore]
        public ICollection<Order>? Orders { get; set; }

        [JsonIgnore]
        public virtual ICollection<IdentityUserRole<string>>? UserRoles { get; set; }

        [JsonIgnore]
        public virtual ICollection<IdentityUserClaim<string>>? Claims { get; set; }
        //public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
        //public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }
    }
}