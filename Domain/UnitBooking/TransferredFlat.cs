using System;
using Domain.UserAuth;

namespace Domain.UnitBooking
{
    public class TransferredFlat
    {
           public Guid Id  { get; set; }
        public string FlatId  { get; set; }
        public Flat Flat { get; set; }
        public string TransmitterId { get; set; }
        public AppUser Transmitter { get; set; }

        public string RecieverId { get; set; }
        public AppUser Reciever { get; set; }
        public DateTime TransferDate { get; set; } 
    }
}