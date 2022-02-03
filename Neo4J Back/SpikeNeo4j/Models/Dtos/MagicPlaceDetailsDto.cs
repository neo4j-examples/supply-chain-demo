using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Models.Dtos
{
    public class MagicPlaceDetailsDto
    {
        public string Id { get; set; }
        public string Description { get; set; }   
        public string City { get; set; }
        public decimal[] Position { get; set; }
        public string Type { get; set; }
        public bool HasProblem { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }     
        public string Reference { get; set; }           
    }
}

