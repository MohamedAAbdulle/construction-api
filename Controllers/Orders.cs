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

        private readonly DateTime currentTime;

        public Orders(DataDbContext context, IAmazonS3 s3Client)
        {
            _context = context;
            _s3Client = s3Client;
            currentTime = DateTime.UtcNow;

        }
        
        [HttpGet("/all-documents")] //admin-level
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

        [HttpGet("{id}/docs")]
        public IActionResult GetOrderById([FromHeader] int customerId,int id)
        {
            var docs = _context.Document.Where(r => r.OwnerId== id && r.FileCategory==DocumentCategory.Order&&r.CustomerId == customerId).ToList();
            return Ok(docs);
        }

        [HttpPost]
        public IActionResult Post([FromBody] OrderObj order, [FromHeader] int customerId)
        {
            _context.Order.Add(new OrderDb()
            {
                InventoryId=order.InventoryId,
                SupplierId=order.SupplierId,
                Price=order.Price,
                Quantity=order.Quantity,
                Delivered=order.Delivered,
                DateDone= currentTime,
                Status= (OrderStatus)1 ,
                CustomerId = customerId,
            });

            

            _context.SaveChanges();
            return Ok("Order Created Successfully");
        }



        [HttpPut("{id}")]
        public IActionResult Put([FromBody] OrderObj order, int id)
        {

            var foundOrder = _context.Order.FirstOrDefault(i => i.Id == id);

            if (foundOrder == null)
            {
                return NotFound();
            }

            foundOrder.InventoryId = order.InventoryId;
            foundOrder.SupplierId = order.SupplierId;
            foundOrder.Price = order.Price;
            foundOrder.Quantity = order.Quantity;
            foundOrder.Delivered = order.Delivered;
            foundOrder.DateDone = currentTime;

            _context.SaveChanges();
            return Ok("Order Updated Successfully");
        }


        [HttpPut("statusUpdate/{id}")]
        public IActionResult UpdateStatus(int id, [FromHeader] int customerId, int userId)
        {
            

            var foundOrder = _context.Order.FirstOrDefault(i => i.Id == id);

            if (foundOrder == null)
            {
                return NotFound();
            }

            var prevStatus =(int)foundOrder.Status;

            if (prevStatus >= 3) {
                return BadRequest();
            }
            if (prevStatus == 1) {
                var inv = _context.Inventory.FirstOrDefault(i => i.Id == foundOrder.InventoryId);
                if (inv == null)
                {
                    return NotFound();
                }
                inv.Quantity += foundOrder.Quantity;
                _context.InventoryHistory.Add(new InventoryHistoryDb()
                {
                    InvId = foundOrder.InventoryId,
                    Quantity = foundOrder.Quantity,
                    Type = (InvHistType)2,
                    DateDone = currentTime,
                    CustomerId= customerId,
                    UserId= userId
                });
            }

            foundOrder.Status = (OrderStatus)prevStatus + 1;
            foundOrder.DateDone = currentTime;

            _context.SaveChanges();
            return Ok();
        }


        [HttpPut("undoStatus/{id}")]
        public IActionResult UndoStatus(int id, [FromHeader] int customerId, int userId)
        {

            var foundOrder = _context.Order.FirstOrDefault(i => i.Id == id);

            if (foundOrder == null)
            {
                return NotFound();
            }

            var prevStatus = (int)foundOrder.Status;

            if (prevStatus <= 1)
            {
                return BadRequest();
            }
            if (prevStatus == 2)
            {
                var inv = _context.Inventory.FirstOrDefault(i => i.Id == foundOrder.InventoryId);
                if (inv == null)
                {
                    return NotFound();
                }
                inv.Quantity -= foundOrder.Quantity;
                _context.InventoryHistory.Add(new InventoryHistoryDb()
                {
                    InvId = foundOrder.InventoryId,
                    Quantity = foundOrder.Quantity,
                    Type = (InvHistType)5,
                    DateDone = currentTime,
                    CustomerId= customerId,
                    UserId=userId
                   
                });
            }

            foundOrder.Status = (OrderStatus)prevStatus - 1;
            foundOrder.DateDone = currentTime;

            _context.SaveChanges();
            return Ok();
        }



        //[HttpPut("docs/{id}")]
        //public async Task<IActionResult> UpdateDocs([FromForm] List<IFormFile> files, [FromForm] string docsJson, [FromHeader] int customerId, int id)
        //{
        //    var docObjs = JsonConvert.DeserializeObject<List<DocumentObj>>(docsJson);

        //    //var bb= List < DocumentObj >

        //    var rr = new Documents(_s3Client, _context); 
            
        //    foreach (var d in docObjs)
        //    {

        //        if (d.Status == EditedAction.Created)
        //        {
        //            var ff = files.Find(f => f.FileName == d.FileName);
        //            if (ff != null)
        //            {
        //                var a = await rr.CreateFile(ff, customerId);
        //                if (a)
        //                {
        //                    _context.Document.Add(new DocumentDb()
        //                    {
        //                        FileName = d.FileName,
        //                        FileType = d.FileType,
        //                        DateCreated = d.DateCreated,
        //                        OwnerId = id,
        //                        FileCategory = DocumentCategory.Order,
        //                        CustomerId = customerId,
        //                    });
        //                }
        //                else { return BadRequest("Could not store file in s3"); }

        //            }
        //            else { BadRequest("file name mismatch"); }
        //        }

        //        if (d.Status == EditedAction.Deleted)
        //        {
        //            //differeciate the s3 errors of notfound and found but couldn't deleted
        //            var bucket = "construction001";
        //            var key = $"{customerId}/orders/{d.FileName}";
        //            var res = await rr.DeleteFile(bucket, key);
        //            if (res)
        //            {
        //                var foundDoc = _context.Document.FirstOrDefault(i => i.Id == d.Id && i.CustomerId == customerId);
        //                _context.Document.Remove(foundDoc);
        //            }
        //            else
        //            {
        //                return BadRequest("Failed To delete from s3");
        //            }
        //        }

        //    };

        //    _context.SaveChanges();
        //    return Ok("Order Updated Successfully");
        //}
        

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

    }
}
