using Angular.API.DTO;
using Angular.API.Entities;
using Angular.API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
//using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Angular.API.Controllers
{

    public class AccountController : BaseAPIController
    {
        private readonly ITokenService _token;
        public AccountController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager, ITokenService service)
            : base(context, userManager, roleManager, signInManager)
        {
            _token = service;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid) { return BadRequest(model); }
            var userExists = await _userManager.FindByEmailAsync(model.UserName);

            if (userExists != null)
            {
                return BadRequest($"User Name {model.UserName} Already Exists!");
            }
            ApplicationUser user = new ApplicationUser { UserName = model.UserName, Email = model.UserName };
            var created = await _userManager.CreateAsync(user, model.Password);
            if (created.Succeeded)
            {
                var roleAdded = await _userManager.AddToRoleAsync(user, "User");
                if (roleAdded.Succeeded)
                {
                    return StatusCode(StatusCodes.Status201Created, new UserDTO
                    {
                        UserName = model.UserName,
                        Token = await _token.CreateToken(user)
                    });
                }
                return BadRequest("Could not add user to role user");
            }
            foreach (var error in created.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(ModelState.Values.Select(x => x.Errors.Select(x => x.ErrorMessage)));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.UserName);
            if (user == null)
            {
                return Unauthorized($"Invalid UserName: {model.UserName}");
            }
            var signIn = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: true, lockoutOnFailure: false);
            if (signIn.Succeeded)
            {
                return StatusCode(StatusCodes.Status200OK, new UserDTO
                {
                    UserName = model.UserName,
                    Token = await _token.CreateToken((ApplicationUser)user)
                });
            }
            return Forbid();
        }
    }
}
