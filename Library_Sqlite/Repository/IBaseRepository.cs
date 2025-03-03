namespace Library.Repository
{
    public interface IBaseRepository<TEntity>
    {
        Task Save();
        IEnumerable<TEntity> Search(Func<TEntity, bool> filter);
    }
}

