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
        public DbSet<Unit> Units { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<TransferredUnit> TransferredUnits { get; set; }
        public DbSet<UnitImage> UnitImages { get; set; }
        public DbSet<Log> ActivityLogs { get; set; }
    }
}