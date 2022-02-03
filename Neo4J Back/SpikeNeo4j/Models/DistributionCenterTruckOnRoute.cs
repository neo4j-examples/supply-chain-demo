using Newtonsoft.Json;
using SpikeNeo4j.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Models
{
    public class DistributionCenterTruckOnRoute
    {
        public CenterItem DistributionCenterOrigin { get; set; }
        public CenterItem Destination { get; set; }
        public Truck Truck { get; set; }
        public IEnumerable<RelationShipBetweenTwoItems> RelationShips { get; set; }
        public RouteProblem DestinationRoute { get; set; }
    }
}
