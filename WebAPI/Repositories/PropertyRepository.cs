using WebAPI.Data;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using System.Collections.Generic;
using WebAPI.ViewModels.Property;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly HomeFinderContext _context;

        public PropertyRepository(HomeFinderContext context)
        {
            _context = context;
        }

        public async Task<List<PropertyViewModel>> GetPropertiesAsync()
        {
            // Hämta alla properties
            var properties = await _context.Properties
                .Include(p => p.Address)
                .Include(p => p.EstateAgent)
                .Include(p => p.PropertyType)
                .Include(p => p.SaleStatus)
                .Include(p => p.Tenure)
                .ToListAsync();
            var propertyViewModels = new List<PropertyViewModel>();

            foreach (var p in properties)
            {
                propertyViewModels.Add(new PropertyViewModel
                {
                    PropertyId = p.Id,
                    Description = p.Description,
                    Summary = p.Summary,
                    NumberOfRooms = p.NumberOfRooms,
                    BuildingArea = p.BuildingArea,
                    BeeArea = p.BeeArea,
                    PlotArea = p.PlotArea,
                    ConstructionYear = p.ConstructionYear,
                    NumberOfViews = p.NumberOfViews,
                    MapUrl = p.MapUrl,
                    Address = string.Concat(p.Address.Street, " ", p.Address.StreetNumber, " ", p.Address.PostalCode, " ", p.Address.City, " ", p.Address.Country),
                    EstateAgent = string.Concat(p.EstateAgent.FirstName, " ", p.EstateAgent.LastName),
                    PropertyType = p.PropertyType.Description,
                    SaleStatus = p.SaleStatus.Description,
                    Tenure = p.Tenure.Description,
                    PublishingDate = p.PublishingDate,
                    ViewingDate = p.ViewingDate

                });
            }

            return propertyViewModels;
        }

        // här jävlar händer det grejer, multitaskQueen XD

        //I try xD Kom på att vi måste ju visa adress och sånt i viewmodel också ;__; oh fuk
        public void CreateProperty()
        {
            //Create shit here

            //Nej.
        }
    }
}