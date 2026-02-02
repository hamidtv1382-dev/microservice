using Messaging_Service.src._01_Domain.Core.Common;
using Messaging_Service.src._01_Domain.Core.Interfaces.Repositories;
using Messaging_Service.src._03_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Messaging_Service.src._03_Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly MessagingDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(MessagingDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(TEntity entity)
        {
            entity.SetAsDeleted();
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        // نیازی به بازنویسی SaveChangesAsync نیست، چون کار مستقیماً با UnitOfWork انجام می‌شود
    }
}
