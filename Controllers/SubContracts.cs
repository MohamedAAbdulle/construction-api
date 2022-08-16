using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConstructionApi.Contracts;
using ConstructionApi.Data;
using ConstructionApi.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SubContracts : ControllerBase
    {
        private readonly DataDbContext _context;
        
        public SubContracts(DataDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetSubContracts([FromHeader] int customerId)
        {
            var aa = _context.SubContract.Where(r => r.CustomerId == customerId).ToList();
            return Ok(aa);
        }

        [HttpPost]
        public IActionResult PostSubContracts([FromHeader] int customerId,[FromBody] SubContractDb subContract)
        {
            var currentTime = DateTime.Now;
            _context.SubContract.Add(new SubContractDb()
            {
                Id = 0,
                Name= subContract.Name,
                Status= subContract.Status,
                TotalPrice= 0,
                ContractorId= subContract.ContractorId,
                CustomerId = customerId,
                LastModified = currentTime,

            });

            
            _context.SaveChanges();
            return Ok("Sub-contractor Created Successfully");
        }

        

        [HttpGet("{id}/contractItems")]
        public IActionResult GetContractItems(int id,[FromHeader] int customerId)
        {
            var aa = _context.ContractItem.Where(i => i.SubContractId == id && i.CustomerId == customerId).ToList();
            return Ok(aa);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSubContract(int id, [FromHeader] int customerId)
        {
            var foundSubCont = _context.SubContract.FirstOrDefault(i => i.Id == id && i.CustomerId == customerId);

            if (foundSubCont == null)
            {
                return NotFound();
            }
            var aa = _context.ContractItem.Where(i => i.SubContractId == id && i.CustomerId == customerId).ToList();
            aa.ForEach(q => {
                _context.Remove(q);
            });
            _context.Remove(foundSubCont);
            _context.SaveChanges();
            return Ok("SubContract and Associated Contract Items Deleted!");
        }


        [HttpPut("{id}")]
        public IActionResult PutSubContract(int id, [FromHeader] int customerId, [FromBody] SubContractFullObj subContractFull)
        {
            List<ContractItemObj> contractItemObjs = subContractFull.ContractItems;
            SubContractDb subContract = subContractFull.SubContract;

            var foundSubCont = _context.SubContract.FirstOrDefault(i => i.Id == id && i.CustomerId == customerId);

            if (foundSubCont == null) 
            {
                return NotFound();
            }

            var foundContractItems = _context.ContractItem.Where(i => i.SubContractId == id && i.CustomerId == customerId);
            int? totalPrice = foundSubCont.TotalPrice;

            contractItemObjs.ForEach(q =>
            {
                if (q.EditedAction == EditedAction.Created)
                {
                    _context.ContractItem.Add(new ContractItemDb()
                    {
                        Id = 0,
                        SubContractId = id,
                        Name = q.Name,
                        Price = q.Price,
                        Status = q.Status,
                        StartDate = q.StartDate,
                        EndDate = q.EndDate,
                        CustomerId = customerId,
                    });
                    totalPrice += q.Price;
                }

                else
                {
                    var foundContractItem = foundContractItems.FirstOrDefault(qt => qt.Id == q.Id);
                    if (foundContractItem != null)
                    {
                        if (q.EditedAction == EditedAction.Deleted)
                        {
                            _context.ContractItem.Remove(foundContractItem);
                            totalPrice -= q.Price;
                        }
                        else if (q.EditedAction == EditedAction.Modified)
                        {
                            if (foundContractItem.Price != q.Price) {
                                totalPrice += q.Price;
                                totalPrice -= foundContractItem.Price;
                            }

                            foundContractItem.Name = q.Name;
                            foundContractItem.Price = q.Price;
                            foundContractItem.Status = q.Status;
                            foundContractItem.StartDate = q.StartDate;
                            foundContractItem.EndDate = q.EndDate;

                        }
                    }
                }
            });
            var currentTime = DateTime.Now;

            foundSubCont.Name = subContract.Name;
            foundSubCont.Status = subContract.Status;
            foundSubCont.TotalPrice = totalPrice;
            foundSubCont.ContractorId = subContract.ContractorId;
            foundSubCont.LastModified = currentTime;

            _context.SaveChanges();
            return Ok("Sub-contractor Updated Successfully");
        }


        //CONTRACTORS

        [HttpGet("contractors")]
        public IActionResult GetContractors([FromHeader] int customerId)
        {
            var aa = _context.Contractor.Where(r => r.CustomerId == customerId).ToList();
            return Ok(aa);
        }

        [HttpPost("contractors")]
        public IActionResult PostContractors([FromHeader] int customerId, [FromBody] ContractorDb contractor)
        {
            _context.Contractor.Add(new ContractorDb()
            {
                Id = 0,
                Name = contractor.Name,
                Phone = contractor.Phone,
                Email = contractor.Email,
                CustomerId = customerId
            });

            _context.SaveChanges();
            return Ok("Contractor Created Successfully");
        }

        [HttpPut("contractors/{id}")]
        public IActionResult PutContractors(int id, [FromHeader] int customerId, [FromBody] ContractorDb contractor)
        {
            var foundContractor = _context.Contractor.FirstOrDefault(i => i.Id == id && i.CustomerId == customerId);

            if (foundContractor == null)
            {
                return NotFound();
            }

            foundContractor.Name = contractor.Name;
            foundContractor.Phone = contractor.Phone;
            foundContractor.Email = contractor.Email;

            _context.SaveChanges();
            return Ok("Contractor Updated Successfully");
        }

        [HttpDelete("contractors/{id}")]
        public IActionResult DeleteContractor(int id, [FromHeader] int customerId)
        {
            var foundContractor = _context.Contractor.FirstOrDefault(i => i.Id == id && i.CustomerId == customerId);

            if (foundContractor == null)
            {
                return NotFound();
            } 

            var aa = _context.SubContract.Where(i => i.ContractorId == id && i.CustomerId == customerId).ToList();
            aa.ForEach(q => {
                q.ContractorId = -1;
            });
            _context.Remove(foundContractor);
            _context.SaveChanges();
            return Ok("Success! Contractor Deleted!");
        }

    }

}
