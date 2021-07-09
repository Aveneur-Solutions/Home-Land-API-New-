using System;
using Domain.UserAuth;

namespace Domain.UnitBooking
{
    public class Booking
    {
        public Guid Id { get; set; }
        public AppUser User { get; set; }
        public string UserId { get; set; }
        public DateTime DateBooked { get; set; }
        public Flat Flat { get; set; }
        public string FlatId { get; set; }

    }
}