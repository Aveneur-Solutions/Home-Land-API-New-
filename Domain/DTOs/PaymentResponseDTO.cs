namespace Domain.DTOs
{
    public class PaymentResponseDTO
    {
        public string Status { get; set; }
        public string GatewayPageURL { get; set; }
        public string FailedReason { get; set; }
    }
}