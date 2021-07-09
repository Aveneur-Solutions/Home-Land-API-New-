using System;

namespace Domain.UnitBooking
{
    public class OrderDetails
    {
        public Guid Id { get; set; }
        public string OrderId { get; set; }
        public Order Order { get; set; }
        public Flat Flat { get; set; }
        public string FlatId { get; set; }
    }
}