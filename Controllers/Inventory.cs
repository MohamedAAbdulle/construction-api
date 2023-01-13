using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Newtonsoft.Json;
using ConstructionApi.Contracts;
using ConstructionApi.Data;
using ConstructionApi.Enums;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ConstructionApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Inventory : ControllerBase
    {
        private readonly DataDbContext _context;
        private readonly DateTime currentTime;

        public Inventory(DataDbContext context)
        {
            _context = context;
            currentTime = DateTime.UtcNow;

        }

        //test
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var invList = new List<int> { 3,4,5,6};
            return Ok(invList);
        }
        

        //get inventory list
        [HttpGet]
        public IActionResult GetInventory([FromHeader] int customerId)
        {
            var aa = _context.Inventory.Where(r => r.CustomerId == customerId).ToList();
            return Ok(aa);
        }

        [HttpPost]
        public IActionResult Post([FromBody] InventoryObj inv, [FromHeader] int customerId)
        {
            Console.WriteLine(inv);
            _context.Inventory.Add(new InventoryDb() { 
                Id=0,
                Name=inv.Name,
                Unit= inv.Unit,
                Quantity = inv.Quantity,
                Threshold= inv.Threshold,
                ModifiedDate= currentTime,
                CustomerId = customerId
            });

            _context.SaveChanges();
            return Ok("Inventory Created Successfully");
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id,[FromBody] InventoryObj inv)
        {
            var foundInv = _context.Inventory.FirstOrDefault(i => i.Id == id);

            if (foundInv == null)
            {
                return NotFound();
            }

            

            foundInv.Name = inv.Name;
            foundInv.Unit = inv.Unit;
            foundInv.Quantity = inv.Quantity;
            foundInv.Threshold = inv.Threshold;
            foundInv.ModifiedDate = currentTime;

            _context.SaveChanges();
            return Ok("Inventory Updated Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromHeader] int customerId)
        {
            var inv=_context.Inventory.FirstOrDefault(i => i.Id == id);

            if (inv==null) { 
                return NotFound();
            }
            var aa = _context.InventoryHistory.Where(i => i.InvId == id&&i.CustomerId== customerId).ToList();
            aa.ForEach(q =>
            {
                _context.Remove(q);
            });
            _context.Inventory.Remove(inv);
            _context.SaveChanges();
            return Ok("Inventory Deleted Successfully");
        }

        public class Validations
        {
            public string ErrorMessage { get; set; }
            public string Key { get; set; }
        }

        public class OperationResult
        {
            private List<string> _errors;

            public List<Validations> Errors { get; set; }
            public OperationResult()
            {
                Errors = new List<Validations>();
            }

            public object data { get; set; }
        }

        [HttpPut("quantity/{id}")]
        public IActionResult UpdateQuantity(int id, [FromHeader] int customerId, int userId, [FromBody] InvQuantity inv)
        {
            var dd = new OperationResult();
            

            //dd.Error = "Quantity should not be greater than Amount";
            var foundInv = _context.Inventory.FirstOrDefault(i => i.Id == id);

            if (foundInv == null)
            {
                return NotFound();
    }


            int typeInt = (int)inv.Type;

            if (typeInt == 2 || typeInt == 4) { foundInv.Quantity += inv.Quantity; }

            else if (typeInt == 1|| typeInt== 3){ 
                var remaining= foundInv.Quantity - inv.Quantity;
                if (remaining < 0) {
                    var error = new Validations { ErrorMessage = "Hello world", Key = "Quantity" };
                    //return new OperationResult { Errors = new List<Validations> { error} };
                    dd.Errors.Add(error);
                    return BadRequest(dd);
                }
                else { foundInv.Quantity = remaining; }
            }

            foundInv.ModifiedDate = currentTime;

            //record it to inventory history
            _context.InventoryHistory.Add(new InventoryHistoryDb()
            {
                InvId = id,
                Quantity = inv.Quantity,
                Type = inv.Type,
                DateDone = currentTime,
                CustomerId = customerId,
                UserId= userId,
            });

            _context.SaveChanges();
            return Ok("Inventory Quantity Updated Successfully");
        }

        //Inventory History
        [HttpGet("invhistory/{id}")]
        public IActionResult GetInvHistory(int id)
        {
            var aa = _context.InventoryHistory.Where(x => x.InvId == id).ToList();
            return Ok(aa);
        }



    }
}
