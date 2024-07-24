using System.Security.Claims;

namespace App.Services
{
    public interface IUserContextService
    {
        ClaimsPrincipal User { get; }
        int? UserId { get; }    
    }

    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserContextService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public ClaimsPrincipal User => _contextAccessor.HttpContext?.User;
        public int? UserId => User is null ? null : (int?)int.Parse(User.FindFirst(p => p.Type == ClaimTypes.NameIdentifier).Value);

    }
}
