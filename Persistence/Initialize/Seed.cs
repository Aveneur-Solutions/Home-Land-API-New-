using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.UnitBooking;
using Domain.UserAuth;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Initialize
{
    public class Seed
    {
        public static async Task SeedData(HomelandContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            
            if (!roleManager.Roles.Any())
            {
                var roles = new List<IdentityRole>{
                    new IdentityRole{
                        Name = "Super Admin"
                    },
                    new IdentityRole{
                        Name = "Admin"
                    },
                    new IdentityRole{
                        Name = "User"
                    }
                };
                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);

                }
               
            }
            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser{
                        Id = "U2",
                        FirstName = "Zulker",
                        LastName = "Rahman Obaidul",
                        UserName = "PervySage",
                        PhoneNumber = "+8801717755244",
                        PhoneNumberConfirmed = true
                    },
                     new AppUser{
                         Id = "U3",
                        FirstName = "Wasif",
                        LastName = "M.Chowdhury",
                        UserName = "CEO",
                        PhoneNumber = "+8801716590911",
                        PhoneNumberConfirmed = true
                    },
                    new AppUser{
                        Id = "U4",
                        FirstName = "Umar",
                        LastName = "Faiaz Kuddus",
                        UserName = "Salman_Muqtadir",
                        PhoneNumber = "+8801625203488",
                        PhoneNumberConfirmed = true
                    },
                    new AppUser{
                        Id = "U5",
                        FirstName = "Ashikur",
                        LastName = "Rahman Kader",
                        UserName = "Pervy_Madara",
                        PhoneNumber = "+8801837440069",
                        PhoneNumberConfirmed = true
                    }
                };
                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "StrongP@ssw0rd");
                    await userManager.AddToRoleAsync(user,"User");       
                }
                 var adminUser = new AppUser
                {
                    Id = "U1",
                    FirstName = "Ragib",
                    LastName = "Ibne King",
                    UserName = "Insaiyan",
                    PhoneNumber = "+8801680800602",
                    PhoneNumberConfirmed = true
                };
             await userManager.CreateAsync(adminUser,"VeryStrongP@ss0wrd");   
             await userManager.AddToRoleAsync(adminUser,"Super Admin");

            }
            if (!context.Flats.Any())
            {
                var flats = new List<Flat>{
                    new Flat{
                    Id = "HL01",
                    Size = 1000,
                    Price = 20000000,
                    Level = 10,
                    BuildingNumber = 102,
                    NoOfBalconies = 3,
                    NoOfBaths =2,
                    NoOfBedrooms = 4,
                    DownPaymentDays = 100,
                    BookingPrice = 100000,
                    IsFeatured =false,
                    IsBooked = false,
                    IsSold = false
                   },
                       new Flat{
                    Id = "HL02",
                    Size = 1000,
                    Price = 20000000,
                    Level = 10,
                    BuildingNumber = 102,
                    NoOfBalconies = 3,
                    NoOfBaths =2,
                    NoOfBedrooms = 4,
                    DownPaymentDays = 100,
                    BookingPrice = 100000,
                    IsFeatured =false,
                    IsBooked = false,
                    IsSold = false
                   },
                    new Flat{
                    Id = "HL03",
                    Size = 1000,
                    Price = 20000000,
                    Level = 10,
                    BuildingNumber = 102,
                    NoOfBalconies = 3,
                    NoOfBaths =2,
                    NoOfBedrooms = 4,
                    DownPaymentDays = 100,
                    BookingPrice = 100000,
                    IsFeatured =false,
                    IsBooked = false,
                    IsSold = false
                   },
                       new Flat{
                    Id = "HL04",
                    Size = 1000,
                    Price = 20000000,
                    Level = 10,
                    BuildingNumber = 102,
                    NoOfBalconies = 3,
                    NoOfBaths =2,
                    NoOfBedrooms = 4,
                    DownPaymentDays = 100,
                    BookingPrice = 100000,
                    IsFeatured =false,
                    IsBooked = false,
                    IsSold = false
                   }
                 };

                await context.Flats.AddRangeAsync(flats);

                await context.SaveChangesAsync();
            }
        }
    }
}