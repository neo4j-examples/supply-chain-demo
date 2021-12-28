using SpikeNeo4j.Models;
using SpikeNeo4j.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Repository
{
    public class ProblemTypeRepository : IProblemTypeRepository
    {

        private readonly List<ItemList> _problemTypes;

        public ProblemTypeRepository()
        {
            _problemTypes = new List<ItemList>()
            {
                new ItemList{Id="1",Description="Traffic jam"},
                new ItemList{Id="2",Description="Adverse weather conditions"},
                new ItemList{Id="3",Description="Truck problem"},
            };
        }

        public List<ItemList> GetProblemTypes()
        {
            return _problemTypes;
        }

        public ItemList GetTrafficJamProblem()
        {
            return _problemTypes.FirstOrDefault(x => x.Id == "1");
        }

        public ItemList GetAdverseWeatherConditionsProblem()
        {
            return _problemTypes.FirstOrDefault(x => x.Id == "2");
        }

        public ItemList GetTruckProblem()
        {
            return _problemTypes.FirstOrDefault(x => x.Id == "3");
        }
    }
}


