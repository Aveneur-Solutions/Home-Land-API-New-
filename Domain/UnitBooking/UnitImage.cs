using System;

namespace Domain.UnitBooking
{
    public class UnitImage
    {
          public Guid Id { get; set; }
        public Guid UnitId { get; set; }
        public Unit Unit { get; set; }
        public string ImageLocation { get; set; }
    }
}