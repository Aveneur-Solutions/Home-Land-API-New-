namespace Domain.DTOs
{
    public class CustomerDTO
    {
        public string PhoneNumber { get; set; }
        public string Fullname { get; set; }
        public string NID { get; set; }
        public string Address { get; set; }
        public int NoOfFlatsBooked { get; set; }
        public int NoOfFlatsAlloted { get; set; }
        public string Role { get; set; }
        public string ProfileImage { get; set; }
     }
}