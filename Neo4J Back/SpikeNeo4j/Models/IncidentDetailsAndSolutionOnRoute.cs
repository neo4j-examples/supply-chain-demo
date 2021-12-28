using Newtonsoft.Json;
using SpikeNeo4j.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Models
{
    public class IncidentDetailsAndSolutionOnRoute
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string DetailedDescription { get; set; }
        public string Solution { get; set; }
        public decimal[] Position { get; set; }
    }
}
