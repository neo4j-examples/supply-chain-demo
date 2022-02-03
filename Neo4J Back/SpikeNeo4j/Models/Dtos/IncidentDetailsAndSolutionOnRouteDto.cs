using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Models.Dtos
{
    

    public class IncidentDetailsAndSolutionOnRouteDto
    {
        public string Id { get; set; }
        public string Description { get; set; }      
        public string DetailedDescription { get; set; }        
        public string Solution { get; set; }
        public decimal[] Position { get; set; }
    }

    public class IncidentDetailsAndSolutionTruckSensorsOnRouteDto : IncidentDetailsAndSolutionOnRouteDto
    {        
        public List<TruckSensorDto> SensorsState { get; set; }
        public List<TruckSensorDto> SensorsInformationState { get; set; }
    }

    public class TruckSensorDto
    {
        public string Name { get; set; }
        public bool Failure { get; set; }
        public decimal Value { get; set; }
        //public string Type { get; set; }
    }
}
