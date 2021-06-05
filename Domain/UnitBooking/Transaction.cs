using System;

namespace Domain.UnitBooking
{
    public class Transaction
    {
        public string tran_id { get; set; }
        public DateTime tran_date { get; set; }   
        public Order order { get; set; }
        public string order_id { get; set; }     
        public string val_id { get; set; }
        public decimal amount { get; set; }
        public decimal store_amount { get; set; }
        public string card_type { get; set; }
        public string card_no { get; set; }
        public string currency { get; set; }
        public string bank_tran_id { get; set; }
        public string card_issuer { get; set; }
        public string card_issuer_country { get; set; }
        public string card_issuer_country_code { get; set; }
        public string currency_type { get; set; }
        public string currency_amount { get; set; }
        public string value_a { get; set; }
        public string value_b { get; set; }
        public string value_c { get; set; }
        public string value_d { get; set; }
        public string verify_sign { get; set; }
        public string verify_key { get; set; }
        public string risk_level { get; set; }
        public string risk_title { get; set; }
    }
}