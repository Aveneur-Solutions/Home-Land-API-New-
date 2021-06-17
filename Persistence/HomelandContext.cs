using Domain.ActivityTracking;
using Domain.Common;
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
        public DbSet<Transfer> TransferredFlats { get; set; }
        public DbSet<FlatImage> UnitImages { get; set; }
        public DbSet<Log> ActivityLogs { get; set; }
        public DbSet<AllotMent> AllotMents { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            builder.Entity<Flat>()
                   .HasMany<FlatImage>(x => x.Images)
                   .WithOne(x => x.Flat)
                   .HasForeignKey(x => x.FlatId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Flat>()
            .HasOne<OrderDetails>( x=> x.OrderDetails)
            .WithOne(x => x.Flat)
            .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<Order>()
                   .HasMany<OrderDetails>(x => x.OrderDetails)
                   .WithOne(x => x.Order)
                   .HasForeignKey(x => x.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Order>()
                   .Property(p => p.Amount)
                   .HasColumnType("decimal(18,4)");



        }
    }
}