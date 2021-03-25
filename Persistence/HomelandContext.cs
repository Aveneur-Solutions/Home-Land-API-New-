using Domain.ActivityTracking;
using Domain.UnitBooking;
using Domain.UserAuth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class HomelandContext : IdentityDbContext<AppUser>
    {
        public HomelandContext(DbContextOptions<HomelandContext> options) : base(options)
        {
        }
        public DbSet<Flat> Flats { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<TransferredFlat> TransferredFlats { get; set; }
        public DbSet<FlatImage> UnitImages { get; set; }
        public DbSet<Log> ActivityLogs { get; set; }
    }
}