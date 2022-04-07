using HomeFinder.Data;
using HomeFinder.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HomeFinder.Controllers
{
    public class ExpressionOfInterestController : Controller
    {
        private readonly HomeFinderContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ExpressionOfInterestController(HomeFinderContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Add(int id)
        {
            var expressionOfInterest = new ExpressionOfInterest()
            {
                ApplicationUser = await _userManager.GetUserAsync(User),
                Property = await _context.Properties.FindAsync(id)
            };
            await _context.ExpressionOfInterests.AddAsync(expressionOfInterest);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Properties", new { id });
        }


        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            ExpressionOfInterest expressionOfInterest = await _context.ExpressionOfInterests.FirstOrDefaultAsync(eoi=>eoi.ApplicationUserId == user.Id 
                                                                                                    && eoi.PropertyId == id);
            _context.ExpressionOfInterests.Remove(expressionOfInterest);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Properties", new {id});
        }

    }
}
