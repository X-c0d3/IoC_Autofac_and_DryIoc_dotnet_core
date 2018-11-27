using System;
using System.Collections.Generic;
using System.Text;

namespace Ioc.Repository.Repositories.Models.DTO {
    public class CityDto {
        public int CityId { get; set; }
        public string CountryCode { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public Country CountryCodeNavigation { get; set; }
        public ICollection<Hotel> Hotel { get; set; }
    }
}
