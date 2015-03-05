using Hans.Angular.Core.Models;
using Hans.Angular.Core.Repositories;
using Hans.Northwind.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Hans.Angular.Web.Controllers
{
    public class CategoryController : ApiController
    {
        private IRepository<Category> CategoryRepository = new Repository<Category>();

        // GET: api/Category
        [System.Web.Http.HttpGet]
        public async Task<int> GetSize()
        {
            var categories = await CategoryRepository.FindAllAsync();

            return categories.Count();
        }

        // GET: api/Category
        [System.Web.Http.HttpGet]
        public async Task<IQueryable<CategoryModel>> GetAll()
        {
            var categories = await CategoryRepository.FindAllAsync();

            return categories.Select(x => new CategoryModel
            {
                CategoryID = x.CategoryID,
                CategoryName = x.CategoryName,
                Description = x.Description,
                Picture = x.Picture
            }).OrderBy(x => x.CategoryName);
        }
    }
}
