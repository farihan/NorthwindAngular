using Hans.Angular.Core.Models;
using Hans.Angular.Core.Repositories;
using Hans.Angular.Web.Models;
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
        public int GetSize()
        {
            return ProductRepository.FindAll().Count();
        }

        // GET: api/Product
        public IQueryable<ProductModel> GetAll()
        {
            return ProductRepository.FindAll().Select(x => new ProductModel
            {
                ProductID = x.ProductID,
                ProductName = x.ProductName,
                SupplierID = x.SupplierID.HasValue ? x.SupplierID.Value : 0,
                CategoryID = x.CategoryID.HasValue ? x.CategoryID.Value : 0,
                QuantityPerUnit = x.QuantityPerUnit,
                UnitPrice = x.UnitPrice.HasValue ? x.UnitPrice.Value : 0,
                UnitsInStock = x.UnitsInStock.HasValue ? x.UnitsInStock.Value : 0,
                UnitsOnOrder = x.UnitsOnOrder.HasValue ? x.UnitsOnOrder.Value : 0,
                ReorderLevel = x.ReorderLevel.HasValue ? x.ReorderLevel.Value : 0,
                Discontinued = x.Discontinued
            });
        }

        // GET: api/Product
        public IQueryable<ProductModel> GetAllBy(int page, int pageSize)
        {
            return ProductRepository.FindAll().Select(x => new ProductModel
            {
                ProductID = x.ProductID,
                ProductName = x.ProductName,
                SupplierID = x.SupplierID.HasValue ? x.SupplierID.Value : 0,
                CategoryID = x.CategoryID.HasValue ? x.CategoryID.Value : 0,
                QuantityPerUnit = x.QuantityPerUnit,
                UnitPrice = x.UnitPrice.HasValue ? x.UnitPrice.Value : 0,
                UnitsInStock = x.UnitsInStock.HasValue ? x.UnitsInStock.Value : 0,
                UnitsOnOrder = x.UnitsOnOrder.HasValue ? x.UnitsOnOrder.Value : 0,
                ReorderLevel = x.ReorderLevel.HasValue ? x.ReorderLevel.Value : 0,
                Discontinued = x.Discontinued
            }).Skip(page * pageSize).Take(pageSize);
        }

        // GET: api/Product/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> Get(int id)
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
        public async Task<IHttpActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = new Product()
            {
                ProductName = product.ProductName,
                SupplierID = product.SupplierID,
                CategoryID = product.CategoryID,
                QuantityPerUnit = product.QuantityPerUnit,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                UnitsOnOrder = product.UnitsOnOrder,
                ReorderLevel = product.ReorderLevel,
                Discontinued = product.Discontinued
            };

            await ProductRepository.SaveAsync(model);

            return CreatedAtRoute("DefaultApi", new { id = product.ProductID }, product);
        }

        // DELETE: api/Product/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> Delete(int id)
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
        public async Task<IHttpActionResult> Edit(int id, Product product)
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
                var model = ProductRepository.FindOneBy(x => x.ProductID == id);

                if (model != null)
                {
                    model.ProductName = product.ProductName;
                    model.SupplierID = product.SupplierID;
                    model.CategoryID = product.CategoryID;
                    model.QuantityPerUnit = product.QuantityPerUnit;
                    model.UnitPrice = product.UnitPrice;
                    model.UnitsInStock = product.UnitsInStock;
                    model.UnitsOnOrder = product.UnitsOnOrder;
                    model.ReorderLevel = product.ReorderLevel;
                    model.Discontinued = product.Discontinued;
                }

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
