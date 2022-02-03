using Neo4jClient;
using Newtonsoft.Json;
using SpikeNeo4j.Models;
using SpikeNeo4j.Models.Dtos;
using SpikeNeo4j.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4jClient.Cypher;
using Newtonsoft.Json.Linq;

namespace SpikeNeo4j.Repository
{
    public class ClienteSpikeNeo4jRepository : IClienteSpikeNeo4jRepository
    {
        private readonly IGraphClient _client;
        private readonly IProblemTypeRepository _problemTypeRepository;
        public ClienteSpikeNeo4jRepository(IGraphClient client, IProblemTypeRepository problemTypeRepository)
        {
            _client = client;
            _problemTypeRepository = problemTypeRepository;
        }

        public IEnumerable<DistributionCenter> GetDistributionCenters(int zoomLevel)
        {
            var distributionCenters = _client.Cypher.Match($"(m:{DistributionCenter.Labels})")
                .Where($"m.zoomLevel<=" + zoomLevel)
               .Return(m => m.As<DistributionCenter>()) 
               .ResultsAsync;
            return distributionCenters.Result;
        }

        public IEnumerable<DestinationCenter> GetDestinationCenters(int zoomLevel)
        {
            var destinationCenters = _client.Cypher.Match($"(m:{DestinationCenter.Labels})")
                .Where($"m.zoomLevel<=" + zoomLevel)
               .Return(m => m.As<DestinationCenter>()) 
               .ResultsAsync;
            return destinationCenters.Result;
        }

        public IEnumerable<CenterItem> GetCenters(int zoomLevel)
        {
            var destinationCenters = _client.Cypher.Match($"(m)")
               .Where($"(m: DistributionCenter OR m: DestinationCenter) AND m.zoomLevel<=" + zoomLevel)
               .Return(m => m.As<CenterItem>())
               .ResultsAsync;
            return destinationCenters.Result;
        }

        public (List<DistributionCenter>,List<DestinationCenter>,List<RelationShipBetweenTwoItems>) GetCentersAndRelations(int zoomLevel)
        {
            var results = _client.Cypher.Match($"(o:{DistributionCenter.Labels})-[]->(d)")
                .Where((DistributionCenter o, CenterItem d) => o.ZoomLevel <= zoomLevel && d.ZoomLevel <= zoomLevel)
                .Return((o, d) => new 
                {
                    Origen= o.As<DistributionCenter>(),
                    DestinoDistributionCenter = d.As<DistributionCenter>(),
                    DestinoDestinationCenter = d.As<DestinationCenter>(),
                    DestinoLabel=d.Labels()                             
                })
                .ResultsAsync;

            var distributionCenters = new List<DistributionCenter>();
            var destinationCenters = new List<DestinationCenter>();
            var relations = new List<RelationShipBetweenTwoItems>();

            foreach (var item in results.Result)
            {
                if (!distributionCenters.Any(x => x.Id == item.Origen.Id)) {
                    distributionCenters.Add(item.Origen);
                }

                if (item.DestinoLabel.Contains("DistributionCenter"))
                {
                    if (!distributionCenters.Any(x => x.Id == item.DestinoDistributionCenter.Id))
                    {
                        distributionCenters.Add(item.DestinoDistributionCenter);
                    }
                    relations.Add(new RelationShipBetweenTwoItems
                    {
                        From = item.Origen.City,
                        To = item.DestinoDistributionCenter.City,
                        PositionFrom = item.Origen.Position,
                        PositionTo = item.DestinoDistributionCenter.Position
                    });

                }
                else if (item.DestinoLabel.Contains("DestinationCenter"))
                {
                    if (!destinationCenters.Any(x => x.Id == item.DestinoDestinationCenter.Id))
                    {
                        destinationCenters.Add(item.DestinoDestinationCenter);
                    }
                    relations.Add(new RelationShipBetweenTwoItems
                    {
                        From = item.Origen.City,
                        To = item.DestinoDestinationCenter.City,
                        PositionFrom = item.Origen.Position,
                        PositionTo = item.DestinoDestinationCenter.Position
                    });
                }

            }
            return (distributionCenters, destinationCenters, relations);
        }

