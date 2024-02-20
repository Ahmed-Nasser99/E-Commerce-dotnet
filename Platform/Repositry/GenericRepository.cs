using Microsoft.EntityFrameworkCore;
using Platform.Contexts;
using System.Linq.Expressions;

namespace Platform.Repositry
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private DbSet<T> table = null;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            table = _context.Set<T>();
        }
        public void Delete(object id)
        {
            T existing = GetById(id);
            table.Remove(existing);
        }


        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = table;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.ToList();
        }

        public T GetById(object id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = table;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var entityParameter = Expression.Parameter(typeof(T));
            var idProperty = typeof(T).GetProperty("id");
            var idPropertyValue = Convert.ChangeType(id, idProperty.PropertyType);
            var whereExpression = Expression.Lambda<Func<T, bool>>(
                Expression.Equal(
                    Expression.Property(entityParameter, idProperty),
                    Expression.Constant(idPropertyValue)
                ),
                entityParameter
            );

            return query.FirstOrDefault(whereExpression);
        }

        public void Insert(T entity)
        {
            table.Add(entity);
        }

        public void Update(T entity)
        {
            table.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
