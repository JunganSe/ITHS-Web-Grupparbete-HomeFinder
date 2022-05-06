using HomeFinder.Data;
using System.Threading.Tasks;
using WebAPI.Interfaces;

namespace WebAPI.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly HomeFinderContext _context;

        public PropertyRepository(HomeFinderContext context)
        {
            _context = context;
        }

        public void CreateProperty()
        {
            //Create shit here

            //Nej.
        }
    }
}