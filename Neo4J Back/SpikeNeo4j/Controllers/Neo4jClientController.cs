using AutoMapper;
using GMap.NET;
using GoogleApi.Entities.Maps.Directions.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Neo4jClient;
using SpikeNeo4j.Mapper.IMappers;
using SpikeNeo4j.Models;
using SpikeNeo4j.Models.Dto;
using SpikeNeo4j.Models.Dtos;
using SpikeNeo4j.Repository.IRepository;
using SpikeNeo4j.Services.IServices;
using System.Collections.Generic;
using System.Linq;

namespace SpikeNeo4j.Controllers
{
    //[Authorize] //descomentar para activar autenticacion por token
    [ApiController]
    [Route("api/neo4j")]
    public class Neo4jClientController : Controller
    {
        private readonly IClienteSpikeNeo4jRepository _clienteSpikeNeo4jRepository;
        private readonly IMapper _mapper;
        private readonly ICustomMappers _customMapper;
        private readonly IGmapService _gmapService;


        public Neo4jClientController(IClienteSpikeNeo4jRepository clienteSpikeNeo4jRepository, IMapper mapper, ICustomMappers customMapper, IGmapService gmapService)
        {
            _clienteSpikeNeo4jRepository = clienteSpikeNeo4jRepository;
            _mapper = mapper;
            _customMapper = customMapper;
            _gmapService = gmapService;
        }
       
