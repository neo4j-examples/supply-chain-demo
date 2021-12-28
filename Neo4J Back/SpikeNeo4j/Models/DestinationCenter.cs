using Newtonsoft.Json;
using SpikeNeo4j.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Models
{
    public class DestinationCenter : CenterItem
    {
        public new static string Labels => nameof(DestinationCenter);
        //public string Type => nameof(DestinationCenter);        
    }
}
