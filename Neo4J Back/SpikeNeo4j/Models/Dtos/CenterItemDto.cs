﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Models.Dtos
{
    

    public class CenterItemDto
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public decimal[] Position { get; set; }
        public string Type { get; set; }
        public bool HasProblem { get; set; }
    }
}