        /// <summary>
        /// hhfghfghfghgfhfghfghfghfghfghfgh
        /// </summary>
        /// <param name="zoomLevel"></param>
        /// <returns></returns>
        [HttpGet("listdistributioncenters")]
        public ActionResult<IEnumerable<DistributionCenter>> ListDistributionCenters(int zoomLevel)
        {
            if (zoomLevel <= 0) return BadRequest("Zoom level must be greater than 0");
            var distributionCenters = _clienteSpikeNeo4jRepository.GetDistributionCenters(zoomLevel);
            return distributionCenters.ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="zoomLevel"></param>
        /// <returns></returns>
        [HttpGet("listdestinationcenters")]
        public ActionResult<IEnumerable<DestinationCenter>> ListDestinationCenters(int zoomLevel)
        {
            if (zoomLevel <= 0) return BadRequest("Zoom level must be greater than 0");
            var destinationCenters = _clienteSpikeNeo4jRepository.GetDestinationCenters(zoomLevel);
            return destinationCenters.ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="zoomLevel"></param>
        /// <returns></returns>
        [HttpGet("listcenters")]
        public ActionResult<IEnumerable<CenterItem>> ListCenters(int zoomLevel)
        {
            if (zoomLevel <= 0) return BadRequest("Zoom level must be greater than 0");
            var distributionCenters = _clienteSpikeNeo4jRepository.GetCenters(zoomLevel);
            return distributionCenters.ToList();
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="zoomLevel"></param>
       /// <returns></returns>
        [HttpGet("listcentersandrelations")]
        public ActionResult<DistributionAndDestinationCenterDto> ListCentersAndRelations(int zoomLevel)
        {
            if (zoomLevel <= 0) return BadRequest("Zoom level must be greater than 0");

            var distributionAndDestinationCenters = _clienteSpikeNeo4jRepository.GetCentersAndRelations(zoomLevel);
            return new DistributionAndDestinationCenterDto()
            {
                DistributionCenters = _mapper.Map<IEnumerable<DistributionCenter>, List<DistributionCenterDto>>(distributionAndDestinationCenters.Item1),
                DestinationCenters = _mapper.Map<IEnumerable<DestinationCenter>, List<DestinationCenterDto>>(distributionAndDestinationCenters.Item2),
                RelationShips= _mapper.Map<IEnumerable<RelationShipBetweenTwoItems>, List<RelationShipBetweenTwoCentersDto>>(distributionAndDestinationCenters.Item3)
            };           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idDistributionCenter"></param>
        /// <returns></returns>
        [HttpGet("distributioncenterdetails")]
        public ActionResult<DistributionCenterDetailsDto> DistributionCenterDetails(int idDistributionCenter)
        {
            if (idDistributionCenter <= 0) return BadRequest("Distribution center id must be greater than 0");
            if (!_clienteSpikeNeo4jRepository.ExistsDistributionCenter(idDistributionCenter)) return BadRequest("Distribution center not found");
            var distributionCenter = _clienteSpikeNeo4jRepository.GetDistributionCenterDetails(idDistributionCenter);
            var distributionCenterDetailsDto = _mapper.Map<DistributionCenterDetailsDto>(distributionCenter);
            return distributionCenterDetailsDto;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idDistributionCenter"></param>
        /// <returns></returns>
        [HttpGet("distributioncentertrucks")]
        public ActionResult<DistributionCenterTrucksDto> DistributionCenterTrucks(int idDistributionCenter)
        {
            if (idDistributionCenter <= 0) return BadRequest("Distribution center id must be greater than 0");
            if (!_clienteSpikeNeo4jRepository.ExistsDistributionCenter(idDistributionCenter)) return BadRequest("Distribution center not found");            
            var distributionCenterTrucksAndRelations = _clienteSpikeNeo4jRepository.GetDistributionCenterTrucksAndRelations(idDistributionCenter);         
            return new DistributionCenterTrucksDto()
            {
                DistributionCenter= _mapper.Map<DistributionCenter, DistributionCenterDto>(distributionCenterTrucksAndRelations.Item1),
                Trucks = _mapper.Map<IEnumerable<Truck>, List<DistributionCenterTruckDto>>(distributionCenterTrucksAndRelations.Item2),
                RelationShips = _mapper.Map<IEnumerable<RelationShipBetweenTwoItems>, List<RelationShipBetweenTwoCentersDto>>(distributionCenterTrucksAndRelations.Item3)
            };
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="idTruck"></param>
        /// <returns></returns>
        [HttpGet("distributioncentertruckonroute")]
        public ActionResult<DistributionCenterTruckOnRouteDto> DistributionCenterTruckOnRoute(string idTruck)
        {
            if (string.IsNullOrEmpty(idTruck)) return BadRequest("Truck if can not be null");
            if (!_clienteSpikeNeo4jRepository.ExistsTruck(idTruck)) return BadRequest("Truck not found");
            var distributionCenterTruckOnRoute = _clienteSpikeNeo4jRepository.GetOriginAndDestinationForTruck(idTruck);
            var distributionCenterTruckOnRouteDto = new DistributionCenterTruckOnRouteDto()
            {
                DistributionCenterOrigin = _mapper.Map<CenterItem, CenterItemDto>(distributionCenterTruckOnRoute?.DistributionCenterOrigin),
                Destination = _mapper.Map<CenterItem, CenterItemDto>(distributionCenterTruckOnRoute?.Destination),
                Truck= _mapper.Map<Truck, DistributionCenterTruckDto>(distributionCenterTruckOnRoute?.Truck),
                RelationShips= _mapper.Map<IEnumerable<RelationShipBetweenTwoItems>, List<RelationShipBetweenTwoCentersDto>>(distributionCenterTruckOnRoute?.RelationShips),
                Route=_customMapper.MapTruckRoute(distributionCenterTruckOnRoute)
            };
            return distributionCenterTruckOnRouteDto;
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idTruck"></param>
        /// <returns></returns>
        [HttpGet("incidentdetaisandsolutionfortruckonroute")]
        public ActionResult<IncidentDetailsAndSolutionOnRouteDto> IncidentDetaisAndSolutionForTruckOnRoute(string idTruck)
        {
            if (string.IsNullOrEmpty(idTruck)) return BadRequest("Truck if can not be null");
            if (!_clienteSpikeNeo4jRepository.ExistsTruck(idTruck)) return BadRequest("Truck not found");
            var incidentDetailsAndSolutionOnRoute = _clienteSpikeNeo4jRepository.GetIncidentDetaisAndSolutionForTruck(idTruck);
            if (incidentDetailsAndSolutionOnRoute==null) return BadRequest("No incidents on route");
            if (typeof(IncidentDetailsAndSolutionTruckSensorsOnRoute).IsInstanceOfType(incidentDetailsAndSolutionOnRoute))
            {
                var incidentDetailsAndSolutionOnRouteDto = _mapper.Map<IncidentDetailsAndSolutionTruckSensorsOnRouteDto>(incidentDetailsAndSolutionOnRoute);
                return incidentDetailsAndSolutionOnRouteDto;
            }
            else
            {                
                var incidentDetailsAndSolutionOnRouteDto = _mapper.Map<IncidentDetailsAndSolutionOnRouteDto>(incidentDetailsAndSolutionOnRoute);
                return incidentDetailsAndSolutionOnRouteDto;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idDistributionCenter"></param>
        /// <returns></returns>
        [HttpPost("distributioncentermagicplaces")]
        public ActionResult<DistributionCenterMagicPlacesDto> DistributionCenterMagicPlaces(MagicPlaceFeaturesSelectedForDistributionCenterDto featuresSelected)
        {
            if (featuresSelected.IdDistributionCenter <= 0) return BadRequest("Distribution center id must be greater than 0");
            if (!_clienteSpikeNeo4jRepository.ExistsDistributionCenter(featuresSelected.IdDistributionCenter)) return BadRequest("Distribution center not found");
            var features = featuresSelected.Features.Where(x=>x.Value).Select(x => x.Key).ToList();
            if (!features.Any()) return BadRequest("At least one feature must be selected");
            var distributionCenterMagicPlaces = _clienteSpikeNeo4jRepository.GetDistributionCenterMagicPlaces(featuresSelected.IdDistributionCenter, features);           
            return new DistributionCenterMagicPlacesDto()
            {
                DistributionCenter = _mapper.Map<DistributionCenter, DistributionCenterDto>(distributionCenterMagicPlaces.Item1),
                MagicPlaces = _mapper.Map<IEnumerable<MagicPlace>, List<MagicPlaceDto>>(distributionCenterMagicPlaces.Item2),
                RelationShips = _mapper.Map<IEnumerable<RelationShipBetweenTwoItems>, List<RelationShipBetweenTwoCentersDto>>(distributionCenterMagicPlaces.Item3)
            };
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idTruck"></param>
        /// <returns></returns>
        [HttpGet("getpointsonroutetrucktodestination")]
        public ActionResult<List<decimal[]>> PointsOnRouteTruckToDestination(string idTruck)
        {
            if (string.IsNullOrEmpty(idTruck)) return BadRequest("Truck if can not be null");
            if (!_clienteSpikeNeo4jRepository.ExistsTruck(idTruck)) return BadRequest("Truck not found");
            var destinationForTruckRelation = _clienteSpikeNeo4jRepository.GetDestinationForTruckRelation(idTruck);
            if (destinationForTruckRelation == null) return BadRequest("A problem ocurried trying to get route");
            var route=_gmapService.getPointsOnRoute(destinationForTruckRelation.PositionFrom, destinationForTruckRelation.PositionTo,false);
            if (!route.Any()) return BadRequest("No points on route");
            return route;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idTruck"></param>
        /// <returns></returns>
        [HttpGet("getpointsonroutetruckfromorigin")]
        public ActionResult<List<decimal[]>> PointsOnRouteTruckFromOrigin(string idTruck)
        {
            if (string.IsNullOrEmpty(idTruck)) return BadRequest("Truck id can not be null");
            if (!_clienteSpikeNeo4jRepository.ExistsTruck(idTruck)) return BadRequest("Truck not found");
            var originForTruckRelation = _clienteSpikeNeo4jRepository.GetOriginForTruckRelation(idTruck);
            if (originForTruckRelation == null) return BadRequest("A problem ocurried trying to get route");
            var route = _gmapService.getPointsOnRoute(originForTruckRelation.PositionFrom, originForTruckRelation.PositionTo,false);
            if (!route.Any()) return BadRequest("No points on route");
            return route;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idTruck"></param>
        /// <returns></returns>
        [HttpGet("getpointsforalternativeroutetrucktodestination")]
        public ActionResult<List<decimal[]>> GetPointsForAlternativeRouteTruckToDestination(string idTruck)
        {
            if (string.IsNullOrEmpty(idTruck)) return BadRequest("Truck if can not be null");
            if (!_clienteSpikeNeo4jRepository.ExistsTruck(idTruck)) return BadRequest("Truck not found");
            var destinationForTruckRelation = _clienteSpikeNeo4jRepository.GetDestinationForTruckRelation(idTruck);
            if (destinationForTruckRelation == null) return BadRequest("A problem ocurried trying to get route");
            var route = _gmapService.getPointsOnRoute(destinationForTruckRelation.PositionFrom, destinationForTruckRelation.PositionTo,true);
            return route;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getmagicplacefeatureslist")]
        public ActionResult<List<MagicPlaceFeature>> GetMagicPlaceFeaturesList()
        {         
            var magicPlaceFeaturesList = _clienteSpikeNeo4jRepository.GetMagicPlaceFeaturesList();
            return magicPlaceFeaturesList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idMagicPlace"></param>
        /// <returns></returns>
        [HttpGet("magicplacedetails")]
        public ActionResult<MagicPlaceDetailsDto> GetMagicPlaceDetails(int idMagicPlace)
        {
            if (idMagicPlace<0) return BadRequest("Magic place id must greater than 0");
            if (!_clienteSpikeNeo4jRepository.ExistsMagicPlace(idMagicPlace)) return BadRequest("Magic place not found");
            var magicPlace = _clienteSpikeNeo4jRepository.GetMagicPlaceDetails(idMagicPlace);
            var magicPlaceDto = _mapper.Map<MagicPlaceDetailsDto>(magicPlace);
            return magicPlaceDto;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getships")]
        public ActionResult<IEnumerable<Ship>> GetShips()
        {
            var ships = _clienteSpikeNeo4jRepository.GetShips();
            var shipsDto = _mapper.Map<IEnumerable<Ship>, List<ShipDto>>(ships);
            return ships.ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyworks"></param>
        /// <returns></returns>
        [HttpGet("searchlocation")]
        public ActionResult<decimal[]> SearchLocation(string keyworks)
        {
            if (string.IsNullOrEmpty(keyworks)) return BadRequest("Keyworks can not be null or empty");
            var point = _gmapService.getPositionForPlace(keyworks);
            return point;
        }

    }
}