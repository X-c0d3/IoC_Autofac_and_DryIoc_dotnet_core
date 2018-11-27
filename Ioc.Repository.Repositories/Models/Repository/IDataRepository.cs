using System.Collections.Generic;

namespace Ioc.Repository.Repositories.Models.Repository
{
    public interface IDataRepository<TEntity, TDto>
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get(long id);
        TDto GetDto(long id);
        void Add(TEntity entity);
        void Update(TEntity entityToUpdate, TEntity entity);
        void Delete(TEntity entity);
    }
}