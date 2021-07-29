using Angular.API.DTO;
using Angular.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Angular.API.Interfaces
{
    public interface IUserRepository
    {
        void Update(ApplicationUser user);

        Task<bool> SaveAllAsync();

        Task<IEnumerable<ApplicationUser>> GetUsersAsync();

        Task<ApplicationUser> GetUserByidAsync(string id);

        Task<ApplicationUser> GetUserByUserNameAsync(string userName);

        Task<IEnumerable<MemberDTO>> GetMembersAsync();

        Task<MemberDTO> GetMemberAsync(string userName);

        Task<MemberDTO> GetMemberAsyncById(string id);

    }
}
