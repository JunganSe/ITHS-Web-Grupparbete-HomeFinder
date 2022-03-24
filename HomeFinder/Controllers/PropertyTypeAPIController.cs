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
    [Route("api/PropertyTypeAPI")]
    [ApiController]
    public class PropertyTypeAPIController : ControllerBase
    {
        private readonly HomeFinderContext _context;

        public PropertyTypeAPIController(HomeFinderContext context)
        {
            _context = context;
        }

        // GET: api/PropertyTypeAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyType>>> GetPropertyTypes()
        {
            return await _context.PropertyTypes.ToListAsync();
        }

        // GET: api/PropertyTypeAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyType>> GetPropertyType(int id)
        {
            var propertyType = await _context.PropertyTypes.FindAsync(id);

            if (propertyType == null)
            {
                return NotFound();
            }

            return propertyType;
        }

        // PUT: api/PropertyTypeAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPropertyType(int id, PropertyType propertyType)
        {
            if (id != propertyType.Id)
            {
                return BadRequest();
            }

            _context.Entry(propertyType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PropertyTypeExists(id))
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

        // POST: api/PropertyTypeAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PropertyType>> PostPropertyType(PropertyType propertyType)
        {
            _context.PropertyTypes.Add(propertyType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPropertyType", new { id = propertyType.Id }, propertyType);
        }

        // DELETE: api/PropertyTypeAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePropertyType(int id)
        {
            var propertyType = await _context.PropertyTypes.FindAsync(id);
            if (propertyType == null)
            {
                return NotFound();
            }

            _context.PropertyTypes.Remove(propertyType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PropertyTypeExists(int id)
        {
            return _context.PropertyTypes.Any(e => e.Id == id);
        }
    }
}
