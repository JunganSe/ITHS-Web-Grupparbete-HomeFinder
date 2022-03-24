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
    [Route("api/SaleStatusAPI")]
    [ApiController]
    public class SaleStatusAPIController : ControllerBase
    {
        private readonly HomeFinderContext _context;

        public SaleStatusAPIController(HomeFinderContext context)
        {
            _context = context;
        }

        // GET: api/SaleStatusAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleStatus>>> GetSaleStatuses()
        {
            return await _context.SaleStatuses.ToListAsync();
        }

        // GET: api/SaleStatusAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SaleStatus>> GetSaleStatus(int id)
        {
            var saleStatus = await _context.SaleStatuses.FindAsync(id);

            if (saleStatus == null)
            {
                return NotFound();
            }

            return saleStatus;
        }

        // PUT: api/SaleStatusAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSaleStatus(int id, SaleStatus saleStatus)
        {
            if (id != saleStatus.Id)
            {
                return BadRequest();
            }

            _context.Entry(saleStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleStatusExists(id))
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

        // POST: api/SaleStatusAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SaleStatus>> PostSaleStatus(SaleStatus saleStatus)
        {
            _context.SaleStatuses.Add(saleStatus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSaleStatus", new { id = saleStatus.Id }, saleStatus);
        }

        // DELETE: api/SaleStatusAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSaleStatus(int id)
        {
            var saleStatus = await _context.SaleStatuses.FindAsync(id);
            if (saleStatus == null)
            {
                return NotFound();
            }

            _context.SaleStatuses.Remove(saleStatus);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SaleStatusExists(int id)
        {
            return _context.SaleStatuses.Any(e => e.Id == id);
        }
    }
}