        public IEnumerable<RelationShipBetweenTwoItems> GetRelacionesCentros(int zoomLevel)
        {
            var results = _client.Cypher.Match($"(o:{DistributionCenter.Labels})-[]->(d)")
                .Where((DistributionCenter o, CenterItem d) => o.ZoomLevel <= zoomLevel && d.ZoomLevel <= zoomLevel)
                .Return((o, d) => new RelationShipBetweenTwoItems 
                {
                    From= o.As<DistributionCenter>().City,
                    To = d.As<DistributionCenter>().City,                  
                    PositionFrom= o.As<DistributionCenter>().Position,
                    PositionTo = d.As<DistributionCenter>().Position
                })
                .ResultsAsync;
            return results.Result;
        }

        public bool ExistsDistributionCenter(int id)
        {
            var distributionCenterQuery = _client.Cypher.Match($"(t:DistributionCenter)")
               .Where((DistributionCenter t) => t.Id == id)
               .Return(t => t.As<DistributionCenter>())
               .ResultsAsync;
            var distributionCenter = distributionCenterQuery.Result.SingleOrDefault();
            if (distributionCenter == null) return false;
            return true;
        }

        public DistributionCenter GetDistributionCenterDetails(int id)
        {
            var distributionCenterQuery = _client.Cypher.Match($"(m:DistributionCenter)")
               .Where($"m.id=" + id)
               .Return(m => m.As<DistributionCenter>())
               .ResultsAsync;
            var distributionCenter= distributionCenterQuery.Result.SingleOrDefault();
            if (distributionCenter != null) distributionCenter.Production = GetDistributionCenterProduction(id);
            return distributionCenter;
        }

        public List<ProductionPerYear> GetDistributionCenterProduction(int id)
        {

            return new List<ProductionPerYear>()
                {
                    new ProductionPerYear(){Year=2013,Production=20},
                    new ProductionPerYear(){Year=2014,Production=25},
                    new ProductionPerYear(){Year=2015,Production=40},
                    new ProductionPerYear(){Year=2016,Production=65},
                    new ProductionPerYear(){Year=2017,Production=50},
                    new ProductionPerYear(){Year=2018,Production=70},
                    new ProductionPerYear(){Year=2019,Production=80},
                    new ProductionPerYear(){Year=2020,Production=90}
                };
            
        }

        public (DistributionCenter,List<Truck>, List<RelationShipBetweenTwoItems>) GetDistributionCenterTrucksAndRelations(int id)
        {
            var distributionCenterTrucksQuery = _client.Cypher.Match($"(c:{DistributionCenter.Labels})<-[:{Relationships.RouteLegOrigin}]-(r:Route)<-[:{Relationships.TruckOnRoute}]-(t:Truck)")
              .Where((DestinationCenter c) => c.Id == id)
              .Return((c, t) => new 
               {
                   DistributionCenterOrigin = c.As<DistributionCenter>(),
                   Truck = t.As<Truck>()

               }).ResultsAsync;

            var distributionCenter = new DistributionCenter();
            var trucks = new List<Truck>();
            var relationships = new List<RelationShipBetweenTwoItems>();


            foreach (var item in distributionCenterTrucksQuery.Result)
            {
                distributionCenter = item.DistributionCenterOrigin;
                trucks.Add(item.Truck);
                relationships.Add(new RelationShipBetweenTwoItems
                {
                    From = item.DistributionCenterOrigin.Id.ToString(),
                    To = item.Truck.Id,
                    PositionFrom = item.DistributionCenterOrigin.Position,
                    PositionTo = item.Truck.Position
                });

                var RelationBetweenTrucksQuery = _client.Cypher.Match($"(t1:{Truck.Labels})-[r:{Relationships.TruckLinkedWithTruck}]->(t2:Truck)")
                .Where((Truck t1) => t1.Id == item.Truck.Id)
                .Return((t1, t2, r) => new
                {
                    T1 = t1.As<Truck>(),
                    T2 = t2.As<Truck>()
                }).ResultsAsync;

                foreach (var relationBetweenTrucks in RelationBetweenTrucksQuery.Result)
                {
                    relationships.Add(new RelationShipBetweenTwoItems
                    {
                        From = relationBetweenTrucks.T1.Id,
                        To = relationBetweenTrucks.T2.Id,
                        PositionFrom = relationBetweenTrucks.T1.Position,
                        PositionTo = relationBetweenTrucks.T2.Position
                    });

                }
            }

            return (distributionCenter,trucks,relationships);
        }

