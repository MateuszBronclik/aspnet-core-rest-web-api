using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace RestaurantAPI.ApiServices
{
    public interface IUserContextService
    {
        ClaimsPrincipal User { get; }
        int? GetUserId { get; }
    }
    public class UserContextService : IUserContextService
    {
        
        private readonly IHttpContextAccessor _httpContextAccessorAccessor;
        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessorAccessor = httpContextAccessor;
        }
        public ClaimsPrincipal User => _httpContextAccessorAccessor.HttpContext?.User;

        public int? GetUserId => User is null ? null : (int?)int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
}
