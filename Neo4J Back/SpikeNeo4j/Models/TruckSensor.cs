using Newtonsoft.Json;
using SpikeNeo4j.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Models
{
    public class TruckSensor
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("failure")]
        public bool Failure { get; set; }
        [JsonProperty("value")]
        public decimal Value { get; set; }
        [JsonProperty("problemDetailedDescription")]
        public string ProblemDetailedDescription { get; set; }
        [JsonProperty("problemSolution")]
        public string ProblemSolution { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
