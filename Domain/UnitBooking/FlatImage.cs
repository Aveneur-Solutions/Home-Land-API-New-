using System;

namespace Domain.UnitBooking
{
    public class FlatImage
    {
        public Guid Id { get; set; }
        public string FlatId { get; set; }
        public Flat Flat { get; set; }
        public string ImageLocation { get; set; }
    }
}