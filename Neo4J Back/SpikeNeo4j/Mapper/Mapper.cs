using AutoMapper;
using SpikeNeo4j.Models;
using SpikeNeo4j.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Mapper
{
    public class Mappers : Profile
    {
        public Mappers()
        {
            CreateMap<DistributionCenter, DistributionCenterDto>().ReverseMap();
            CreateMap<DestinationCenter, DestinationCenterDto>().ReverseMap();
            CreateMap<RelationShipBetweenTwoItems, RelationShipBetweenTwoCentersDto>().ReverseMap();
            CreateMap<DistributionCenter, DistributionCenterDetailsDto>().ReverseMap();
            CreateMap<Truck, DistributionCenterTruckDto>().ReverseMap();
            CreateMap<CenterItem, CenterItemDto>().ReverseMap();
            CreateMap<TruckSensor, TruckSensorDto>().ReverseMap();
            CreateMap<IncidentDetailsAndSolutionTruckSensorsOnRoute, IncidentDetailsAndSolutionTruckSensorsOnRouteDto>().ForMember(dest => dest.SensorsState, opt => opt.MapFrom(src => src.SensorsState)).ReverseMap();
            CreateMap<IncidentDetailsAndSolutionOnRoute, IncidentDetailsAndSolutionOnRouteDto>().ReverseMap();
            CreateMap<MagicPlace, MagicPlaceDto>().ReverseMap();
            CreateMap<MagicPlace, MagicPlaceDetailsDto>().ReverseMap();
            CreateMap<Ship, ShipDto>().ReverseMap();
        }
    }
}
