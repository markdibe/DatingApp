using Angular.API.DTO;
using Angular.API.Entities;
using Angular.API.Extensions;
using Angular.API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Angular.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {

        private readonly IUserRepository _userRepos;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photo;
        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photo)
        {
            _userRepos = userRepository;
            _mapper = mapper;
            _photo = photo;
        }

        [HttpGet]
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

        [HttpGet("{username}", Name = "GetUser")]
        public async Task<ActionResult<MemberDTO>> GetByUser([FromRoute] string username)
        {
            var user = await _userRepos.GetUserByUserNameAsync(username);
            var returnedUser = _mapper.Map<MemberDTO>(user);
            return Ok(returnedUser);
        }


        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDTO member)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepos.GetUserByUserNameAsync(userName);
            _mapper.Map(member, user);
            _userRepos.Update(user);
            if (await _userRepos.SaveAllAsync()) return NoContent();
            return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
        {
            string userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userRepos.GetUserByUserNameAsync(userName);
            var result = await _photo.AddPhotoAsync(file);
            if (result.Error != null) { return BadRequest(result.Error.Message); }
            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };
            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }
            user.Photos.Add(photo);
            if (await _userRepos.SaveAllAsync())
            {
                return CreatedAtRoute("GetUser", new { username = user.UserName }, _mapper.Map<PhotoDTO>(photo)); ;
            }
            return BadRequest("Problem while adding a phot");

        }




        [HttpPut("{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRepos.GetUserByUserNameAsync(User.FindFirstValue(ClaimTypes.Name));
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo.IsMain)
            {
                return BadRequest("this is already your main photo!");
            }
            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) { currentMain.IsMain = false; }
            var selectedPhoto = user.Photos.FirstOrDefault(x => x.Id == photoId);
            selectedPhoto.IsMain = true;
            if (await _userRepos.SaveAllAsync()) { return NoContent(); }
            else { return BadRequest("Failed to set main photo"); }

        }
        [HttpDelete("{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepos.GetUserByUserNameAsync(User.FindFirstValue(ClaimTypes.Name));
            if (user != null)
            {
                bool photoExists = user.Photos.Any(x => x.Id == photoId && !x.IsMain);
                if (photoExists)
                {
                    Photo selectedPhoto = user.Photos.FirstOrDefault(x => x.Id == photoId);
                    var result = await _photo.DeletePhotoAsync(selectedPhoto.PublicId);
                    if (result.Error != null)
                    {
                        return BadRequest("can not delete the photo");
                    }
                    user.Photos.Remove(selectedPhoto);
                }
                if (await (_userRepos.SaveAllAsync())) { return Ok(); }
            }
            return BadRequest("Could not delete the photo!");
        }



    }
}
