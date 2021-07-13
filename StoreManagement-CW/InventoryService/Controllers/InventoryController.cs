using InventoryService.Database;
using InventoryService.Database.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryService.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        DatabaseContext db;

        public InventoryController()
        {
            db = new DatabaseContext();
        }
        // GET: api/<InventoryController>
        [HttpGet]
        public IEnumerable<Inventory> Get()
        {
            return db.Inventories.ToList();
        }

        // GET api/<InventoryController>/5
        [HttpGet("{id}")]
        public Inventory Get(int id)
        {
            return db.Inventories.Find(id);
        }

        // POST api/<InventoryController>
        [HttpPost]
        public IActionResult Post([FromBody] Inventory model)
        {
            try
            {
                db.Inventories.Add(model);
                db.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, model);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/<InventoryController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Inventory model)
        {

            try
            {
                db.Inventories.Update(model);
                db.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, model);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE api/<InventoryController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                Inventory inventory = db.Inventories.Find(id);
                if (inventory is not null)
                {
                    db.Inventories.Remove(inventory);
                    db.SaveChanges();
                    return StatusCode(StatusCodes.Status200OK, id);
                }
                return StatusCode(StatusCodes.Status404NotFound);


            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
