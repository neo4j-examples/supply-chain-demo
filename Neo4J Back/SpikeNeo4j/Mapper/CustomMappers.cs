using GMap.NET;
using GoogleApi.Entities.Common;
using GoogleApi.Entities.Maps.Directions.Response;
using SpikeNeo4j.Helpers.IMathHelpers;
using SpikeNeo4j.Mapper.IMappers;
using SpikeNeo4j.Models;
using SpikeNeo4j.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Mapper
{
    public class CustomMappers : ICustomMappers
    {
        private readonly IMathHelper _mathHelper;
        public CustomMappers(IMathHelper mathHelper)
        {
            _mathHelper = mathHelper;
        }
        public TruckOnRoute MapTruckRoute(DistributionCenterTruckOnRoute distributionCenterTruckOnRoute)
        {
            if (distributionCenterTruckOnRoute == null) return new TruckOnRoute();
            return new TruckOnRoute()
            {
                IdTruck = distributionCenterTruckOnRoute.Truck?.Id,
                SerialNumberTruck = distributionCenterTruckOnRoute.Truck?.SerialNumber,
                DistributionCenterAddress = distributionCenterTruckOnRoute.DistributionCenterOrigin.Address,
                DistributionCenterCountry = distributionCenterTruckOnRoute.DistributionCenterOrigin.Country,
                DistributionCenterCity = distributionCenterTruckOnRoute.DistributionCenterOrigin.City,
                DistributionCenterPhoneNumber = distributionCenterTruckOnRoute.DistributionCenterOrigin.PhoneNumber,
                OriginRoute = new RoutePoint()
                {
                    Id = distributionCenterTruckOnRoute.DistributionCenterOrigin.Id.ToString(),
                    Description = distributionCenterTruckOnRoute.DistributionCenterOrigin.Description,
                    Address = distributionCenterTruckOnRoute.DistributionCenterOrigin.Address,
                    City = distributionCenterTruckOnRoute.DistributionCenterOrigin.City,
                    Country = distributionCenterTruckOnRoute.DistributionCenterOrigin.Country,
                    //TODO:obtener hora y fecha real usando googleapi 
                    DateAndTime = "Aug. 14, 2021 - 8:25AM",
                    Position = distributionCenterTruckOnRoute.DistributionCenterOrigin.Position
                },
                DestinationRoute = new RoutePoint()
                {
                    Id = distributionCenterTruckOnRoute.Destination.Id.ToString(),
                    Description = distributionCenterTruckOnRoute.Destination.Description,
                    Address = distributionCenterTruckOnRoute.Destination.Address,
                    City = distributionCenterTruckOnRoute.Destination.City,
                    Country = distributionCenterTruckOnRoute.Destination.Country,
                    //TODO:obtener hora y fecha real usando googleapi 
                    DateAndTime = "Aug. 14, 2021 - 15:45AM",
                    Position = distributionCenterTruckOnRoute.Destination.Position
                },
                IncidentRoute = distributionCenterTruckOnRoute.DestinationRoute!=null ? new RoutePoint()
                {
                    Id = distributionCenterTruckOnRoute.DestinationRoute.Id,
                    Description = distributionCenterTruckOnRoute.DestinationRoute.Description,
                    Address = distributionCenterTruckOnRoute.DestinationRoute.Address,
                    City = distributionCenterTruckOnRoute.DestinationRoute.City,
                    Country = distributionCenterTruckOnRoute.DestinationRoute.Country,
                    DateAndTime = distributionCenterTruckOnRoute.DestinationRoute.DateTime,
                    Position = _mathHelper.MidPoint((double)distributionCenterTruckOnRoute.Truck.Position[0], (double)distributionCenterTruckOnRoute.Truck.Position[1], (double)distributionCenterTruckOnRoute.Destination.Position[0], (double)distributionCenterTruckOnRoute.Destination.Position[1])
                } : null
            };
        }

        public List<decimal[]> LatPointLatLongToArrayDecimal(List<PointLatLng> pointsLatLng)
        {
            var points = new List<decimal[]>();
            if (pointsLatLng == null) return points;
            foreach (var item in pointsLatLng)
            {
                points.Add(new decimal[] {(decimal)item.Lat ,(decimal)item.Lng });
            }
            return points;
        }

        public List<decimal[]> CoordinateToArrayDecimal(List<Coordinate> pointsLatLng)
        {
            var points = new List<decimal[]>();
            if (pointsLatLng == null) return points;
            var i = 0;
            foreach (var item in pointsLatLng)
            {               
                if (i%10==0) points.Add(new decimal[] { (decimal)item.Latitude, (decimal)item.Longitude });
                i++;
            }
            return points;
        }
    }
}
