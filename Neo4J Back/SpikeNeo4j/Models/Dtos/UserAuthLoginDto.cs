using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Models.Dtos
{
    public class UserAuthLoginDto
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string Username { get; set; }
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string Password { get; set; }
    }
}
