using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using StaffShiftService.Database;
using StaffShiftService.Database.Entites;

namespace StaffShiftService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffShiftController : ControllerBase
    {
        DatabaseContext db;
        // GET: api/<StaffController>
        public StaffShiftController()
        {
            db = new DatabaseContext();
        }
        [HttpGet]
        public IEnumerable<StaffShift> Get()
        {
            return db.StaffShifts.ToList();
        }

        // GET api/<StaffController>/5
        [HttpGet("{id}")]
        public StaffShift Get(int id)
        {
            return db.StaffShifts.Find(id);
        }

        // POST api/<StaffController>
        [HttpPost]
        public IActionResult Post([FromBody] StaffShift model)
        {
            try
            {
                db.StaffShifts.Add(model);
                db.SaveChanges();
                return StatusCode(StatusCodes.Status201Created, model);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);

            }
        }

        // PUT api/<StaffController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] StaffShift model)
        {
            try
            {
                db.StaffShifts.Update(model);
                db.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, model);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE api/<StaffController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            try
            {
                StaffShift staff = db.StaffShifts.Find(id);
                if (staff is not null)
                {
                    db.StaffShifts.Remove(staff);
                    db.SaveChanges();
                    return StatusCode(StatusCodes.Status200OK, id);
                }
                return StatusCode(StatusCodes.Status404NotFound, id);

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
