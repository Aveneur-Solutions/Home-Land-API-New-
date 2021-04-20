using System.Collections.Generic;
using Domain.UnitBooking;

namespace Domain.DTOs
{
    public class FlatDTO
    {
        public string Id { get; set; }
        public int Size { get; set; }
        public double Price { get; set; }
        public int Level { get; set; }
        public int BuildingNumber { get; set; }
        public int NoOfBedrooms { get; set; }
        public int NoOfBaths { get; set; }
        public int NoOfBalconies { get; set; }
        public double BookingPrice { get; set; }
        public bool IsBooked { get; set; }
        public int DownPaymentDays { get; set; }
        public double NetArea { get; set; }
        public double CommonArea { get; set; } 
        public ICollection<ImageDTO> Images { get; set; }
    }
}