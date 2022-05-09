using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.ViewModels.Property;

namespace WebAPI.Interfaces
{
    public interface IPropertyRepository
    {

        public void CreateProperty();

        public Task<List<PropertyViewModel>> GetPropertiesAsync();
    }
}