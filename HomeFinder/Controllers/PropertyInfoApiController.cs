using HomeFinder.Data;
using HomeFinder.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyInfoApiController : ControllerBase
    {
        private readonly HomeFinderContext _context;

        public PropertyInfoApiController(HomeFinderContext context)
        {
            _context = context;
        }



        // GET: api/PropertyInfoApi/PropertyType
        [HttpGet]
        [Route("PropertyType")]
        public async Task<ActionResult<IEnumerable<PropertyType>>> GetPropertyTypes()
        {
            return await _context.PropertyTypes.ToListAsync();
        }

        // GET: api/PropertyInfoApi/PropertyType/5
        [HttpGet("{id}")]
        [Route("PropertyType/{id}")]
        public async Task<ActionResult<PropertyType>> GetPropertyType(int id)
        {
            var propertyType = await _context.PropertyTypes.FindAsync(id);

            if (propertyType == null)
            {
                return NotFound();
            }

            return propertyType;
        }



        // GET: api/PropertyInfoApi/SaleStatus
        [HttpGet]
        [Route("SaleStatus")]
        public async Task<ActionResult<IEnumerable<SaleStatus>>> GetSaleStatuses()
        {
            return await _context.SaleStatuses.ToListAsync();
        }

        // GET: api/PropertyInfoApi/SaleStatus/5
        [HttpGet("{id}")]
        [Route("SaleStatus/{id}")]
        public async Task<ActionResult<SaleStatus>> GetSaleStatus(int id)
        {
            var saleStatus = await _context.SaleStatuses.FindAsync(id);

            if (saleStatus == null)
            {
                return NotFound();
            }

            return saleStatus;
        }



        // GET: api/PropertyInfoApi/Tenure
        [HttpGet]
        [Route("Tenure")]
        public async Task<ActionResult<IEnumerable<Tenure>>> GetTenures()
        {
            return await _context.Tenures.ToListAsync();
        }

        // GET: api/PropertyInfoApi/Tenure/5
        [HttpGet("{id}")]
        [Route("Tenure/{id}")]
        public async Task<ActionResult<Tenure>> GetTenure(int id)
        {
            var tenure = await _context.Tenures.FindAsync(id);

            if (tenure == null)
            {
                return NotFound();
            }

            return tenure;
        }
    }
}
