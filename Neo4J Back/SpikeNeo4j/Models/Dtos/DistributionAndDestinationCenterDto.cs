using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Models.Dtos
{
    public class DistributionAndDestinationCenterDto
    {
        public IEnumerable<DistributionCenterDto> DistributionCenters { get; set; }
        public IEnumerable<DestinationCenterDto> DestinationCenters { get; set; }
        public IEnumerable<RelationShipBetweenTwoCentersDto> RelationShips { get; set; }
    }
}
