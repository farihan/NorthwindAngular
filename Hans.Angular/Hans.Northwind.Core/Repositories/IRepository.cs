using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hans.Northwind.Core.Repositories
{
    public interface IRepository<TModel> where TModel : class
    {
        void Save(TModel instance);
        void Update(TModel instance);
        void Delete(TModel instance);
        void SaveAsync(TModel instance);
        void UpdateAsync(TModel instance);
        void DeleteAsync(TModel instance);

        IList<TModel> FindAll();
        IList<TModel> FindAllBy(Expression<Func<TModel, bool>> where);
        TModel FindOneBy(Expression<Func<TModel, bool>> where);

        Task<IList<TModel>> FindAllAsync();
        Task<IList<TModel>> FindAllByAsync(Expression<Func<TModel, bool>> where);
        Task<TModel> FindOneByAsync(Expression<Func<TModel, bool>> where);
    }
}