        public List<Truck> GetDistributionCenterTrucks(int id)
        {
            var distributionCenterTrucksQuery = _client.Cypher.Match($"(c:{DistributionCenter.Labels})<-[:{Relationships.RouteLegOrigin}]-(r:Route)<-[:{Relationships.TruckOnRoute}]-(t:Truck)")
               //.Where($"c.id=" + id)
               .Where((DestinationCenter c) => c.Id == id)
               .Return(t => t.As<Truck>())
               .ResultsAsync;
            var distributionCenterTrucks = distributionCenterTrucksQuery.Result.ToList();
            return distributionCenterTrucks;
        }

   
        public RelationShipBetweenTwoItems GetOriginForTruckRelation(string id)
        {
            var originAndDestinatioForTruckQuery = _client.Cypher.Match($"(c)<-[:{Relationships.RouteLegOrigin}]-(r:{Route.Labels})<-[:{Relationships.TruckOnRoute}]-(t:{Truck.Labels})")
               .Where((Truck t) => t.Id == id)
               .Return((c, t) => new RelationShipBetweenTwoItems
               {
                   From = t.As<Truck>().Id,
                   To = c.As<CenterItem>().City,
                   PositionFrom = t.As<Truck>().Position,
                   PositionTo = c.As<CenterItem>().Position                  
               })
               .ResultsAsync;
            var originAndDestinatioForTruck = originAndDestinatioForTruckQuery.Result.SingleOrDefault();
            return originAndDestinatioForTruck;
        }

        public RelationShipBetweenTwoItems GetDestinationForTruckRelation(string id)
        {
          
            var originAndDestinatioForTruckQuery = _client.Cypher.Match($"(c)<-[:{Relationships.RouteLegDestination}]-(r:{Route.Labels})<-[:{Relationships.TruckOnRoute}]-(t:{Truck.Labels})")
               .Where((Truck t) => t.Id == id)
               .Return((c, t) => new RelationShipBetweenTwoItems
               {
                   From = t.As<Truck>().Id,
                   To = c.As<CenterItem>().City,
                   PositionFrom = t.As<Truck>().Position,
                   PositionTo = c.As<CenterItem>().Position                   
               })
               .ResultsAsync;
            var originAndDestinatioForTruck = originAndDestinatioForTruckQuery.Result.SingleOrDefault();
            return originAndDestinatioForTruck;
        }

