using Angular.API.DTO;
using Angular.API.Entities;
using Angular.API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Angular.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class UsersController : ControllerBase
    {

        private readonly IUserRepository _userRepos;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository userRepository, IMapper mapper) 
        {
            _userRepos = userRepository;
            _mapper = mapper;
        }
        // GET: api/<UsersController>

     

        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> Get()
        {            
            return Ok(await _userRepos.GetMembersAsync());
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDTO>> Get(string id)
        {
            var user = await _userRepos.GetUserByidAsync(id);
            var returnedUser = _mapper.Map<MemberDTO>(user);
            return Ok(returnedUser);
        }

        [HttpGet]
        public async Task<ActionResult<MemberDTO>> GetByUser(string userName)
        {
            var user = await _userRepos.GetUserByUserNameAsync(userName);
            var returnedUser = _mapper.Map<MemberDTO>(user);
            return Ok(returnedUser);
        }





    }
}
