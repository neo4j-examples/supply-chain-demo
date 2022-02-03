using SpikeNeo4j.Models;
using SpikeNeo4j.Models.Dtos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Repository.IRepository
{
    public interface IClienteSpikeNeo4jRepository
    {
        IEnumerable<DistributionCenter> GetDistributionCenters(int zoomLevel);
        IEnumerable<DestinationCenter> GetDestinationCenters(int zoomLevel);
        //DistributionAndDestinationCenter GetCentersAndRelations(int zoomLevel);
        public (List<DistributionCenter>, List<DestinationCenter>, List<RelationShipBetweenTwoItems>) GetCentersAndRelations(int zoomLevel);

        IEnumerable<CenterItem> GetCenters(int zoomLevel);
        IEnumerable<RelationShipBetweenTwoItems> GetRelacionesCentros(int zoomLevel);
        DistributionCenter GetDistributionCenterDetails(int id);
        List<ProductionPerYear> GetDistributionCenterProduction(int id);
        List<Truck> GetDistributionCenterTrucks(int id);
        (DistributionCenter, List<Truck>, List<RelationShipBetweenTwoItems>) GetDistributionCenterTrucksAndRelations(int id);
        RelationShipBetweenTwoItems GetOriginForTruckRelation(string id);
        RelationShipBetweenTwoItems GetDestinationForTruckRelation(string id);
        DistributionCenterTruckOnRoute GetOriginAndDestinationForTruck(string id);
        bool ExistsTruck(string idTruck);
        bool ExistsDistributionCenter(int id);
        IncidentDetailsAndSolutionOnRoute GetIncidentDetaisAndSolutionForTruck(string id);
        (DistributionCenter, List<MagicPlace>, List<RelationShipBetweenTwoItems>) GetDistributionCenterMagicPlaces(int idDistributionCenter, List<string> features);
        bool HasTruckAProblem(string idTruck);
        List<MagicPlaceFeature> GetMagicPlaceFeaturesList();
        bool ExistsMagicPlace(int id);
        MagicPlace GetMagicPlaceDetails(int id);
        IEnumerable<Ship> GetShips();


    }
}