        public DistributionCenterTruckOnRoute GetOriginAndDestinationForTruck(string id)
        {

            var truckRouteQuery = _client.Cypher.Match($"(r:{Route.Labels})<-[:{Relationships.TruckOnRoute}]-(t:Truck)")
              .Where((Truck t) => t.Id == id)
              .Return((r, t) => new
              {
                  Route = r.As<Route>(),
                  Truck = t.As<Truck>()
              })
              .ResultsAsync;
            var truckRoute = truckRouteQuery.Result.SingleOrDefault();

            var originAndDestinatioForTruckQuery = _client.Cypher.Match($"(o)<-[:{Relationships.RouteLegOrigin}]-(r:{Route.Labels})-[:{Relationships.RouteLegDestination}]-(d)")
               .Where((Route r) => r.Id == truckRoute.Route.Id)
               .Return((o, d) => new DistributionCenterTruckOnRoute()
               {
                   DistributionCenterOrigin = o.As<CenterItem>(),
                   Destination = d.As<CenterItem>(),
               }).ResultsAsync;


            var originAndDestinatioForTruck = originAndDestinatioForTruckQuery.Result.SingleOrDefault();
            if (originAndDestinatioForTruck == null) return originAndDestinatioForTruck;
            originAndDestinatioForTruck.Truck = truckRoute.Truck;


            //buscamos problema en ruta
            var problemOnRouteQuery = _client.Cypher.Match($"(p:{RouteProblem.Labels})<-[:{Relationships.RouteHasProblem}]-(r:{Route.Labels})")
              .Where((Route r) => r.Id == truckRoute.Route.Id)
              .Return(p => p.As<RouteProblem>())
              .ResultsAsync;

            var problemOnRoute = problemOnRouteQuery.Result.SingleOrDefault();

            //si no hay problemas en ruta, miramos a ver si hay problemas en algun sensor del camion
            if (problemOnRoute == null)
            {
                var sensoresCamion = GetTruckSensors(id);
                var sensorConProblemas = sensoresCamion.FirstOrDefault(x => x.Failure && x.Type=="C");
                if (sensorConProblemas != null)
                {
                    var truckProblemType = _problemTypeRepository.GetTruckProblem();
                    originAndDestinatioForTruck.DestinationRoute = new RouteProblem();
                    originAndDestinatioForTruck.DestinationRoute.Id = truckProblemType.Id;
                    originAndDestinatioForTruck.DestinationRoute.Description = truckProblemType.Description;
                    originAndDestinatioForTruck.DestinationRoute.DetailedDescription = sensorConProblemas.ProblemDetailedDescription;
                    originAndDestinatioForTruck.DestinationRoute.Solution = sensorConProblemas.ProblemSolution;
                    //TODO: obtener direccion exacta donde se ha producido el fallo en el camion
                    originAndDestinatioForTruck.DestinationRoute.Address = "145 W Van Buren St.";
                    originAndDestinatioForTruck.DestinationRoute.City = "Phoneix";
                    originAndDestinatioForTruck.DestinationRoute.Country = "EEUU";
                    originAndDestinatioForTruck.DestinationRoute.DateTime = "Aug. 14, 2021 - 15:00AM";

                }
            }
            else
            {
                originAndDestinatioForTruck.DestinationRoute = new RouteProblem();
                originAndDestinatioForTruck.DestinationRoute.Id = problemOnRoute.Id;
                originAndDestinatioForTruck.DestinationRoute.Description = problemOnRoute.Description;
                originAndDestinatioForTruck.DestinationRoute.DetailedDescription = problemOnRoute.DetailedDescription;
                originAndDestinatioForTruck.DestinationRoute.Solution = problemOnRoute.Solution;
                originAndDestinatioForTruck.DestinationRoute.Position = problemOnRoute.Position;
                originAndDestinatioForTruck.DestinationRoute.Address = problemOnRoute.Address;
                originAndDestinatioForTruck.DestinationRoute.City = problemOnRoute.City;
                originAndDestinatioForTruck.DestinationRoute.Country = problemOnRoute.Country;
                originAndDestinatioForTruck.DestinationRoute.DateTime = problemOnRoute.DateTime;
            }

            originAndDestinatioForTruck.RelationShips = new List<RelationShipBetweenTwoItems>(){
                        new RelationShipBetweenTwoItems()
                        {
                           From = originAndDestinatioForTruck?.Truck?.Id,
                           To = originAndDestinatioForTruck?.DistributionCenterOrigin?.City,
                           PositionFrom = originAndDestinatioForTruck?.Truck?.Position,
                           PositionTo = originAndDestinatioForTruck?.DistributionCenterOrigin?.Position
                        },
                        new RelationShipBetweenTwoItems()
                        {
                           From = originAndDestinatioForTruck?.Truck?.Id,
                           To = originAndDestinatioForTruck?.Destination?.City,
                            PositionFrom = originAndDestinatioForTruck?.Truck?.Position,
                           PositionTo = originAndDestinatioForTruck?.Destination?.Position
                        }
             };
            return originAndDestinatioForTruck;
        }

