namespace Platform.Repositry
{
   public interface IGenericRepository<T> where T : class
   {
            T GetById(object id);
            IEnumerable<T> GetAll();
            void Insert(T entity);
            void Update(T entity);
            void Delete(object id);
   }
}
