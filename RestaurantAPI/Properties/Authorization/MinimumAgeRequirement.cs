using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Properties.Authorization
{
    public class MinimumAgeRequirement : IAuthorizationRequirement
    {
        public int MinumumAge { get; }
        public MinimumAgeRequirement(int minumumAge)
        {
            MinumumAge = minumumAge;
        }
    }
}
