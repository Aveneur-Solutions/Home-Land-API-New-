using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.UserAuth;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Initialize
{
    public class Seed
    {
        public static async Task SeedData(HomelandContext context , UserManager<AppUser> userManager)
        {
              if (!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser{
                        FirstName = "Zulker",
                        LastName = "Nien",
                        UserName = "PervySage",
                        PhoneNumber = "+8801717755244",
                        PhoneNumberConfirmed = true
                    },
                     new AppUser{
                        FirstName = "Wasif",
                        LastName = "M.Chowdhury",
                        UserName = "CEO",
                        PhoneNumber = "+8801716590911",
                        PhoneNumberConfirmed = true
                    },
                     new AppUser{                      
                        FirstName = "Ragib",
                        LastName = "Ibne Hossain",
                        UserName = "Insaiyan",
                        PhoneNumber = "+8801680800602",
                        PhoneNumberConfirmed = true
                    }
                    ,
                         new AppUser{                      
                        FirstName = "Umar",
                        LastName = "Faiaz Moon",
                        UserName = "Salman_Muqtadir",
                        PhoneNumber = "+8801625203488",
                        PhoneNumberConfirmed = true
                    }


                };
                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "StrongP@ssw0rd");
                }
            }
        }
    }
}