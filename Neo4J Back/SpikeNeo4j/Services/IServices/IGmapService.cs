using GMap.NET;
using GoogleApi.Entities.Maps.Directions.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Services.IServices
{
    public interface IGmapService
    {
        List<decimal[]> getPointsOnRoute(decimal[] positionFrom, decimal[] positionTo, bool alternative);
        List<decimal[]> getRouteGoogleApi(decimal[] positionFrom, decimal[] positionTo);
        public decimal[] getPositionForPlace(string keyworks);
    }
}