        public bool ExistsTruck(string idTruck)
        {
            var truckQuery = _client.Cypher.Match($"(t:Truck)")
               .Where((Truck t) => t.Id == idTruck)
               .Return(t => t.As<Truck>())
               .ResultsAsync;
            var truck = truckQuery.Result.SingleOrDefault();
            if (truck == null) return false;
            return true;        
        }

        public Truck GetTruck(string idTruck)
        {
            var truckQuery = _client.Cypher.Match($"(t:Truck)")
               .Where((Truck t) => t.Id == idTruck)
               .Return(t => t.As<Truck>())
               .ResultsAsync;
            var truck = truckQuery.Result.SingleOrDefault();
            if (truck == null) return null;
            return truck;
        }

        public bool HasTruckAProblem(string idTruck)
        {
            var problemTruckQuery = _client.Cypher.Match($"(s)-[r1:{Relationships.SystemSensorTruck}]->(t:Truck)")
               .Where((Truck t,TruckSensor s) => t.Id == idTruck && s.Failure)
               .Return(t =>  t.As<Truck>())
               .ResultsAsync;
            var problemTruck = problemTruckQuery.Result.ToList();
            if (problemTruckQuery == null || !problemTruck.Any()) return false;
            return true;
        }

        public IncidentDetailsAndSolutionOnRoute GetIncidentDetaisAndSolutionForTruck(string id)
        {
            var incidentDetaisAndSolutionForTruckQuery = _client.Cypher.Match($"(t:Truck)-[:{Relationships.TruckOnRoute}]->(r:{Route.Labels})-[:{Relationships.RouteHasProblem}]->(p:{RouteProblem.Labels})")
               .Where((Truck t) => t.Id == id)
                .Return((r, t,p) => new
                {
                    Route = r.As<Route>(),
                    Truck = t.As<Truck>(),
                    Problem = p.As<RouteProblem>()
                })
               .ResultsAsync;           
            var incidentDetaisAndSolutionForTruck = incidentDetaisAndSolutionForTruckQuery.Result.SingleOrDefault();
            
            //si no hay problemas en ruta buscamos problemas en sensores de camion 
            if (incidentDetaisAndSolutionForTruck == null)
            {
                var sensoresCamion = GetTruckSensors(id);
                var sensorConProblemas = sensoresCamion.FirstOrDefault(x => x.Failure && x.Type=="C");
                if (sensorConProblemas != null)
                {
                    var truckProblemType = _problemTypeRepository.GetTruckProblem();
                    var truck = GetTruck(id);
                    return new IncidentDetailsAndSolutionTruckSensorsOnRoute()
                    {
                        Id = truckProblemType.Id,
                        Description = truckProblemType.Description,
                        DetailedDescription= sensorConProblemas.ProblemDetailedDescription,
                        Solution = sensorConProblemas.ProblemSolution,
                        SensorsState= sensoresCamion.Where(x=>x.Type=="C").ToList(),
                        SensorsInformationState= sensoresCamion.Where(x => x.Type == "I").ToList(),
                        Position =truck.Position
                    };
                }
                else
                {
                    return null;
                }
            }

            return new IncidentDetailsAndSolutionOnRoute()
            {
                Id = incidentDetaisAndSolutionForTruck.Problem.Id,
                Description = incidentDetaisAndSolutionForTruck.Problem.Description,
                DetailedDescription = incidentDetaisAndSolutionForTruck.Problem.DetailedDescription,
                Solution = incidentDetaisAndSolutionForTruck.Problem.Solution,
                Position=incidentDetaisAndSolutionForTruck.Problem.Position
            }; 
        }

        public List<TruckSensor> GetTruckSensors(string id)
        {
            var incidentDetaisAndSolutionForTruckQuery = _client.Cypher.Match($"(t:Truck)<-[r:{Relationships.SystemSensorTruck}]-(s)")
               .Where((Truck t, TruckSensor s) => t.Id == id)
               .Return(s => s.As<TruckSensor>())
               .ResultsAsync;            
            return incidentDetaisAndSolutionForTruckQuery.Result.ToList();
        }

