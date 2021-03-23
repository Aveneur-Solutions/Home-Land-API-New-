using System;
using Domain.UserAuth;

namespace Domain.UnitBooking
{
    public class TransferredUnit
    {
           public Guid Id  { get; set; }
        public int UnitId  { get; set; }
        public Unit Unit { get; set; }
        public string TransmitterId { get; set; }
        public AppUser Transmitter { get; set; }

        public string RecieverId { get; set; }
        public AppUser Reciever { get; set; }
        public DateTime TransferDate { get; set; } 
    }
}