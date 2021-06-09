using System.Collections.Generic;
using Domain.UnitBooking;

namespace Domain.DTOs
{
    public class OrderDetailsDTO
    {
        public List<Flat> Flats { get; set; }
        public decimal Amount { get; set; }
        public int TotalUnits { get; set; }
    }
}