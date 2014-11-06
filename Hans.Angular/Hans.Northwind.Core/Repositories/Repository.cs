using Hans.Northwind.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hans.Northwind.Core.Repositories
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

        public void SaveAsync(TModel instance)
        {
            Context.Set<TModel>().Add(instance);
            Context.SaveChangesAsync();
        }

        public void UpdateAsync(TModel instance)
        {
            Context.Set<TModel>().Attach(instance);
            Context.Entry(instance).State = System.Data.Entity.EntityState.Modified;
            Context.SaveChangesAsync();
        }

        public void DeleteAsync(TModel instance)
        {
            Context.Set<TModel>().Remove(instance);
            Context.SaveChangesAsync();
        }

        public IList<TModel> FindAll()
        {
            return Context.Set<TModel>().ToList();
        }

        public IList<TModel> FindAllBy(System.Linq.Expressions.Expression<Func<TModel, bool>> where)
        {
            return Context.Set<TModel>().Where(where.Compile()).ToList();
        }

        public TModel FindOneBy(System.Linq.Expressions.Expression<Func<TModel, bool>> where)
        {
            return Context.Set<TModel>().FirstOrDefault(where.Compile());
        }

        public Task<IList<TModel>> FindAllAsync()
        {
            return Task.Run<IList<TModel>>(() =>
            {
                return Context.Set<TModel>().AsParallel().ToList();
            });
        }

        public Task<IList<TModel>> FindAllByAsync(System.Linq.Expressions.Expression<Func<TModel, bool>> where)
        {
            return Task.Run<IList<TModel>>(() =>
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
