using System.Linq;
using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Security
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserPhoneNo()
        {
            var phoneNo = _httpContextAccessor.HttpContext.User?.Claims?
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            return phoneNo;
        }
        public string GetUserRole()
        {
            var role = _httpContextAccessor.HttpContext.User?.Claims?
                     .FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            return role;
        }
    }
}