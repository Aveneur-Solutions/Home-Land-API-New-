using System;

namespace Domain.Common
{
    public class Image
    {
        public Guid Id { get; set; }
        public string ImageLocation { get; set; }      
        public string Section { get; set; }
    }
}