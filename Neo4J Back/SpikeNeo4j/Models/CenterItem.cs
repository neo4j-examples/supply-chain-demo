using Newtonsoft.Json;
using SpikeNeo4j.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Models
{
    public class CenterItem
    {
        [JsonProperty("id")] //This is for Neo4jClient
        public int Id { get; set; }
        [JsonProperty("description")] 
        public string Description { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("zoomLevel")]
        public int ZoomLevel { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("hasProblem")]
        public bool HasProblem { get; set; }

        public static string Labels => nameof(CenterItem);
        [JsonProperty("position")]
        public decimal[] Position { get; set; }

    }
}