        public List<MagicPlaceFeature> GetMagicPlaceFeaturesList()
        {
            var magicPlacesFeaturesQuery = _client.Cypher.Match($"(f:{MagicPlaceFeature.Labels})")
               .Return(f => f.As<MagicPlaceFeature>())
               .ResultsAsync;
            return magicPlacesFeaturesQuery.Result.ToList();
        }

        
        public (DistributionCenter, List<MagicPlace>, List<RelationShipBetweenTwoItems>) GetDistributionCenterMagicPlaces(int idDistributionCenter,List<string> features)
        {
            var distributionCenter = GetDistributionCenterDetails(idDistributionCenter);
            string p = "";
            foreach (var item in features) p += "'" + item + "',";
            p= p.Remove(p.Length - 1, 1);
            var magicPlacesQuery = _client.Cypher.Match($"(m:MagicPlace)<-[r]-(f)")
              .With($"m,f,count(r) as rels,point(" + "{latitude:" + distributionCenter.Position[0].ToString().Replace(",",".") + ", longitude:" + distributionCenter.Position[1].ToString().Replace(",", ".") + "}" + ") AS p1, point({latitude: m.position[0], longitude: m.position[1]}) AS p2")
              .Where($"distance(p2,p1)/1000<500 AND f.id in [" + p + "] AND f.value")
              .Return(m => m.As<MagicPlace>())
              .ResultsAsync;
            var magicPlaces = magicPlacesQuery.Result.ToList();
            var magicPlacesAgrupados = magicPlaces.GroupBy(x => x.Description);
            var magicPlacesSeleccionados = new List<MagicPlace>();
            foreach (var item in magicPlacesAgrupados)
            {
                if (item.ToList().Count() == features.Count())
                {
                    magicPlacesSeleccionados.Add(item.ToList()[0]);
                };
            }

            var relationShips=new List<RelationShipBetweenTwoItems>();
            foreach (var item in magicPlacesSeleccionados)
            {
                relationShips.Add(new RelationShipBetweenTwoItems()
                {
                    From = distributionCenter?.Id.ToString(),
                    To = item.Id.ToString(),
                    PositionFrom = distributionCenter?.Position,
                    PositionTo = item?.Position
                });
            }
            return (distributionCenter, magicPlacesSeleccionados, relationShips);                            
        }

        public bool ExistsMagicPlace(int id)
        {
            var magicPlaceQuery = _client.Cypher.Match($"(t:MagicPlace)")
               .Where((MagicPlace t) => t.Id == id)
               .Return(t => t.As<MagicPlace>())
               .ResultsAsync;
            var magicPlace = magicPlaceQuery.Result.SingleOrDefault();
            if (magicPlace == null) return false;
            return true;
        }

        public MagicPlace GetMagicPlaceDetails(int id)
        {
            var magicPlaceQuery = _client.Cypher.Match($"(t:{MagicPlace.Labels})")
              .Where((MagicPlace t) => t.Id == id)
              .Return(t => t.As<MagicPlace>())
              .ResultsAsync;
            var magicPlace = magicPlaceQuery.Result.SingleOrDefault();
            return magicPlace;
        }

        public IEnumerable<Ship> GetShips()
        {       
            var shipsQuery = _client.Cypher.Match($"(m:{Ship.Labels})")
             .With($"m,m.positions as p")
             .Return((m,p) => new
             {
                 Ship = m.As<Ship>(),
                 Positions=p.As<string>()
               })
             .ResultsAsync;

            var pp = shipsQuery.Result;
            foreach (var item in pp.ToList())
            {
                decimal[][] positions =    JsonConvert.DeserializeObject<decimal[][]>(item.Positions);
                item.Ship.Positions = positions;
            }

            return pp.Select(x => x.Ship).ToList();         
        }

    }
}
