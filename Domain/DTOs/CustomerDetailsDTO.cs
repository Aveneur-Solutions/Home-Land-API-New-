using System;
using System.Collections.Generic;

namespace Domain.DTOs
{
    public class CustomerDetailsDTO
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string NID { get; set; }
        public List<TransferDTO> Transfers { get; set; }
        public List<BookingDTO> Bookings { get; set; }
        public List<AllotmentDTO> Allotments { get; set; }
    }
}