using Microsoft.EntityFrameworkCore;
using SmartSaccos.ApplicationCore.Interfaces;
using SmartSaccos.Domains.Entities;
using SmartSaccos.persistence.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SmartSaccos.persistence.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly SmartSaccosContext smartSaccosContext;
        public Repository(SmartSaccosContext context)
        {
            smartSaccosContext = context;
        }
        public void Add(T entity)
        {
            smartSaccosContext.Set<T>().Add(entity);
        }
        public void AddRange(List<T> entities)
        {
            smartSaccosContext.Set<T>().AddRange(entities);
        }

        public async Task<bool> Any(Expression<Func<T, bool>> predicate)
        {
            return await smartSaccosContext.Set<T>().AnyAsync(predicate);
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public void Delete(T entity)
        {
            smartSaccosContext.Set<T>().Remove(entity);
        }

        public void SoftDelete(T entity)
        {
            entity.IsDeleted = true;
            smartSaccosContext.Entry(smartSaccosContext.Set<T>()
                .Find(entity.Id)).CurrentValues.SetValues(entity);
        }

        public void DeleteRange(List<T> entities)
        {
            smartSaccosContext.Set<T>().RemoveRange(entities);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return smartSaccosContext.Set<T>()
                .Where(predicate);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await smartSaccosContext.Set<T>().FindAsync(id);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await smartSaccosContext.Set<T>()
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<List<T>> ListAllAsync()
        {
            return await smartSaccosContext.Set<T>().ToListAsync();
        }

        public async Task<List<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public int SaveChanges()
        {
            UpdateSoftDeleteStatuses();
            return smartSaccosContext.SaveChanges();
        }

        public int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            //UpdateSoftDeleteStatuses();
            return smartSaccosContext.SaveChanges(acceptAllChangesOnSuccess);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //UpdateSoftDeleteStatuses();
            return await smartSaccosContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            //UpdateSoftDeleteStatuses();
            return await smartSaccosContext.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateSoftDeleteStatuses()
        {
            foreach (var entry in smartSaccosContext.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["isDeleted"] = false;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["isDeleted"] = true;
                        break;
                }
            }
        }

        public void Update(T entity)
        {
            entity.UpdateCode += 1;
            smartSaccosContext.Entry(smartSaccosContext.Set<T>()
                .Find(entity.Id)).CurrentValues.SetValues(entity);
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(smartSaccosContext.Set<T>().AsQueryable(), spec);
        }
    }
}
