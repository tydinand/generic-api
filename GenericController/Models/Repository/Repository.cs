using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericController.API.Models.Repositories
{
    //ref: https://chathuranga94.medium.com/generic-repository-pattern-for-asp-net-core-9e3e230e20cb
    //
    public interface IRepository<T> : IDisposable
    {
        Task<T> GetById(int id);
        Task<T> GetByIdInclude(int id, Func<IQueryable<T>, IQueryable<T>> func);
        Task<List<T>> GetAll(Func<IQueryable<T>, IQueryable<T>> func);
        Task<List<T>> GetAllInclude(Func<IQueryable<T>, IQueryable<T>> func);
        Task<bool> Save(T model, EntityState entityState);
        Task<bool> Remove(T entity);
    }

    public class Repository<T> : IRepository<T> where T : ApplicationEntity
    {
        private ApplicationDbContext context;

        public Repository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async virtual Task<T> GetById(int id)
        {
            return await GetByIdInclude(id, x => x.Where(p => p.Id > 0));
        }

        ////ref: https://stackoverflow.com/questions/37463226/using-repository-pattern-to-eager-load-entities-using-theniclude
        /////example: var xx = xrepository.GetByIdAsync<Table>(10, x => x.Include(p => p.WorkArea), true);
        public async virtual Task<T> GetByIdInclude(int id, Func<IQueryable<T>, IQueryable<T>> func)
        {
            return (await GetAllInclude(func)).FirstOrDefault(e => e.Id == id);
        }

        public async virtual Task<List<T>> GetAll(Func<IQueryable<T>, IQueryable<T>> func)
        {
            return await GetAllInclude(x => x.Where(p => p.Id > 0));

        }
        public async virtual Task<List<T>> GetAllInclude(Func<IQueryable<T>, IQueryable<T>> func)
        {
            DbSet<T> result = context.Set<T>();
            IQueryable<T> resultWithEagerLoading = func(result);
            
            return await resultWithEagerLoading.ToListAsync();
        }

        public async Task<bool> Save(T entity, EntityState entityState)
        {
            context.Attach(entity).State = entityState;
            try
            {
                return await context.SaveChangesAsync() > 0;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var tt = ex;
                //logger
            }
            catch (Exception ex)
            {
                var hh = ex;
                //logger
            }
            return false;
        }

        public virtual async Task<bool> Remove(T entity)
        {
            try
            {
                var geT = context.Set<T>().Where(x => x.Id == entity.Id).SingleOrDefault();

                context.Set<T>().Remove(geT);
                return await context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
            }
            return false;
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (context != null)
                {
                    context.Dispose();
                    context = null;
                }
            }
        }
        #endregion
    }
}
