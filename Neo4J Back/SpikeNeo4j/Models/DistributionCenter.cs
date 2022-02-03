using Newtonsoft.Json;
using SpikeNeo4j.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Models
{
    public class DistributionCenter : CenterItem
    {
        public new static string Labels => nameof(DistributionCenter);
        //public string Type => nameof(DistributionCenter);

        [JsonProperty("timeStart")]
        public string TimeStart { get; set; }

        [JsonProperty("timeEnd")]
        public string TimeEnd { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("planning")]
        public decimal Planning { get; set; }

        [JsonProperty("sales")]
        public decimal Sales { get; set; }

        [JsonProperty("sourcing")]
        public decimal Sourcing { get; set; }

        [JsonProperty("warehouseAndDistribution")]
        public decimal WarehouseAndDistribution { get; set; }
        [JsonProperty("warehouseAndDistributionTotal")]
        public decimal WarehouseAndDistributionTotal { get; set; }
        public List<ProductionPerYear> Production { get; set; }

    }
}
