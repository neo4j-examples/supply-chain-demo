using Newtonsoft.Json;
using SpikeNeo4j.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Models
{
    public class Route
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("startDateTime")]
        public string StartDateTime { get; set; }
        [JsonProperty("endDateTime")]
        public string EndDateTime { get; set; }
        public static string Labels => nameof(Route);
       
    }
}
