using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Models.Dtos
{
    public class DistributionCenterTruckOnRouteDto
    {
        public CenterItemDto DistributionCenterOrigin { get; set; }
        public CenterItemDto Destination { get; set; }
        public IEnumerable<RelationShipBetweenTwoCentersDto> RelationShips { get; set; }
        public DistributionCenterTruckDto Truck { get; set; }
        public TruckOnRoute Route { get; set; }
    }

    public class TruckOnRoute
    {
        public string IdTruck { get; set; }
        public string SerialNumberTruck { get; set; }
        public string DistributionCenterAddress { get; set; }
        public string DistributionCenterCity { get; set; }
        public string DistributionCenterCountry { get; set; }
        public string DistributionCenterPhoneNumber { get; set; }
        public string DistributionCenterTimeSchedule { get; set; }
        public RoutePoint OriginRoute { get; set; }
        public RoutePoint DestinationRoute { get; set; }
        public RoutePoint IncidentRoute { get; set; }
    }

    public class RoutePoint
    {
        public string Id { get; set; }
        public string  Description { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string DateAndTime { get; set; }
        public decimal[] Position { get; set; }
    }
}

