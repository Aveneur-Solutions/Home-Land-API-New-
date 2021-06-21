using System.Collections.Generic;

namespace Domain.DTOs
{
    public class BuildingDTO
    {
         public string BuildingNumber { get; set; }
        public string Image { get; set; }
        public List<FlatDTO> Flats { get; set; }
    }
}