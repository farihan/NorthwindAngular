using Hans.Angular.Core.Models;
using Hans.Angular.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Hans.Angular.Web.Controllers
{
    public class ProductController : ApiController
    {
        private IRepository<Product> ProductRepository = new Repository<Product>();

        // GET: api/Product
        public IQueryable<Product> GetProductList()
        {
            return ProductRepository.FindAll();
        }

        // GET: api/Product
        public int GetProductListSize()
        {
            return ProductRepository.FindAll().Count();
        }

        // GET: api/Product
        public IQueryable<Product> GetProductList(int page, int pageSize)
        {
            return ProductRepository.FindAll().Skip(page * pageSize).Take(pageSize);
        }

        // GET: api/Product/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> GetProduct(int id)
        {
            var product = await ProductRepository.FindOneByAsync(x => x.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/Product
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> CreateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await ProductRepository.SaveAsync(product);

            return CreatedAtRoute("DefaultApi", new { id = product.ProductID }, product);
        }

        // DELETE: api/Product/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            var product = ProductRepository.FindOneBy(x => x.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            await ProductRepository.DeleteAsync(product);

            return Ok(product);
        }

        // PUT: api/Product/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> EditProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductID)
            {
                return BadRequest();
            }

            try
            {
                await ProductRepository.UpdateAsync(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                var productToEdit = ProductRepository.FindOneBy(x => x.ProductID == id);
                if (productToEdit != null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
