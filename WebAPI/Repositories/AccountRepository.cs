using HomeFinder.Data;
using HomeFinder.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Claims;
using WebAPI.Interfaces;
using WebAPI.ViewModels.Account;
using System;

namespace WebAPI.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly HomeFinderContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountRepository(HomeFinderContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }


        public async Task CreateAccountAsync(PostAccountViewModel model)
        {

            var user = new ApplicationUser()
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded) // Om det gick bra att skapa användaren:
            {
                await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, false);
            }
            else // Om det inte gick att skapa användaren:
            {
                throw new Exception("Det gick inte att skapa användaren");
            }
        }
    }


}