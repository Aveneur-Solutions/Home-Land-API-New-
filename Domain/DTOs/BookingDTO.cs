using System;

namespace Domain.DTOs
{
    public class BookingDTO
    {
        public string FlatId { get; set; }
        public string CustomerName { get; set; }
        public DateTime DateBooked { get; set; }
    }
}