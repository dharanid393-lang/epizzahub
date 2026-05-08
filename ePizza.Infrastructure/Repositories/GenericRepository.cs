using AutoMapper;
using AutoMapper.QueryableExtensions;
using ePizza.Domain.Interfaces;
using ePizza.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;


namespace ePizza.Infrastructure.Repositories
{
    public class GenericRepository<TDomain, TEntity> : IGenericRepository<TDomain>
        where TDomain : class
        where TEntity : class
    {

        protected readonly ePizzaDbContext _dbContext;
        protected readonly IMapper _mapper;

        public GenericRepository(ePizzaDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<TDomain>> GetAllAsync()
        {

            var dbResponse = _dbContext.Set<TEntity>();

            var response = await
                                dbResponse.ProjectTo<TDomain>(_mapper.ConfigurationProvider)
                                .ToListAsync();
            return response;
        }

        public async Task<TDomain> GetByIdAsync(object id)
        {
            var dbResponse = await _dbContext.Set<TEntity>().FindAsync(id);

            return dbResponse == null ? null : _mapper.Map<TDomain>(dbResponse);
        }

        public async Task AddAsync(TDomain domainEntity)
        {
            var entity = _mapper.Map<TEntity>(domainEntity);

            await _dbContext.Set<TEntity>().AddAsync(entity);
        }

        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(TDomain domainEntity, object id)
        {
            var existingEntity = await _dbContext.Set<TEntity>().FindAsync(id);
            if (existingEntity == null)
                throw new KeyNotFoundException($"Entity with id {id} not found.");

            _mapper.Map(domainEntity, existingEntity);
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id);

            if (entity == null)
                throw new KeyNotFoundException($"Entity with id {id} not found.");

            _dbContext.Set<TEntity>().Remove(entity);
        }

        public IQueryable<TDomain> AsQueryable()
        {
            return _dbContext.Set<TEntity>()
                .ProjectTo<TDomain>(_mapper.ConfigurationProvider);
        }

        public async Task<bool> ExistsAsync(object id)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id);
            return entity != null;
        }

    }
}
