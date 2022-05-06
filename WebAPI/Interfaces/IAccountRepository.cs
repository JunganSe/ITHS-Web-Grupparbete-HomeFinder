using System.Threading.Tasks;
using WebAPI.ViewModels.Account;

namespace WebAPI.Interfaces
{
    public interface IAccountRepository
    {

        public Task CreateAccountAsync(PostAccountViewModel model);
    }
}