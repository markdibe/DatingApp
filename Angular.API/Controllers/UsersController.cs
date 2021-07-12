using Angular.API.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Angular.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : BaseAPIController
    {


        public UsersController(ApplicationDbContext context
            , UserManager<IdentityUser> userManager
            , RoleManager<IdentityRole> roleManager
            , SignInManager<IdentityUser> signInManager) : base(context, userManager, roleManager, signInManager)
        {
        }
        // GET: api/<UsersController>

        private async Task CreateRoles()
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = "User", NormalizedName = "USER" });
            await _roleManager.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
        }

        private async Task CreateUsers()
        {
            for (int i = 0; i < 100; i++)
            {
                ApplicationUser applicationUser = new ApplicationUser
                {
                    UserName = "UserName" + i
                    ,
                    Email = $"email{i}@mail.com",
                    FirstName = $"First Name{i}",
                    LastName = $"Last Name {i}"
                };
                var created = await _userManager.CreateAsync(applicationUser, "P@$$w0rd");
                if (created.Succeeded)
                {
                    await _userManager.AddToRoleAsync(applicationUser, "User");
                }
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IEnumerable<ApplicationUser>> Get()
        {
            //await CreateRoles();
            //await CreateUsers();
            return await _userManager
                .Users.Cast<ApplicationUser>()
                .ToListAsync();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ApplicationUser> Get(string id)
        {
            return (ApplicationUser)await _userManager.FindByIdAsync(id);
        }

        // POST api/<UsersController>
        [HttpPost]
        public void Post([FromBody] ApplicationUser user)
        {

        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
