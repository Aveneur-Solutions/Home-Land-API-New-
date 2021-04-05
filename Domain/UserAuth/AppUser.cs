using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain.UserAuth
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public string OTP { get; set; }

        public static implicit operator AppUser(Task<AppUser> v)
        {
            throw new NotImplementedException();
        }
    }
}