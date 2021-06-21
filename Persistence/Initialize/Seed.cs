using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.UnitBooking;
using Domain.UserAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
                        LastName = "Nien",
                        UserName = "PervySage",
                        PhoneNumber = "+8801717755244",
                        NID = "123456789",
                        Address ="Dhanmondi,Dhaka,Bangladesh",
                        PhoneNumberConfirmed = true
                    },
                     new AppUser{
                         Id = "U3",
                        FirstName = "Wasif",
                        LastName = "M.Chowdhury",
                        UserName = "CEO",
                        PhoneNumber = "+8801716590911",
                        NID = "123456789",
                        Address ="Dhanmondi,Dhaka,Bangladesh",
                        PhoneNumberConfirmed = true
                    },
                    new AppUser{
                        Id = "U4",
                        FirstName = "Umar",
                        LastName = "Faiaz Kuddus",
                        UserName = "Salman_Muqtadir",
                        PhoneNumber = "+8801625203488",
                        NID = "123456789",
                        Address ="Dhanmondi,Dhaka,Bangladesh",
                        PhoneNumberConfirmed = true
                    },
                    new AppUser{
                        Id = "U5",
                        FirstName = "Ashikur",
                        LastName = "Rahman Kader",
                        UserName = "Pervy_Madara",
                        NID = "123456789",
                        Address ="Dhanmondi,Dhaka,Bangladesh",
                        PhoneNumber = "+8801837440069",
                        PhoneNumberConfirmed = true
                    }
                };
                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "StrongP@ssw0rd");
                    await userManager.AddToRoleAsync(user, "User");
                }
                var adminUser = new AppUser
                {
                    Id = "U1",
                    FirstName = "Ragib",
                    LastName = "Ibne King",
                    UserName = "Insaiyan",
                    NID = "123456789",
                    Address = "Dhanmondi,Dhaka,Bangladesh",
                    PhoneNumber = "+8801680800602",
                    PhoneNumberConfirmed = true
                };
                await userManager.CreateAsync(adminUser, "StrongP@ssw0rd");
                await userManager.AddToRoleAsync(adminUser, "Super Admin");

            }
            if (!context.Buildings.Any())
            {
                var buildings = new List<Building>{
                     new Building{
                         BuildingNumber = "1",
                         Image = "\\Building\\Building.jpg"
                     },
                     new Building{
                         BuildingNumber = "2",
                         Image = "\\Building\\Building.jpg"
                     },
                     new Building{
                         BuildingNumber = "3",
                         Image = "\\Building\\Building.jpg"
                     },
                     new Building{
                         BuildingNumber = "4",
                         Image = "\\Building\\Building.jpg"
                     },
                     new Building{
                         BuildingNumber = "5",
                         Image = "\\Building\\Building.jpg"
                     },
                     new Building{
                         BuildingNumber = "6",
                         Image = "\\Building\\Building.jpg"
                     },
                     new Building{
                         BuildingNumber = "7",
                         Image = "\\Building\\Building.jpg"
                     },
                     new Building{
                         BuildingNumber = "8",
                         Image = "\\Building\\Building.jpg"
                     },
                     new Building{
                         BuildingNumber = "9",
                         Image = "\\Building\\Building.jpg"
                     },
                     new Building{
                         BuildingNumber = "10",
                         Image = "\\Building\\Building.jpg"
                     },
                     new Building{
                         BuildingNumber = "11",
                         Image = "\\Building\\Building.jpg"
                     },
                     new Building{
                         BuildingNumber = "12",
                         Image = "\\Building\\Building.jpg"
                     },
                     new Building{
                         BuildingNumber = "13",
                         Image = "\\Building\\Building.jpg"
                     },
                     new Building{
                         BuildingNumber = "14",
                         Image = "\\Building\\Building.jpg"
                     },
                     new Building{
                         BuildingNumber = "15",
                         Image = "\\Building\\Building.jpg"
                     },
                     new Building{
                         BuildingNumber = "16",
                         Image = "\\Building\\Building.jpg"
                     },
                     new Building{
                         BuildingNumber = "17",
                         Image = "\\Building\\Building.jpg"
                     },
                     new Building{
                         BuildingNumber = "18",
                         Image = "\\Building\\Building.jpg"
                     },
                     new Building{
                         BuildingNumber = "19",
                         Image = "\\Building\\Building.jpg"
                     },
                     new Building{
                         BuildingNumber = "20",
                         Image = "\\Building\\Building.jpg"
                     }
                };
                await context.Buildings.AddRangeAsync(buildings);
                await context.SaveChangesAsync();
            }
            if (!context.Flats.Any())
            {
                var building = await context.Buildings.FirstOrDefaultAsync();
                var flats = new List<Flat>{
                    new Flat{
                    Id = "HL01",
                    Size = 1000,
                    Price = 3300000,
                    Level = 10,
                    Building = building,
                    NoOfBalconies = 3,
                    NoOfBaths =2,
                    NoOfBedrooms = 4,
                    DownPaymentDays = 100,
                    BookingPrice = 100000,
                    IsFeatured =false,
                    IsBooked = false,
                    IsSold = false,
                    NetArea = 1000,
                    CommonArea = 200
                   },
                       new Flat{
                    Id = "HL02",
                    Size = 1000,
                    Price = 3300000,
                    Level = 10,
                    Building = building,
                    NoOfBalconies = 3,
                    NoOfBaths =2,
                    NoOfBedrooms = 4,
                    DownPaymentDays = 100,
                    BookingPrice = 100000,
                    IsFeatured =false,
                    IsBooked = false,
                    IsSold = false,
                     NetArea = 1000,
                    CommonArea = 200
                   },
                    new Flat{
                    Id = "HL03",
                    Size = 1000,
                    Price = 3300000,
                    Level = 10,
                    Building = building,
                    NoOfBalconies = 3,
                    NoOfBaths =2,
                    NoOfBedrooms = 4,
                    DownPaymentDays = 100,
                    BookingPrice = 100000,
                    IsFeatured =false,
                    IsBooked = false,
                    IsSold = false,
                     NetArea = 1000,
                    CommonArea = 200
                   },
                    new Flat{
                    Id = "HL04",
                    Size = 1000,
                    Price = 3300000,
                    Level = 10,
                    Building = building,
                    NoOfBalconies = 3,
                    NoOfBaths =2,
                    NoOfBedrooms = 4,
                    DownPaymentDays = 100,
                    BookingPrice = 100000,
                    IsFeatured =false,
                    IsBooked = false,
                    IsSold = false,
                    NetArea = 1000,
                    CommonArea = 200
                   },
                    new Flat{
                    Id = "HL05",
                    Size = 1000,
                    Price = 3300000,
                    Level = 10,
                    Building = building,
                    NoOfBalconies = 3,
                    NoOfBaths =2,
                    NoOfBedrooms = 4,
                    DownPaymentDays = 100,
                    BookingPrice = 100000,
                    IsFeatured =false,
                    IsBooked = false,
                    IsSold = false,
                    NetArea = 1000,
                    CommonArea = 200
                   },
                    new Flat{
                    Id = "HL06",
                    Size = 1000,
                    Price = 3300000,
                    Level = 10,
                    Building = building,
                    NoOfBalconies = 3,
                    NoOfBaths =2,
                    NoOfBedrooms = 4,
                    DownPaymentDays = 100,
                    BookingPrice = 100000,
                    IsFeatured =false,
                    IsBooked = false,
                    IsSold = false,
                     NetArea = 1000,
                    CommonArea = 200
                   },
                    new Flat{
                    Id = "HL07",
                    Size = 1000,
                    Price = 3300000,
                    Level = 10,
                     Building = building,
                    NoOfBalconies = 3,
                    NoOfBaths =2,
                    NoOfBedrooms = 4,
                    DownPaymentDays = 100,
                    BookingPrice = 100000,
                    IsFeatured =false,
                    IsBooked = false,
                    IsSold = false,
                     NetArea = 1000,
                    CommonArea = 200
                   },
                    new Flat{
                    Id = "HL08",
                    Size = 1000,
                    Price = 3300000,
                    Level = 10,
                     Building = building,
                    NoOfBalconies = 3,
                    NoOfBaths =2,
                    NoOfBedrooms = 4,
                    DownPaymentDays = 100,
                    BookingPrice = 100000,
                    IsFeatured =false,
                    IsBooked = false,
                    IsSold = false,
                    NetArea = 1000,
                    CommonArea = 200
                   },
                   new Flat{
                    Id = "HL09",
                    Size = 1000,
                    Price = 3300000,
                    Level = 10,
                     Building = building,
                    NoOfBalconies = 3,
                    NoOfBaths =2,
                    NoOfBedrooms = 4,
                    DownPaymentDays = 100,
                    BookingPrice = 100000,
                    IsFeatured =false,
                    IsBooked = false,
                    IsSold = false,
                    NetArea = 1000,
                    CommonArea = 200
                   },
                       new Flat{
                    Id = "HL10",
                    Size = 1000,
                    Price = 3300000,
                    Level = 10,
                     Building = building,
                    NoOfBalconies = 3,
                    NoOfBaths =2,
                    NoOfBedrooms = 4,
                    DownPaymentDays = 100,
                    BookingPrice = 100000,
                    IsFeatured =false,
                    IsBooked = false,
                    IsSold = false,
                     NetArea = 1000,
                    CommonArea = 200
                   },
                    new Flat{
                    Id = "HL11",
                    Size = 1000,
                    Price = 3300000,
                    Level = 10,
                     Building = building,
                    NoOfBalconies = 3,
                    NoOfBaths =2,
                    NoOfBedrooms = 4,
                    DownPaymentDays = 100,
                    BookingPrice = 100000,
                    IsFeatured =false,
                    IsBooked = false,
                    IsSold = false,
                     NetArea = 1000,
                    CommonArea = 200
                   },
                    new Flat{
                    Id = "HL12",
                    Size = 1000,
                    Price = 3300000,
                    Level = 10,
                     Building = building,
                    NoOfBalconies = 3,
                    NoOfBaths =2,
                    NoOfBedrooms = 4,
                    DownPaymentDays = 100,
                    BookingPrice = 100000,
                    IsFeatured =false,
                    IsBooked = false,
                    IsSold = false,
                    NetArea = 1000,
                    CommonArea = 200
                   },
                    new Flat{
                    Id = "HL13",
                    Size = 1000,
                    Price = 3300000,
                    Level = 10,
                     Building = building,
                    NoOfBalconies = 3,
                    NoOfBaths =2,
                    NoOfBedrooms = 4,
                    DownPaymentDays = 100,
                    BookingPrice = 100000,
                    IsFeatured =false,
                    IsBooked = false,
                    IsSold = false,
                    NetArea = 1000,
                    CommonArea = 200
                   },
                    new Flat{
                    Id = "HL14",
                    Size = 1000,
                    Price = 3300000,
                    Level = 10,
                     Building = building,
                    NoOfBalconies = 3,
                    NoOfBaths =2,
                    NoOfBedrooms = 4,
                    DownPaymentDays = 100,
                    BookingPrice = 100000,
                    IsFeatured =false,
                    IsBooked = false,
                    IsSold = false,
                     NetArea = 1000,
                    CommonArea = 200
                   },
                    new Flat{
                    Id = "HL15",
                    Size = 1000,
                    Price = 3300000,
                    Level = 10,
                     Building = building,
                    NoOfBalconies = 3,
                    NoOfBaths =2,
                    NoOfBedrooms = 4,
                    DownPaymentDays = 100,
                    BookingPrice = 100000,
                    IsFeatured =false,
                    IsBooked = false,
                    IsSold = false,
                     NetArea = 1000,
                    CommonArea = 200
                   },
                    new Flat{
                    Id = "HL16",
                    Size = 1000,
                    Price = 3300000,
                    Level = 10,
                     Building = building,
                    NoOfBalconies = 3,
                    NoOfBaths =2,
                    NoOfBedrooms = 4,
                    DownPaymentDays = 100,
                    BookingPrice = 100000,
                    IsFeatured =false,
                    IsBooked = false,
                    IsSold = false,
                    NetArea = 1000,
                    CommonArea = 200
                   }
                 };
                var flatImages = new List<FlatImage> { };
                foreach (var flat in flats)
                {
                    var flatImage = new FlatImage
                    {
                        Flat = flat,
                        ImageLocation = "\\Flat\\Flat1 - Copy.jpg"
                    };
                    flatImages.Add(flatImage);
                }
                await context.UnitImages.AddRangeAsync(flatImages);
                await context.Flats.AddRangeAsync(flats);
                await context.SaveChangesAsync();
            }
        }
    }
}