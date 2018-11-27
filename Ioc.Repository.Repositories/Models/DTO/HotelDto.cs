using System;
using System.Collections.Generic;
using System.Text;

namespace Ioc.Repository.Repositories.Models.DTO {
    public class HotelDto {
        public int HotelId { get; set; }
        public string CountryCode { get; set; }
        public int? CityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public City City { get; set; }
        public Country CountryCodeNavigation { get; set; }
    }
}
