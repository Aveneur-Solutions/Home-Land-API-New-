using Domain.UserAuth;

namespace Application.Interfaces
{

    public interface IJwtGenerator
    {
        string CreateToken(AppUser user,string role);
    }

}