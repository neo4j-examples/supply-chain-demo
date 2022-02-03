using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Models.Dtos
{
    public class MagicPlaceDto
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public decimal[] Position { get; set; }
    }
}

