﻿using BHI.Northwind.Models;
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
    public class CustomerController : ApiController
    {
        private IRepository<Customer> CustomerRepository = new Repository<Customer>();

        // GET: api/Customer
        public int GetSize()
        {
            return CustomerRepository.FindAll().Count();
        }

        // GET: api/Customer
        public IQueryable<CustomerModel> GetAll()
        {
            return CustomerRepository.FindAll().Select(x => new CustomerModel
            {
                CustomerID = x.CustomerID,
                CompanyName = x.CompanyName,
                ContactName = x.ContactName,
                ContactTitle = x.ContactTitle,
                Address = x.Address,
                City = x.City,
                Region = x.Region,
                PostalCode = x.PostalCode,
                Country = x.Country,
                Phone = x.Phone,
                Fax = x.Fax
            });
        }

        // GET: api/Customer
        public IQueryable<CustomerModel> GetAllBy(int page, int pageSize, string sort = "customerid", bool asc = true)
        {
            var customers = CustomerRepository.FindAll().OrderBy(x => x.CustomerID);

            switch (sort.ToLower())
            {
                case "customerid":
                    customers = asc ? customers.OrderBy(p => p.CustomerID) : customers.OrderByDescending(p => p.CustomerID);
                    break;
                case "companyname":
                    customers = asc ? customers.OrderBy(p => p.CompanyName) : customers.OrderByDescending(p => p.CompanyName);
                    break;
                case "contactname":
                    customers = asc ? customers.OrderBy(p => p.ContactName) : customers.OrderByDescending(p => p.ContactName);
                    break;
                case "contacttitle":
                    customers = asc ? customers.OrderBy(p => p.ContactTitle) : customers.OrderByDescending(p => p.ContactTitle);
                    break;
                case "address":
                    customers = asc ? customers.OrderBy(p => p.Address) : customers.OrderByDescending(p => p.Address);
                    break;
                case "city":
                    customers = asc ? customers.OrderBy(p => p.City) : customers.OrderByDescending(p => p.City);
                    break;
                case "region":
                    customers = asc ? customers.OrderBy(p => p.Region) : customers.OrderByDescending(p => p.Region);
                    break;
                case "postalcode":
                    customers = asc ? customers.OrderBy(p => p.PostalCode) : customers.OrderByDescending(p => p.PostalCode);
                    break;
                case "country":
                    customers = asc ? customers.OrderBy(p => p.Country) : customers.OrderByDescending(p => p.Country);
                    break;
                case "phone":
                    customers = asc ? customers.OrderBy(p => p.Phone) : customers.OrderByDescending(p => p.Phone);
                    break;
                case "fax":
                    customers = asc ? customers.OrderBy(p => p.Fax) : customers.OrderByDescending(p => p.Fax);
                    break;
            }

            return customers.Select(x => new CustomerModel
            {
                CustomerID = x.CustomerID,
                CompanyName = x.CompanyName,
                ContactName = x.ContactName,
                ContactTitle = x.ContactTitle,
                Address = x.Address,
                City = x.City,
                Region = x.Region,
                PostalCode = x.PostalCode,
                Country = x.Country,
                Phone = x.Phone,
                Fax = x.Fax
            }).Skip(page * pageSize).Take(pageSize);
        }

        // GET: api/Customer/5
        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> Get(int id)
        {
            var customer = await CustomerRepository.FindOneByAsync(x => x.CustomerID == id.ToString());
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // POST: api/Customer
        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> Create(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = new Customer()
            {
                CompanyName = customer.CompanyName,
                ContactName = customer.ContactName,
                ContactTitle = customer.ContactTitle,
                Address = customer.Address,
                City = customer.City,
                Region = customer.Region,
                PostalCode = customer.PostalCode,
                Country = customer.Country,
                Phone = customer.Phone,
                Fax = customer.Fax
            };

            await CustomerRepository.SaveAsync(model);

            return CreatedAtRoute("DefaultApi", new { id = customer.CustomerID }, customer);
        }

        // DELETE: api/Customer/5
        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var customer = CustomerRepository.FindOneBy(x => x.CustomerID == id.ToString());
            if (customer == null)
            {
                return NotFound();
            }

            await CustomerRepository.DeleteAsync(customer);

            return Ok(customer);
        }

        // PUT: api/Customer/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Edit(int id, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id.ToString() != customer.CustomerID)
            {
                return BadRequest();
            }

            try
            {
                var model = CustomerRepository.FindOneBy(x => x.CustomerID == id.ToString());

                if (model != null)
                {
                    model.CompanyName = customer.CompanyName;
                    model.ContactName = customer.ContactName;
                    model.ContactTitle = customer.ContactTitle;
                    model.Address = customer.Address;
                    model.City = customer.City;
                    model.Region = customer.Region;
                    model.PostalCode = customer.PostalCode;
                    model.Country = customer.Country;
                    model.Phone = customer.Phone;
                    model.Fax = customer.Fax;
                }

                await CustomerRepository.UpdateAsync(customer);
            }
            catch (DbUpdateConcurrencyException)
            {
                var customerToEdit = CustomerRepository.FindOneBy(x => x.CustomerID == id.ToString());

                if (customerToEdit != null)
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