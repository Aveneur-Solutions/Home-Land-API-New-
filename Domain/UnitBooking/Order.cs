using System;
using System.Collections.Generic;
using Domain.UserAuth;

namespace Domain.UnitBooking
{
    public class Order
    {
        public string Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public AppUser User { get; set; }
        public string UserId { get; set; }
        public bool PaymentConfirmed { get; set; }
        public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}