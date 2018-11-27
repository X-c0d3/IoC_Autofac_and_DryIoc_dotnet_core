using System;
using System.Collections.Generic;
using System.Text;

namespace Ioc.Repository.Repositories.Models.DTO {
    public class CountryDto {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public ICollection<City> City { get; set; }
        public ICollection<Hotel> Hotel { get; set; }
    }
}
