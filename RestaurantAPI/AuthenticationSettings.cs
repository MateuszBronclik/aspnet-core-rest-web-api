using System.Security.Policy;

namespace RestaurantAPI
{
    public class AuthenticationSettings
    {
        public int JwtKey { get; set; }
        public int JwtExpireDays { get; set; }
        public string JwtIssuer { get; set;}
    }
}
