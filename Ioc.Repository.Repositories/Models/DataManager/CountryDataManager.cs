using Ioc.Repository.Repositories.Models.DTO;
using Ioc.Repository.Repositories.Models.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ioc.Repository.Repositories.Models.DataManager {
    public class CountryDataManager : IDataRepository<Country, CountryDto> {
        public void Add(Country entity) {
            throw new NotImplementedException();
        }

        public void Delete(Country entity) {
            throw new NotImplementedException();
        }

        public Country Get(int id) {
            throw new NotImplementedException();
        }

        public IEnumerable<Country> GetAll() {
            throw new NotImplementedException();
        }

        public CountryDto GetDto(int id) {
            throw new NotImplementedException();
        }

        public void Update(Country entityToUpdate, Country entity) {
            throw new NotImplementedException();
        }
    }
}
