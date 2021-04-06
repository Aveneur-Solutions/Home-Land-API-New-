using System;
using System.Threading.Tasks;

namespace Domain.UnitBooking
{
    public class Flat
    {
        public string Id { get; set; }
        public int Size { get; set; }
        public double Price { get; set; }
        public int Level { get; set; }
        public int BuildingNumber { get; set; }
        public int NoOfBedrooms { get; set; }
        public int NoOfBaths { get; set; }
        public int NoOfBalconies { get; set; }
        public bool IsFeatured { get; set; }
        public double BookingPrice { get; set; }
        public bool IsBooked { get; set; }
        public bool IsSold { get; set; }
        public int DownPaymentDays { get; set; }

        public static implicit operator Flat(ValueTask<Flat> v)
        {
            throw new NotImplementedException();
        }
    }
}