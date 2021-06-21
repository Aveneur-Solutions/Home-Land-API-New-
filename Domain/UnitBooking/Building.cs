using System;
using System.Collections.Generic;

namespace Domain.UnitBooking
{
    public class Building
    {
        public Guid Id { get; set; }
        public string BuildingNumber { get; set; }
        public string Image { get; set; }
        public ICollection<Flat> Flats { get; set; }
    }
}