using Newtonsoft.Json;
using SpikeNeo4j.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Models
{
    public class IncidentDetailsAndSolutionTruckSensorsOnRoute : IncidentDetailsAndSolutionOnRoute
    {
        public List<TruckSensor> SensorsState { get; set; }
        public List<TruckSensor> SensorsInformationState { get; set; }
    }
}
