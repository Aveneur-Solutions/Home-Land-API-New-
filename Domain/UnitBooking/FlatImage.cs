using System;

namespace Domain.UnitBooking
{
    public class FlatImage
    {
        public Guid Id { get; set; }
        public Guid FlatId { get; set; }
        public Flat Flat { get; set; }
        public string ImageLocation { get; set; }
    }
}