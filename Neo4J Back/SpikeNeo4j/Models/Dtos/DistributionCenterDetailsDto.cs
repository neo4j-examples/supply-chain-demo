using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Models.Dtos
{
    public class DistributionCenterDetailsDto
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
        public decimal Planning { get; set; }
        public decimal Sales { get; set; }
        public decimal Sourcing { get; set; }       
        public decimal WarehouseAndDistribution { get; set; }
        public decimal WarehouseAndDistributionTotal { get; set; }
        public List<ProductionPerYear> Production { get; set; }

    }

    public class ProductionPerYear
    {
        public int Year { get; set; }
        public decimal Production { get; set; }
    }
}

