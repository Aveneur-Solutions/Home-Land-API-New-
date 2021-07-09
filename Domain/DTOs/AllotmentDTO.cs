using System;

namespace Domain.DTOs
{
    public class AllotmentDTO
    {
        public string FlatId { get; set; }
        public string CustomerName { get; set; }
        public DateTime DateAlloted { get; set; }
    }
}