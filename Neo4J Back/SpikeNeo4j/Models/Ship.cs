using Newtonsoft.Json;
using SpikeNeo4j.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Models
{
    public class Ship
    {
        [JsonProperty("id")]
        public string Id { get; set; }       
        [JsonProperty("description")]
        public string Description { get; set; }       
        public static string Labels => nameof(Ship);
        [JsonProperty("positions")]
        public decimal[][] Positions { get; set; }
    }
}
