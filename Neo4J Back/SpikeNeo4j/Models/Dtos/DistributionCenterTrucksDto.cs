using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Models.Dtos
{
    public class DistributionCenterTrucksDto
    {
        public DistributionCenterDto DistributionCenter { get; set; }
        public IEnumerable<DistributionCenterTruckDto> Trucks { get; set; }
        public IEnumerable<RelationShipBetweenTwoCentersDto> RelationShips { get; set; }
    }
}
