using Angular.API.Entities;
using Angular.API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Angular.API.DTO
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MemberDTO> GetMemberAsync(string userName)
        {
            return await _context.ApplicationUsers.Where(x => x.UserName.ToLower().Equals(userName.ToLower()))
                .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
        }

        public async Task<MemberDTO> GetMemberAsyncById(string id)
        {
            return await _context.ApplicationUsers
                .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<MemberDTO>> GetMembersAsync()
        {
            return await _context.ApplicationUsers
                .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<ApplicationUser> GetUserByidAsync(string id)
        {
            return await _context.ApplicationUsers.FindAsync(id);
        }

        public async Task<ApplicationUser> GetUserByUserNameAsync(string userName)
        {
            return await _context.ApplicationUsers
                .Include(x=>x.Photos)
                .FirstOrDefaultAsync(x => x.UserName.ToLower().Equals(userName.ToLower()));
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersAsync()
        {
            return await _context.ApplicationUsers
                .Include(x=>x.Photos)
                .ToListAsync();
        }

        public  async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(ApplicationUser user)
        {
            _context.Entry<ApplicationUser>(user).State = EntityState.Modified;
        }
    }
}
