using Microsoft.EntityFrameworkCore;
using ProjFinal.Business.Interfaces.Repositories;
using ProjFinal.Business.Models.Entities;
using ProjFinal.Data.Context;
using System.Linq.Expressions;

namespace ProjFinal.Data.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        protected readonly AppDbContext Db;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(AppDbContext dbContext)
        {
            Db = dbContext; 
            DbSet = dbContext.Set<TEntity>();
        }
        public async Task<IEnumerable<TEntity>> BuscarAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }
        public virtual async Task<TEntity> ObterPorIdAsync(Guid id)
        {
            return await DbSet.FindAsync(id); 
        }

        public virtual async Task<List<TEntity>> ObterTodosAsync()
        {
            return await DbSet.ToListAsync(); 
        }

        public virtual async Task AdicionarAsync(TEntity entity)
        {
            DbSet.Add(entity);
            await SaveChangesAsync();
        }
        public virtual async Task AtualizarAsync(TEntity entity)
        {
            DbSet.Update(entity);
            await SaveChangesAsync(); 
        }
        public virtual async Task RemoverAsync(Guid id)
        {
            DbSet.Remove(new TEntity { Id = id });
            await SaveChangesAsync(); 
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Db.SaveChangesAsync(); 
        }
        public async void Dispose()
        {
            Db.Dispose();
        }
    }
}
