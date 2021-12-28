using Newtonsoft.Json;
using SpikeNeo4j.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Models
{
    public class RelationShipBetweenTwoItems
    {
        public string From { get; set; }
        public string To { get; set; }
        public decimal[] PositionFrom { get; set; }
        public decimal[] PositionTo { get; set; }
    }
}
