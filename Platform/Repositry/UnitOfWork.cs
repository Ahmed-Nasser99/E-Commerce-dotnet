using Platform.Contexts;

namespace Platform.Repositry
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : class
    {

        private readonly AppDbContext _context;
        private IGenericRepository<T> _entity;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public IGenericRepository<T> Entity
        {
            get
            {
                return _entity ?? (_entity = new GenericRepository<T>(_context));
            }
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}
