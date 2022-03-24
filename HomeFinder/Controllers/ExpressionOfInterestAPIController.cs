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
    [Route("api/ExpressionOfInterestAPI")]
    [ApiController]
    public class ExpressionOfInterestAPIController : ControllerBase
    {
        private readonly HomeFinderContext _context;

        public ExpressionOfInterestAPIController(HomeFinderContext context)
        {
            _context = context;
        }

        // GET: api/ExpressionOfInterestAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpressionOfInterest>>> GetExpressionOfInterests()
        {
            return await _context.ExpressionOfInterests.ToListAsync();
        }

        // GET: api/ExpressionOfInterestAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpressionOfInterest>> GetExpressionOfInterest(string id)
        {
            var expressionOfInterest = await _context.ExpressionOfInterests.FindAsync(id);

            if (expressionOfInterest == null)
            {
                return NotFound();
            }

            return expressionOfInterest;
        }

        // PUT: api/ExpressionOfInterestAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpressionOfInterest(string id, ExpressionOfInterest expressionOfInterest)
        {
            if (id != expressionOfInterest.ApplicationUserId)
            {
                return BadRequest();
            }

            _context.Entry(expressionOfInterest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpressionOfInterestExists(id))
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

        // POST: api/ExpressionOfInterestAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ExpressionOfInterest>> PostExpressionOfInterest(ExpressionOfInterest expressionOfInterest)
        {
            _context.ExpressionOfInterests.Add(expressionOfInterest);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ExpressionOfInterestExists(expressionOfInterest.ApplicationUserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetExpressionOfInterest", new { id = expressionOfInterest.ApplicationUserId }, expressionOfInterest);
        }

        // DELETE: api/ExpressionOfInterestAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpressionOfInterest(string id)
        {
            var expressionOfInterest = await _context.ExpressionOfInterests.FindAsync(id);
            if (expressionOfInterest == null)
            {
                return NotFound();
            }

            _context.ExpressionOfInterests.Remove(expressionOfInterest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExpressionOfInterestExists(string id)
        {
            return _context.ExpressionOfInterests.Any(e => e.ApplicationUserId == id);
        }
    }
}
