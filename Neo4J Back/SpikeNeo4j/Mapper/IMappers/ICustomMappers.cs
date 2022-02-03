using GMap.NET;
using GoogleApi.Entities.Common;
using SpikeNeo4j.Models;
using SpikeNeo4j.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Mapper.IMappers
{
    public interface ICustomMappers
    {
        TruckOnRoute MapTruckRoute(DistributionCenterTruckOnRoute distributionCenterTruckOnRoute);
        List<decimal[]> LatPointLatLongToArrayDecimal(List<PointLatLng> pointsLatLng);
        List<decimal[]> CoordinateToArrayDecimal(List<Coordinate> pointsLatLng);


    };
}
