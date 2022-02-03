using SpikeNeo4j.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Repository.IRepository
{
    public interface IProblemTypeRepository
    {
        List<ItemList> GetProblemTypes();
        ItemList GetTrafficJamProblem();
        ItemList GetAdverseWeatherConditionsProblem();
        public ItemList GetTruckProblem();
    }
}
