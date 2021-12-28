using Newtonsoft.Json;
using SpikeNeo4j.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Models.Dto
{
    public class MagicPlaceFeaturesSelectedForDistributionCenterDto
    {
        public int IdDistributionCenter { get; set; }
        public Dictionary<string,bool> Features { get; set; }
    }

    public class MagicPlaceFeaturesSelected
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; }
    }
}
