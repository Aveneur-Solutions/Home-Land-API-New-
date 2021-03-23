using System;
using Domain.UserAuth;

namespace Domain.UnitBooking
{
    public class Booking
    {
             public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
        public DateTime DateBooked { get; set; }
        
    }
}