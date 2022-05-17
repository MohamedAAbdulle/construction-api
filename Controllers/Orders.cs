using Amazon.S3;
using ConstructionApi.Contracts;
using ConstructionApi.Data;
using ConstructionApi.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Orders : ControllerBase
    {
        private readonly DataDbContext _context;
        private readonly IAmazonS3 _s3Client;

        public Orders(DataDbContext context, IAmazonS3 s3Client)
        {
            _context = context;
            _s3Client = s3Client;

        }
        [HttpGet("/all-documents")]
        public IActionResult GetAllDocs()
        {
            var docs = _context.Document.ToList();
            return Ok(docs);
        }
        
        [HttpGet]
        public IActionResult GetOrders([FromHeader] int customerId)
        {
            var orders = _context.Order.Where(r => r.CustomerId == customerId).ToList();
            return Ok(orders);
        }
        [HttpGet("{ownerId}")]
        public IActionResult GetOrderById([FromHeader] int customerId,int ownerId)
        {
            var docs = _context.Document.Where(r => r.OwnerId== ownerId && r.FileCategory==DocumentCategory.Order&&r.CustomerId == customerId).ToList();
            return Ok(docs);
        }

        [HttpPost]
        public IActionResult Post([FromBody] OrderDb order, [FromHeader] int customerId)
        {
            _context.Order.Add(new OrderDb()
            {
                InventoryId=order.InventoryId,
                SupplierId=order.SupplierId,
                Price=order.Price,
                Quantity=order.Quantity,
                Delivered=order.Delivered,
                DateDone=order.DateDone,
                Status=order.Status,
                CustomerId = customerId
            });

            if ((int)order.Status > 1) {
                var inv=_context.Inventory.FirstOrDefault(i => i.Id == order.InventoryId);
                if (inv!=null) {
                    inv.Quantity += order.Quantity;
                    _context.InventoryHistory.Add(new InventoryHistoryDb()
                    {
                        InvId = order.InventoryId,
                        Quantity = order.Quantity,
                        Type = (InvHistType)2,
                        DateDone = order.DateDone
                    });
                }

            }

            Console.WriteLine((int) order.Status>1);

            _context.SaveChanges();
            return Ok("Order Created Successfully");
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromForm] List<IFormFile> files, [FromForm] string orderString, [FromHeader] int customerId, int id)
        {
            OrderObj order = JsonConvert.DeserializeObject<OrderObj>(orderString);

            //var rr = new Documents(_s3Client);

            //var a = await rr.Bb(file, obj.FileName);

            var foundOrder = _context.Order.FirstOrDefault(i => i.Id == id);

            if (foundOrder == null)
            {
                return NotFound();
            }

            if ((int)order.Status > 1 && (int) foundOrder.Status==1)
            {
                var inv = _context.Inventory.FirstOrDefault(i => i.Id == order.InventoryId);
                if (inv != null)
                {
                    inv.Quantity += order.Quantity;
                    _context.InventoryHistory.Add(new InventoryHistoryDb()
                    {
                        InvId = order.InventoryId,
                        Quantity = order.Quantity,
                        Type = (InvHistType)2,
                        DateDone = order.DateDone
                    });

                }
            }

            foundOrder.InventoryId = order.InventoryId;
            foundOrder.SupplierId = order.SupplierId;
            foundOrder.Price = order.Price;
            foundOrder.Quantity = order.Quantity;
            foundOrder.Delivered = order.Delivered;
            foundOrder.DateDone = order.DateDone;
            foundOrder.Status = order.Status;



             var rr = new Documents(_s3Client);

            
            foreach(var d in order.Documents)
            {


                if (d.Status == QuoteStatus.Created)
                {
                    var ff=files.Find(f => f.FileName == d.FileName);
                    if (ff != null)
                    {
                        var a = await rr.CreateFile(ff,customerId);
                        if (a)
                        {
                            _context.Document.Add(new DocumentDb()
                            {
                                FileName = d.FileName,
                                FileType = d.FileType,
                                DateCreated = d.DateCreated,
                                OwnerId = id,
                                FileCategory = DocumentCategory.Order,
                                CustomerId = customerId,
                            });
                        }
                        else {  return BadRequest("Could not store file in s3"); }

                    }
                    else {  BadRequest("file name mismatch"); }  
                }

                if (d.Status == QuoteStatus.Deleted)
                {
                    //differeciate the s3 errors of notfound and found but couldn't deleted
                    var bucket = "construction001";
                    var key = $"{customerId}/orders/{d.FileName}";
                    var res = await rr.DeleteFile(bucket, key);
                    if (res)
                    {
                        var foundDoc = _context.Document.FirstOrDefault(i => i.Id == d.Id && i.CustomerId == customerId);
                        _context.Document.Remove(foundDoc);
                    }
                    else
                    {
                        return BadRequest("Failed To delete from s3");
                    }
                }
            
        };

            _context.SaveChanges();
            return Ok("Order Updated Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromHeader] int customerId)
        {
            var foundOrder = _context.Order.FirstOrDefault(i => i.Id == id&&i.CustomerId==customerId);

            if (foundOrder == null)
            {
                return NotFound();
            }
            _context.Order.Remove(foundOrder);
            _context.SaveChanges();
            return Ok("Order Deleted Successfully");
        }
        [HttpDelete("/documents-delete/{id}")]
        public async Task<IActionResult> DeleteDoc(int id, [FromHeader] int customerId)
        {
            var foundOrder = _context.Document.FirstOrDefault(i => i.Id == id && i.CustomerId == customerId);

            if (foundOrder == null)
            {
                return NotFound();
            }
            
                _context.Document.Remove(foundOrder);
                _context.SaveChanges();
                return Ok("Order Deleted Successfully");
            
        }
    }
}
