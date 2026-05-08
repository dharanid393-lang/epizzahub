namespace ePizza.Domain.Interfaces
{
    public interface IGenericRepository<TDomain> where TDomain : class
    {
        Task<IEnumerable<TDomain>> GetAllAsync();
        Task<TDomain> GetByIdAsync(object id);
        Task AddAsync(TDomain domainEntity);
        Task UpdateAsync(TDomain domainEntity, object id);
        Task DeleteAsync(object id);
        Task<bool> ExistsAsync(object id);
        Task<int> CommitAsync();
        IQueryable<TDomain> AsQueryable();
    }
}


