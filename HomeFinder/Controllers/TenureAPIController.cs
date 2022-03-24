using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeFinder.Data;
using HomeFinder.Models;

namespace HomeFinder.Controllers
{
    [Route("api/TenureAPI")]
    [ApiController]
    public class TenureAPIController : ControllerBase
    {
        private readonly HomeFinderContext _context;

        public TenureAPIController(HomeFinderContext context)
        {
            _context = context;
        }

        // GET: api/TenureAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tenure>>> GetTenures()
        {
            return await _context.Tenures.ToListAsync();
        }

        // GET: api/TenureAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tenure>> GetTenure(int id)
        {
            var tenure = await _context.Tenures.FindAsync(id);

            if (tenure == null)
            {
                return NotFound();
            }

            return tenure;
        }

        // PUT: api/TenureAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTenure(int id, Tenure tenure)
        {
            if (id != tenure.Id)
            {
                return BadRequest();
            }

            _context.Entry(tenure).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TenureExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TenureAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tenure>> PostTenure(Tenure tenure)
        {
            _context.Tenures.Add(tenure);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTenure", new { id = tenure.Id }, tenure);
        }

        // DELETE: api/TenureAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTenure(int id)
        {
            var tenure = await _context.Tenures.FindAsync(id);
            if (tenure == null)
            {
                return NotFound();
            }

            _context.Tenures.Remove(tenure);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TenureExists(int id)
        {
            return _context.Tenures.Any(e => e.Id == id);
        }
    }
}
