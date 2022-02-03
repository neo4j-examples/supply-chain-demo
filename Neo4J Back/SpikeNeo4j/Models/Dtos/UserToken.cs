using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Models.Dtos
{
    public class UserToken
    {
        public string Token { get; set; }
        public DateTime Expiracion { get; set; }
    }
}
