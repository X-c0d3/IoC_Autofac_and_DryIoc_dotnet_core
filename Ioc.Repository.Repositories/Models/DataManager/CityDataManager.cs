using Ioc.Repository.Repositories.Models.DTO;
using Ioc.Repository.Repositories.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ioc.Repository.Repositories.Models.DataManager {
    public class CityDataManager : IDataRepository<City, CityDto> {
        readonly EF_DEMOContext _DEMOContext;
        public CityDataManager(EF_DEMOContext eF_DEMOContext) {
            _DEMOContext = eF_DEMOContext;
        }

        public void Add(City entity) {
            throw new NotImplementedException();
        }

        public void Delete(City entity) {
            throw new NotImplementedException();
        }

        public City Get(int id) {
            _DEMOContext.ChangeTracker.LazyLoadingEnabled = false;

            var city = _DEMOContext.City
                .SingleOrDefault(b => b.CityId == id);

            if (city == null) {
                return null;
            }

            //_DEMOContext.Entry(hotel)
            //    .Collection(b => b.City)
            //    .Load();

            _DEMOContext.Entry(city)
               .Collection(b => b.Hotel)
               .Load();


            //Many use : Collection
            return city;
        }

        public IEnumerable<City> GetAll() {
            throw new NotImplementedException();
        }

        public CityDto GetDto(int id) {
            throw new NotImplementedException();
        }

        public void Update(City entityToUpdate, City entity) {
            throw new NotImplementedException();
        }
    }
}
