using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Models.Dtos
{
    public class DistributionCenterTruckDto
    {
        public string Id { get; set; }
        public string SerialNumber { get; set; }
        public bool HasProblem { get; set; }

        public decimal[] Position { get; set; }
    }
}

