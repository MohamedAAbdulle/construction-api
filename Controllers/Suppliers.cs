using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConstructionApi.Contracts;
using ConstructionApi.Data;
using ConstructionApi.Enums;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ConstructionApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Suppliers : ControllerBase
    {
        private readonly DataDbContext _context;

        public Suppliers(DataDbContext context)
        {
            _context = context;

        }

        //test
        [HttpGet("Quotes")]
        public IActionResult GetAllQuotes()
        {
            var quotes = _context.SupplierInventories.ToList();
            return Ok(quotes);
        }

        [HttpGet]
        public IActionResult GetSuppliers([FromHeader] int customerId)
        {
            var suppliers = _context.Supplier.Where(r => r.CustomerId == customerId).ToList();
            return Ok(suppliers);
        }

        [HttpGet("{id}/Quotes")]
        public IActionResult GetQuotes(int id)
        {
            var quotes = _context.SupplierInventories.Where(q=>q.SupplierId==id).ToList();
            return Ok(quotes);
        }

        [HttpPost]
        public IActionResult Post([FromBody] SupplierDb supplier, [FromHeader] int customerId)
        {

            _context.Supplier.Add(new SupplierDb()
            {
                Name = supplier.Name,
                Phone = supplier.Phone,
                Address = supplier.Address,
                Email = supplier.Email,
                CustomerId = customerId
            });


            _context.SaveChanges();

            return Ok("Supplier Created Successfully");
        }


        [HttpPut("{id}")]
        public IActionResult Put([FromBody] EditSupplierAndQuotes supplierObj, int id)
        {
            var foundSupplier = _context.Supplier.FirstOrDefault(i => i.Id == id);

            if (foundSupplier == null)
            {
                return NotFound();
            }

            var supplier = supplierObj.Supplier;

            foundSupplier.Name = supplier.Name;
            foundSupplier.Email = supplier.Email;
            foundSupplier.Phone = supplier.Phone;
            foundSupplier.Address = supplier.Address;

            var quotesFromDb = _context.SupplierInventories.Where(q => q.SupplierId == id).ToList();

            var quotesFromBody = supplierObj.Quotes;

            quotesFromBody.ForEach(q =>
            {
                if (q.Status == QuoteStatus.Created) {
                    _context.SupplierInventories.Add(new QuoteDb() {
                        InventoryId = q.InventoryId,
                        SupplierId = id,
                        Amount = q.Amount,
                        Price = q.Price
                    });
                }

                else{
                    var foundQuote = quotesFromDb.FirstOrDefault(qt => qt.Id == q.Id);
                    if (foundQuote!=null)
                    {
                        if (q.Status == QuoteStatus.Deleted)
                        {
                            _context.SupplierInventories.Remove(foundQuote);
                        }
                        else if (q.Status == QuoteStatus.Modified)
                        {
                            foundQuote.InventoryId = q.InventoryId;
                            foundQuote.SupplierId = id;
                            foundQuote.Amount = q.Amount;
                            foundQuote.Price = q.Price;
                        }
                    }
                }
            });

            _context.SaveChanges();
            return Ok("Supplier Updated Successfully");

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var foundSupplier = _context.Supplier.FirstOrDefault(i => i.Id == id);

            if (foundSupplier == null)
            {
                return NotFound();
            }
            _context.Supplier.Remove(foundSupplier);
            _context.SaveChanges();
            return Ok("Supplier Deleted Successfully");
        }
    }
}
