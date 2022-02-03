using Newtonsoft.Json;
using SpikeNeo4j.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Models
{
    public class Truck
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("serialNumber")]
        public string SerialNumber { get; set; }

        [JsonProperty("zoomLevel")]
        public int ZoomLevel { get; set; }
        [JsonProperty("hasProblem")]
        public bool HasProblem { get; set; }

        public static string Labels => nameof(Truck);
        [JsonProperty("position")]
        public decimal[] Position { get; set; }
    }
}
