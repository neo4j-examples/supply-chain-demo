using GMap.NET;
using GMap.NET.MapProviders;
using GoogleApi.Entities.Maps.Common;
using GoogleApi.Entities.Maps.Directions.Request;
using GoogleApi.Entities.Maps.Directions.Response;
using GoogleApi.Entities.Maps.Geocoding.Address.Request;
using Microsoft.Extensions.Configuration;
using SpikeNeo4j.Mapper.IMappers;
using SpikeNeo4j.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Services
{
    public class GmapService : IGmapService
    {

        private readonly ICustomMappers _customMapper;
        private readonly IConfiguration _configuration;

        public GmapService(ICustomMappers customMapper, IConfiguration configuration)
        {
            _customMapper = customMapper;
            _configuration = configuration;
        }

        public List<decimal[]> getPointsOnRoute(decimal[] positionFrom, decimal[] positionTo,bool alternative)
        {
            GoogleMapProvider.Instance.ApiKey = _configuration.GetSection("ApikeyGoogleMaps:ApiKey").Value;
            var start = new PointLatLng((double)positionFrom[0], (double)positionFrom[1]);
            var end = new PointLatLng((double)positionTo[0], (double)positionTo[1]);
            var route = GMap.NET.MapProviders.GoogleMapProvider.Instance.GetRoute(start, end, alternative, false, 15);
            var pointsOnRoute = _customMapper.LatPointLatLongToArrayDecimal(route?.Points);
            return pointsOnRoute;
        }
        public List<decimal[]> getRouteGoogleApi(decimal[] positionFrom, decimal[] positionTo)
        {
            DirectionsRequest request = new DirectionsRequest();
            request.Key = _configuration.GetSection("ApikeyGoogleMaps:ApiKey").Value;
            request.Origin = new GoogleApi.Entities.Maps.Common.LocationEx(new CoordinateEx((double)positionFrom[0], (double)positionFrom[1]));
            request.Destination = new GoogleApi.Entities.Maps.Common.LocationEx(new CoordinateEx((double)positionTo[0], (double)positionTo[1]));
            request.Alternatives = true;
            var response = GoogleApi.GoogleMaps.Directions.Query(request);
            var route = response.Routes.ToList()[1];
            var pointsOnRoute = new List<decimal[]>();
            foreach (var leg in route.Legs)
            {
                foreach (var step in leg.Steps)
                {
                    pointsOnRoute.AddRange(_customMapper.CoordinateToArrayDecimal(step.PolyLine.Line.ToList()));
                }
            }
            return pointsOnRoute;
        }

        public decimal[] getPositionForPlace(string keyworks)
        {
            GoogleMapProvider.Instance.ApiKey = _configuration.GetSection("ApikeyGoogleMaps:ApiKey").Value;
            var status = new GeoCoderStatusCode();
            var point = GMap.NET.MapProviders.GoogleMapProvider.Instance.GetPoint(keyworks, out status);
            if (point == null) return null;
            return new decimal[] { (decimal)point.Value.Lat, (decimal)point.Value.Lng };         
        }
    }
}
