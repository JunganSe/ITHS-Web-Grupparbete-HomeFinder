using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.ViewModels.Account;

namespace WebAPI.Interfaces
{
    public interface IAccountRepository
    {

        public Task<bool>CreateAccountAsync(PostAccountViewModel model);
        public Task<bool>LoginAsync(LoginViewModel model);
        public Task<bool> ExternalLogin(string provider, string returnUrl);
        public Task<bool> ExternalLoginCallback(string returnurl = null, string remoteError = null);
        public Task<bool> Logout();
 




    }
}