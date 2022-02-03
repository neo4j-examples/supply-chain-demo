using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Models
{
    public static class Relationships
    {
        public const string SupplyTo = "SUPPLY_TO";
        public const string Linked = "LINKED";
        public const string TruckLinkedWithTruck = "LINKED_WITH_TRUCK";
        public const string SystemSensorTruck = "SYSTEM";
        public const string TruckOnRoute = "ON_ROUTE";
        public const string RouteLegOrigin = "LEG_ORIGIN";
        public const string RouteLegDestination = "LEG_DESTINATION";
        public const string RouteHasProblem = "HAS_PROBLEM";
    }
}
