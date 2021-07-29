using Angular.API.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Angular.API.DTO
{
    public class MemberDTO
    {


        public string Username { get; set; }

        public string PhotoUrl { get; set; }
        public int Age { get; set; }

        [StringLength(250)]
        public string FirstName { get; set; }
        [StringLength(250)]
        public string LastName { get; set; }
        [StringLength(250)]
        public string FatherName { get; set; }
        [StringLength(250)]
        public string MotherName { get; set; }

        public string KnownAs { get; set; }

        public DateTime Created { get; set; } 

        public DateTime LastActive { get; set; } 

        public string Gender { get; set; }

        public string Introduction { get; set; }

        public string LookingFor { get; set; }

        public string Interests { get; set; }

        public string Country { get; set; }

        public ICollection<PhotoDTO> Photos { get; set; }
    }
}
