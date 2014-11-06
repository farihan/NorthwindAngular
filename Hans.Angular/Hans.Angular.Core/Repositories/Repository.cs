using Hans.Angular.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hans.Angular.Core.Repositories
{
    public class Repository<TModel> : IRepository<TModel> where TModel : class
    {
        private NORTHWNDContext Context { get; set; }

        public Repository()
        {
            this.Context = new NORTHWNDContext();
        }

        public void Save(TModel instance)
        {
            Context.Set<TModel>().Add(instance);
            Context.SaveChanges();
        }

        public void Update(TModel instance)
        {
            Context.Set<TModel>().Attach(instance);
            Context.Entry(instance).State = System.Data.Entity.EntityState.Modified;
            Context.SaveChanges();
        }

        public void Delete(TModel instance)
        {
            Context.Set<TModel>().Remove(instance);
            Context.SaveChanges();
        }

        public async Task SaveAsync(TModel instance)
        {
            Context.Set<TModel>().Add(instance);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TModel instance)
        {
            Context.Set<TModel>().Attach(instance);
            Context.Entry(instance).State = System.Data.Entity.EntityState.Modified;
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TModel instance)
        {
            Context.Set<TModel>().Remove(instance);
            await Context.SaveChangesAsync();
        }

        public IQueryable<TModel> FindAll()
        {
            return Context.Set<TModel>();
        }

        public IQueryable<TModel> FindAllBy(System.Linq.Expressions.Expression<Func<TModel, bool>> where)
        {
            return Context.Set<TModel>().Where(where.Compile()).AsQueryable();
        }

        public TModel FindOneBy(System.Linq.Expressions.Expression<Func<TModel, bool>> where)
        {
            return Context.Set<TModel>().FirstOrDefault(where.Compile());
        }

        public Task<IQueryable<TModel>> FindAllAsync()
        {
            return Task.Run<IQueryable<TModel>>(() =>
            {
                return Context.Set<TModel>().AsParallel().AsQueryable();
            });
        }

        public Task<IQueryable<TModel>> FindAllByAsync(System.Linq.Expressions.Expression<Func<TModel, bool>> where)
        {
            return Task.Run<IQueryable<TModel>>(() =>
            {
                return FindAllBy(where);
            });
        }

        public Task<TModel> FindOneByAsync(System.Linq.Expressions.Expression<Func<TModel, bool>> where)
        {
            return Task.Run<TModel>(() =>
            {
                return FindOneBy(where);
            });
        }
    }
}
