namespace Platform.Repositry
{
    public interface IUnitOfWork<T> where T : class
    {
        IGenericRepository<T> Entity { get; }
        void Complete();
    }
}
