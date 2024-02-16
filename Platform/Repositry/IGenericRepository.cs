using System.Linq.Expressions;

namespace Platform.Repositry
{
   public interface IGenericRepository<T> where T : class
   {
            T GetById(object id, params Expression<Func<T, object>>[] includes);
           IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes);
            void Insert(T entity);
            void Update(T entity);
            void Delete(object id);
   }
}
