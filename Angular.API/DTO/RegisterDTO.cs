using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Angular.API.DTO
{
    public class RegisterDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(250)]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [StringLength(26)]
        [Required]
        public string Password { get; set; }
    }
}
