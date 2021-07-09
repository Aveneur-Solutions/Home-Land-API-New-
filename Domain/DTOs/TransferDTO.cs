using System;

namespace Domain.DTOs
{
    public class TransferDTO
    {
        public string FlatId { get; set; }
        public string TransferredFrom  { get; set; }
        public string TransferredTo { get; set; }
        public DateTime TransferDate { get; set; }
    }
}