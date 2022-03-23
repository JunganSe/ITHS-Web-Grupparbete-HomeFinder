using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeFinder.Data;
using HomeFinder.Models;

namespace HomeFinder.Controllers
{
    public class PropertiesController : Controller
    {
        private readonly HomeFinderContext _context;

        public PropertiesController(HomeFinderContext context)
        {
            _context = context;
        }

        // GET: Properties
        public async Task<IActionResult> Index()
        {
            var homeFinderContext = _context.Properties.Include(p => p.Adress).Include(p => p.EstateAgent).Include(p => p.PropertyType).Include(p => p.SaleStatus).Include(p => p.Tenure);
            return View(await homeFinderContext.ToListAsync());
        }

        // GET: Properties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var property = await _context.Properties
                .Include(p => p.Adress)
                .Include(p => p.EstateAgent)
                .Include(p => p.PropertyType)
                .Include(p => p.SaleStatus)
                .Include(p => p.Tenure)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (property == null)
            {
                return NotFound();
            }

            return View(property);
        }

        // GET: Properties/Create
        public IActionResult Create()
        {
            ViewData["AdressId"] = new SelectList(_context.Adresses, "Id", "City");
            ViewData["EstateAgentId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            ViewData["PropertyTypeId"] = new SelectList(_context.PropertyTypes, "Id", "Description");
            ViewData["SaleStatusId"] = new SelectList(_context.SaleStatuses, "Id", "Description");
            ViewData["TenureId"] = new SelectList(_context.Tenures, "Id", "Description");
            return View();
        }

        // POST: Properties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Price,Description,NumberOfRooms,BuildingArea,PlotArea,ConstructionYear,NumberOfViews,MapUrl,PublishingDate,ViewingDate,AdressId,PropertyTypeId,TenureId,SaleStatusId,EstateAgentId")] Property property)
        {
            if (ModelState.IsValid)
            {
                _context.Add(property);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdressId"] = new SelectList(_context.Adresses, "Id", "City", property.AdressId);
            ViewData["EstateAgentId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", property.EstateAgentId);
            ViewData["PropertyTypeId"] = new SelectList(_context.PropertyTypes, "Id", "Description", property.PropertyTypeId);
            ViewData["SaleStatusId"] = new SelectList(_context.SaleStatuses, "Id", "Description", property.SaleStatusId);
            ViewData["TenureId"] = new SelectList(_context.Tenures, "Id", "Description", property.TenureId);
            return View(property);
        }

        // GET: Properties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var property = await _context.Properties.FindAsync(id);
            if (property == null)
            {
                return NotFound();
            }
            ViewData["AdressId"] = new SelectList(_context.Adresses, "Id", "City", property.AdressId);
            ViewData["EstateAgentId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", property.EstateAgentId);
            ViewData["PropertyTypeId"] = new SelectList(_context.PropertyTypes, "Id", "Description", property.PropertyTypeId);
            ViewData["SaleStatusId"] = new SelectList(_context.SaleStatuses, "Id", "Description", property.SaleStatusId);
            ViewData["TenureId"] = new SelectList(_context.Tenures, "Id", "Description", property.TenureId);
            return View(property);
        }

        // POST: Properties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Price,Description,NumberOfRooms,BuildingArea,PlotArea,ConstructionYear,NumberOfViews,MapUrl,PublishingDate,ViewingDate,AdressId,PropertyTypeId,TenureId,SaleStatusId,EstateAgentId")] Property property)
        {
            if (id != property.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(property);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyExists(property.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdressId"] = new SelectList(_context.Adresses, "Id", "City", property.AdressId);
            ViewData["EstateAgentId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", property.EstateAgentId);
            ViewData["PropertyTypeId"] = new SelectList(_context.PropertyTypes, "Id", "Description", property.PropertyTypeId);
            ViewData["SaleStatusId"] = new SelectList(_context.SaleStatuses, "Id", "Description", property.SaleStatusId);
            ViewData["TenureId"] = new SelectList(_context.Tenures, "Id", "Description", property.TenureId);
            return View(property);
        }

        // GET: Properties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var property = await _context.Properties
                .Include(p => p.Adress)
                .Include(p => p.EstateAgent)
                .Include(p => p.PropertyType)
                .Include(p => p.SaleStatus)
                .Include(p => p.Tenure)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (property == null)
            {
                return NotFound();
            }

            return View(property);
        }

        // POST: Properties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PropertyExists(int id)
        {
            return _context.Properties.Any(e => e.Id == id);
        }
    }
}
