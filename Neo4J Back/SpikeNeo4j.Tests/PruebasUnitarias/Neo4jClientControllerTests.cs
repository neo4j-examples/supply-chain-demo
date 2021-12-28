using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SpikeNeo4j.Controllers;
using SpikeNeo4j.Mapper.IMappers;
using SpikeNeo4j.Models;
using SpikeNeo4j.Models.Dto;
using SpikeNeo4j.Models.Dtos;
using SpikeNeo4j.Repository.IRepository;
using SpikeNeo4j.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpikeNeo4j.Tests.PruebasUnitarias
{
    [TestClass]
    public class Neo4jClientControllerTests : BasePruebas
    {
        [TestMethod]
        public void ListarCentrosYRelaciones()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            var zoomLevel = 1;
            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            (List<DistributionCenter>, List<DestinationCenter>, List<RelationShipBetweenTwoItems>) getCentersAndRelationsResponse =

                (new List<DistributionCenter>()
                {
                    new DistributionCenter(){Id=1,City="Madrid"},
                    new DistributionCenter(){Id=2,City="London"},
                    new DistributionCenter(){Id=3,City="NewYork"},
                },
                new List<DestinationCenter>()
                {
                    new DestinationCenter(){Id=4,City="LosAngeles"},
                    new DestinationCenter(){Id=5,City="LasVegas"},
                },
                new List<RelationShipBetweenTwoItems>()
                {
                    new RelationShipBetweenTwoItems(){From="Madrid",To="London"},
                    new RelationShipBetweenTwoItems(){From="Madrid",To="NewYork"},
                    new RelationShipBetweenTwoItems(){From="Madrid",To="LosAngeles"},
                    new RelationShipBetweenTwoItems(){From="London",To="LasVegas"},
                    new RelationShipBetweenTwoItems(){From="NewYork",To="LasVegas"},
                    new RelationShipBetweenTwoItems(){From="London",To="LosAngeles"},
                });
            
            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.GetCentersAndRelations(It.IsAny<int>()
            )).Returns(getCentersAndRelationsResponse);
            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);

            var respuesta = controller.ListCentersAndRelations(zoomLevel);
            var result = respuesta.Result as ObjectResult;

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual(3, respuesta.Value.DistributionCenters.Count());
            Assert.AreEqual(2, respuesta.Value.DestinationCenters.Count());
            Assert.AreEqual(6, respuesta.Value.RelationShips.Count());

        }

        [TestMethod]
        public void SiZoomLevelEsMenorQueCeroDevolverError()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            var zoomLevel = 0;
            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.ListCentersAndRelations(zoomLevel);
            var result = respuesta.Result as ObjectResult;

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("Zoom level must be greater than 0", result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }


        [TestMethod]
        public void ObtenerDetalleDeCentroDeDistribucion()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            var idDistributionCenter = 1;

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.ExistsDistributionCenter(It.IsAny<int>()
            )).Returns(true);

            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.GetDistributionCenterDetails(It.IsAny<int>()
            )).Returns(new DistributionCenter()
            {
                Id = 1,
                City = "Madrid",
                Country = "Spain",
                Position = new decimal[] { 23.77777m, 134.3343434m },

            });

            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.DistributionCenterDetails(idDistributionCenter);

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("1", respuesta.Value.Id);
            Assert.AreEqual("Madrid", respuesta.Value.City);
            Assert.AreEqual("Spain", respuesta.Value.Country);
        }


        [TestMethod]
        public void ObtenerDetalleDeCentroDeDistribucionConIdMenorqueCero()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            var idDistributionCenter = -1;

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.DistributionCenterDetails(idDistributionCenter);
            var result = respuesta.Result as ObjectResult;

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("Distribution center id must be greater than 0", result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void ObtenerDetalleDeCentroDeDistribucionQueNoExiste()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            var idDistributionCenter = 25;

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.ExistsDistributionCenter(It.IsAny<int>()
            )).Returns(false);

            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.DistributionCenterDetails(idDistributionCenter);
            var result = respuesta.Result as ObjectResult;

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("Distribution center not found", result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void ListarCamionesDeUnCentroDeDistribucion()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            var idDistributionCenter = 1;
            (DistributionCenter, List<Truck>, List<RelationShipBetweenTwoItems>) getDistributionCenterTrucksAndRelations =

                 (new DistributionCenter()
                 {
                     Id = 1,
                     City = "Madrid",
                     Country = "Spain",
                     Position = new decimal[] { 23.77777m, 134.3343434m },
                 },
                 new List<Truck>()
                {
                    new Truck(){Id="1421416P",SerialNumber="VXD5678753632"},
                    new Truck(){Id="1421417P",SerialNumber="VXD5678753633"}
                },
                 new List<RelationShipBetweenTwoItems>()
                {
                    new RelationShipBetweenTwoItems(){From="1",To="1421416P"},
                    new RelationShipBetweenTwoItems(){From="1",To="1421417P"},
                    new RelationShipBetweenTwoItems(){From="1421417P",To="1421417P"}
                });

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.ExistsDistributionCenter(It.IsAny<int>()
            )).Returns(true);

            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.GetDistributionCenterTrucksAndRelations(It.IsAny<int>()
             )).Returns(getDistributionCenterTrucksAndRelations);

            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.DistributionCenterTrucks(idDistributionCenter);

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("1", respuesta.Value.DistributionCenter.Id);
            Assert.AreEqual(2, respuesta.Value.Trucks.Count());
            Assert.AreEqual("1421416P", respuesta.Value.Trucks.First().Id);
            Assert.AreEqual(3, respuesta.Value.RelationShips.Count());
        }

        [TestMethod]
        public void ListarCamionesDeUnCentroDeDistribucionConIdMenorqueCero()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            var idDistributionCenter = -1;

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.DistributionCenterTrucks(idDistributionCenter);
            var result = respuesta.Result as ObjectResult;

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("Distribution center id must be greater than 0", result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void ListarCamionesDeUnCentroDeDistribucionQueNoExiste()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            var idDistributionCenter = 25;

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.ExistsDistributionCenter(It.IsAny<int>()
            )).Returns(false);

            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.DistributionCenterTrucks(idDistributionCenter);
            var result = respuesta.Result as ObjectResult;

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("Distribution center not found", result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void ObtenerDetallesDeRutaDeCamionDeUnCentroDeDistribucion()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            var idTruck = "1421416P";
            var getOriginAndDestinationForTruckResponse = new DistributionCenterTruckOnRoute()
            {
                DistributionCenterOrigin = new DistributionCenter()
                {
                    Id = 1,
                    City = "Madrid",
                    Country = "Spain",
                    Position = new decimal[] { 23.77777m, 134.3343434m },
                },
                Destination = new CenterItem()
                {
                    Id = 2,
                    City = "London",
                    Country = "UK",
                    Position = new decimal[] { 26.77777m, 140.3343434m },
                },
                Truck = new Truck()
                {
                    Id = "1421416P",
                    SerialNumber = "VXD5678753632"
                },
                RelationShips = new List<RelationShipBetweenTwoItems>()
                {
                    new RelationShipBetweenTwoItems(){From="1421416P",To="Madrid"},
                    new RelationShipBetweenTwoItems(){From="1421416P",To="London"},
                },
                DestinationRoute = new RouteProblem()
                {
                    //HasProblem = true,
                    Id = "1",
                    Description = "Traffic jam",
                    DetailedDescription = "Accident preventing meeting deadlines and/or perishable goods being deliverred in optimal conditions",
                    Solution = "Change the route to reach your destination on time"
                }
            };

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.ExistsTruck(It.IsAny<string>()
            )).Returns(true);

            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.GetOriginAndDestinationForTruck(It.IsAny<string>()
             )).Returns(getOriginAndDestinationForTruckResponse);

            var mockCustomMapper = new Mock<ICustomMappers>();
            mockCustomMapper.Setup(x =>
            x.MapTruckRoute(It.IsAny<DistributionCenterTruckOnRoute>()
             )).Returns(new TruckOnRoute()
             {
                 IdTruck = getOriginAndDestinationForTruckResponse.Truck?.Id,
                 SerialNumberTruck = getOriginAndDestinationForTruckResponse.Truck?.SerialNumber,
                 DistributionCenterAddress = getOriginAndDestinationForTruckResponse.DistributionCenterOrigin.Address,
                 DistributionCenterCountry = getOriginAndDestinationForTruckResponse.DistributionCenterOrigin.Country,
                 DistributionCenterCity = getOriginAndDestinationForTruckResponse.DistributionCenterOrigin.City,
                 DistributionCenterPhoneNumber = getOriginAndDestinationForTruckResponse.DistributionCenterOrigin.PhoneNumber,
                 OriginRoute = new RoutePoint()
                 {
                     Id = getOriginAndDestinationForTruckResponse.DistributionCenterOrigin.Id.ToString(),
                     Description = getOriginAndDestinationForTruckResponse.DistributionCenterOrigin.Description,
                     Address = getOriginAndDestinationForTruckResponse.DistributionCenterOrigin.Address,
                     City = getOriginAndDestinationForTruckResponse.DistributionCenterOrigin.City,
                     Country = getOriginAndDestinationForTruckResponse.DistributionCenterOrigin.Country,
                     DateAndTime = "Aug. 14, 2021 - 8:25AM",
                     Position = getOriginAndDestinationForTruckResponse.DistributionCenterOrigin.Position
                 },
                 DestinationRoute = new RoutePoint()
                 {
                     Id = getOriginAndDestinationForTruckResponse.Destination.Id.ToString(),
                     Description = getOriginAndDestinationForTruckResponse.Destination.Description,
                     Address = getOriginAndDestinationForTruckResponse.Destination.Address,
                     City = getOriginAndDestinationForTruckResponse.Destination.City,
                     Country = getOriginAndDestinationForTruckResponse.Destination.Country,
                     DateAndTime = "Aug. 14, 2021 - 15:45AM",
                     Position = getOriginAndDestinationForTruckResponse.Destination.Position
                 },
                 IncidentRoute = getOriginAndDestinationForTruckResponse.DestinationRoute!=null ? new RoutePoint()
                 {
                     Id = getOriginAndDestinationForTruckResponse.DestinationRoute.Id,
                     Description = getOriginAndDestinationForTruckResponse.DestinationRoute.Description,
                     Address = getOriginAndDestinationForTruckResponse.DestinationRoute.Address,
                     City = getOriginAndDestinationForTruckResponse.DestinationRoute.City,
                     Country = getOriginAndDestinationForTruckResponse.DestinationRoute.Country,
                     DateAndTime = getOriginAndDestinationForTruckResponse.DestinationRoute.DateTime
                 } : null
             });

            var mockGMapSercive = new Mock<IGmapService>();


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.DistributionCenterTruckOnRoute(idTruck);

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("1", respuesta.Value.DistributionCenterOrigin.Id);
            Assert.AreEqual("2", respuesta.Value.Destination.Id);
            Assert.AreEqual("1421416P", respuesta.Value.Truck.Id);
            Assert.AreEqual(2, respuesta.Value.RelationShips.Count());
            Assert.AreEqual("1", respuesta.Value.Route.OriginRoute.Id);
            Assert.AreEqual("Madrid", respuesta.Value.Route.OriginRoute.City);
            Assert.AreEqual("2", respuesta.Value.Route.DestinationRoute.Id);
            Assert.AreEqual("London", respuesta.Value.Route.DestinationRoute.City);
            Assert.IsNotNull(respuesta.Value.Route.IncidentRoute);
        }

        [TestMethod]
        public void ObtenerDetallesDeRutaDeCamionConIdNuloDeUnCentroDeDistribucion()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            string idTruck = null;

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();

            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.DistributionCenterTruckOnRoute(idTruck);
            var result = respuesta.Result as ObjectResult;

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("Truck if can not be null", result.Value);
            Assert.AreEqual(400, result.StatusCode);


        }


        [TestMethod]
        public void ObtenerDetallesDeRutaDeCamionQueNoExisteDeUnCentroDeDistribucion()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            string idTruck = "54534P";

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.ExistsTruck(It.IsAny<string>()
            )).Returns(false);
            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.DistributionCenterTruckOnRoute(idTruck);
            var result = respuesta.Result as ObjectResult;

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("Truck not found", result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }


        [TestMethod]
        public void ObtenerIncidenteYSolucionParaCamion()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            var idTruck = "1421416P";
            var getIncidentDetaisAndSolutionForTruckResponse = new IncidentDetailsAndSolutionOnRoute()
            {
                Id = "1",
                Description = "Traffic jam",
                DetailedDescription = "Accident preventing meeting deadlines and/or perishable goods being deliverred in optimal conditions",
                Solution = "Change the route to reach your destination on time"

            };

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.ExistsTruck(It.IsAny<string>()
            )).Returns(true);

            //mockClienteSpikeNeo4jRepository.Setup(x =>
            //x.HasTruckAProblem(It.IsAny<string>()
            //)).Returns(true);

            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.GetIncidentDetaisAndSolutionForTruck(It.IsAny<string>()
             )).Returns(getIncidentDetaisAndSolutionForTruckResponse);

            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();



            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.IncidentDetaisAndSolutionForTruckOnRoute(idTruck);

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("1", respuesta.Value.Id);
            Assert.AreEqual("Traffic jam", respuesta.Value.Description);
            Assert.AreEqual("Accident preventing meeting deadlines and/or perishable goods being deliverred in optimal conditions", respuesta.Value.DetailedDescription);
            Assert.AreEqual("Change the route to reach your destination on time", respuesta.Value.Solution);
        }

        [TestMethod]
        public void ObtenerIncidenteYSolucionParaCamionConIdNulo()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            string idTruck = null;

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();

            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.IncidentDetaisAndSolutionForTruckOnRoute(idTruck);
            var result = respuesta.Result as ObjectResult;

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("Truck if can not be null", result.Value);
            Assert.AreEqual(400, result.StatusCode);

        }


        [TestMethod]
        public void ObtenerIncidenteYSolucionParaCamionQueNoExiste()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            string idTruck = "54534P";

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.ExistsTruck(It.IsAny<string>()
            )).Returns(false);
            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.IncidentDetaisAndSolutionForTruckOnRoute(idTruck);
            var result = respuesta.Result as ObjectResult;

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("Truck not found", result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void ObtenerMagicPlacesParaCentroDeDistribucion()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            var idDistributionCenter = 1;
            var magicPlaceFeaturesSelectedForDistributionCenterDto = new MagicPlaceFeaturesSelectedForDistributionCenterDto()
            {
                IdDistributionCenter = 1,
                Features = new Dictionary<string, bool>()
            };

            magicPlaceFeaturesSelectedForDistributionCenterDto.Features.Add("1", true);
            magicPlaceFeaturesSelectedForDistributionCenterDto.Features.Add("2", false);

            (DistributionCenter, List<MagicPlace>, List<RelationShipBetweenTwoItems>) getDistributionCenterMagicPlacesResponse =

                (new DistributionCenter()
                {
                    Id = 1,
                    City = "Madrid",
                    Country = "Spain",
                    Position = new decimal[] { 23.77777m, 134.3343434m },
                },
                new List<MagicPlace>()
               {
                    new MagicPlace(){ Id = 1,Description = "Cypress Glen", Position = new decimal[] { 32.825179562976714m, -116.53082287142396m }},
                    new MagicPlace(){ Id = 2,Description = "Jamul", Position = new decimal[] { 33.098180211673146m, -116.99570494526313m }}
               },
                new List<RelationShipBetweenTwoItems>()
               {
                    new RelationShipBetweenTwoItems(){From="1",To="1"},
                    new RelationShipBetweenTwoItems(){From="1",To="2"},
               });

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.ExistsDistributionCenter(It.IsAny<int>()
            )).Returns(true);

            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.GetDistributionCenterMagicPlaces(It.IsAny<int>(), It.IsAny<List<string>>()
             )).Returns(getDistributionCenterMagicPlacesResponse);

            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();



            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.DistributionCenterMagicPlaces(magicPlaceFeaturesSelectedForDistributionCenterDto);

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual(2, respuesta.Value.MagicPlaces.Count());
            Assert.AreEqual(2, respuesta.Value.RelationShips.Count());
        }

        [TestMethod]
        public void ObtenerMagicPlacesParaCentroDeDistribucionConIdMenorqueCero()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            var magicPlaceFeaturesSelectedForDistributionCenterDto = new MagicPlaceFeaturesSelectedForDistributionCenterDto()
            {
                IdDistributionCenter = -1,
                Features = new Dictionary<string, bool>()
            };

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.DistributionCenterMagicPlaces(magicPlaceFeaturesSelectedForDistributionCenterDto);
            var result = respuesta.Result as ObjectResult;

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("Distribution center id must be greater than 0", result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void ObtenerMagicPlacesParaCentroDeDistribucionQueNoExiste()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            var magicPlaceFeaturesSelectedForDistributionCenterDto = new MagicPlaceFeaturesSelectedForDistributionCenterDto()
            {
                IdDistributionCenter = 25,
                Features = new Dictionary<string, bool>()
            };

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.ExistsDistributionCenter(It.IsAny<int>()
            )).Returns(false);

            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.DistributionCenterMagicPlaces(magicPlaceFeaturesSelectedForDistributionCenterDto);
            var result = respuesta.Result as ObjectResult;

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("Distribution center not found", result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void ListarCentrosDeDistribucion()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            var zoomLevel = 1;
            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.GetDistributionCenters(It.IsAny<int>()
            )).Returns(new List<DistributionCenter>()
            {
                    new DistributionCenter(){Id=1,City="Madrid"},
                    new DistributionCenter(){Id=2,City="London"},
                    new DistributionCenter(){Id=3,City="NewYork"},
            });

            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);

            var respuesta = controller.ListDistributionCenters(zoomLevel);

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual(3, respuesta.Value.Count());
            Assert.AreEqual(1, respuesta.Value.First().Id);
            Assert.AreEqual(3, respuesta.Value.Last().Id);

        }

        [TestMethod]
        public void ListarCentrosDeDistribucionSiZoomLevelEsMenorQueCeroDevolverError()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            var zoomLevel = 0;
            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.ListDistributionCenters(zoomLevel);
            var result = respuesta.Result as ObjectResult;

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("Zoom level must be greater than 0", result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void ListarCentrosDeDestino()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            var zoomLevel = 1;
            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.GetDestinationCenters(It.IsAny<int>()
            )).Returns(new List<DestinationCenter>()
            {
                    new DestinationCenter(){Id=1,City="Madrid"},
                    new DestinationCenter(){Id=2,City="London"},
                    new DestinationCenter(){Id=3,City="NewYork"},
            });

            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);

            var respuesta = controller.ListDestinationCenters(zoomLevel);

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual(3, respuesta.Value.Count());
            Assert.AreEqual(1, respuesta.Value.First().Id);
            Assert.AreEqual(3, respuesta.Value.Last().Id);

        }

        [TestMethod]
        public void ListarCentrosDeDestinoSiZoomLevelEsMenorQueCeroDevolverError()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            var zoomLevel = 0;
            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.ListDestinationCenters(zoomLevel);
            var result = respuesta.Result as ObjectResult;

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("Zoom level must be greater than 0", result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void ListarCentros()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            var zoomLevel = 1;
            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.GetCenters(It.IsAny<int>()
            )).Returns(new List<CenterItem>()
            {
                    new CenterItem(){Id=1,City="Madrid"},
                    new CenterItem(){Id=2,City="London"},
                    new CenterItem(){Id=3,City="NewYork"},
            });

            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);

            var respuesta = controller.ListCenters(zoomLevel);

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual(3, respuesta.Value.Count());
            Assert.AreEqual(1, respuesta.Value.First().Id);
            Assert.AreEqual(3, respuesta.Value.Last().Id);

        }

        [TestMethod]
        public void ListarCentrosSiZoomLevelEsMenorQueCeroDevolverError()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            var zoomLevel = 0;
            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.ListCenters(zoomLevel);
            var result = respuesta.Result as ObjectResult;

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("Zoom level must be greater than 0", result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void ObtenerPuntosEnRutaParaCamionHastaDestino()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            var idTruck = "1421415P";
            var getDestinationForTruckRelationResponse = new RelationShipBetweenTwoItems()
            {
                From = "1421415P",
                To = "LosAngeles",
                PositionFrom = new decimal[] { 32.825179562976714m, -116.53082287142396m },
                PositionTo = new decimal[] { 33.098180211673146m, -116.99570494526313m }
            };

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.ExistsTruck(It.IsAny<string>()
            )).Returns(true);

            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.GetDestinationForTruckRelation(It.IsAny<string>()
             )).Returns(getDestinationForTruckRelationResponse);

            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();

            mockGMapSercive.Setup(x =>
          x.getPointsOnRoute(It.IsAny<decimal[]>(), It.IsAny<decimal[]>(), It.IsAny<bool>()
           )).Returns(new List<decimal[]>()
           {
               new decimal[] { 32.825179562976714m, -116.53082287142396m },
               new decimal[] { 33.925179562976714m, -117.54082287142396m },
               new decimal[] { 34.525179562976714m, -118.53082287142396m },
               new decimal[] { 34.925179562976714m, -119.53082287142396m }
           });


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.PointsOnRouteTruckToDestination(idTruck);

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual(4, respuesta.Value.Count());
        }

        [TestMethod]
        public void ObtenerPuntosEnRutaParaCamionHastaDestinoConIdNulo()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            string idTruck = null;

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();

            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.PointsOnRouteTruckToDestination(idTruck);
            var result = respuesta.Result as ObjectResult;

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("Truck if can not be null", result.Value);
            Assert.AreEqual(400, result.StatusCode);

        }


        [TestMethod]
        public void ObtenerPuntosEnRutaParaCamionHastaDestinoQueNoExiste()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            string idTruck = "54534P";

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.ExistsTruck(It.IsAny<string>()
            )).Returns(false);
            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.PointsOnRouteTruckToDestination(idTruck);
            var result = respuesta.Result as ObjectResult;

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("Truck not found", result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void ObtenerPuntosEnRutaParaCamionHastaDestinoQueNoTieneRutaDeDestino()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            string idTruck = "54534P";

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.ExistsTruck(It.IsAny<string>()
            )).Returns(true);

            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.GetDestinationForTruckRelation(It.IsAny<string>()
             )).Returns((RelationShipBetweenTwoItems)null);
            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();

            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.PointsOnRouteTruckToDestination(idTruck);
            var result = respuesta.Result as ObjectResult;

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("A problem ocurried trying to get route", result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void ObtenerPuntosEnRutaParaCamionYElServicioGmapDevuelveUnaListaVacia()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            string idTruck = "54534P";

            var getDestinationForTruckRelationResponse = new RelationShipBetweenTwoItems()
            {
                From = "1421415P",
                To = "LosAngeles",
                PositionFrom = new decimal[] { 32.825179562976714m, -116.53082287142396m },
                PositionTo = new decimal[] { 33.098180211673146m, -116.99570494526313m }
            };

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.ExistsTruck(It.IsAny<string>()
            )).Returns(true);

            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.GetDestinationForTruckRelation(It.IsAny<string>()
             )).Returns(getDestinationForTruckRelationResponse);

            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();

            mockGMapSercive.Setup(x =>
          x.getPointsOnRoute(It.IsAny<decimal[]>(), It.IsAny<decimal[]>(), It.IsAny<bool>()
           )).Returns(new List<decimal[]>());

            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.PointsOnRouteTruckToDestination(idTruck);
            var result = respuesta.Result as ObjectResult;

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("No points on route", result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }


        [TestMethod]
        public void ObtenerPuntosEnRutaParaCamionDesdeOrigen()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            var idTruck = "1421415P";
            var getOriginForTruckRelationResponse = new RelationShipBetweenTwoItems()
            {
                From = "1421415P",
                To = "LosAngeles",
                PositionFrom = new decimal[] { 32.825179562976714m, -116.53082287142396m },
                PositionTo = new decimal[] { 33.098180211673146m, -116.99570494526313m }
            };

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.ExistsTruck(It.IsAny<string>()
            )).Returns(true);

            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.GetOriginForTruckRelation(It.IsAny<string>()
             )).Returns(getOriginForTruckRelationResponse);

            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();

            mockGMapSercive.Setup(x =>
          x.getPointsOnRoute(It.IsAny<decimal[]>(), It.IsAny<decimal[]>(), It.IsAny<bool>()
           )).Returns(new List<decimal[]>()
           {
               new decimal[] { 32.825179562976714m, -116.53082287142396m },
               new decimal[] { 33.925179562976714m, -117.54082287142396m },
               new decimal[] { 34.525179562976714m, -118.53082287142396m },
               new decimal[] { 34.925179562976714m, -119.53082287142396m }
           });


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.PointsOnRouteTruckFromOrigin(idTruck);

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual(4, respuesta.Value.Count());
        }

        [TestMethod]
        public void ObtenerPuntosEnRutaParaCamionDesdeOrigenConIdNulo()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            string idTruck = null;

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();

            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.PointsOnRouteTruckFromOrigin(idTruck);
            var result = respuesta.Result as ObjectResult;

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("Truck id can not be null", result.Value);
            Assert.AreEqual(400, result.StatusCode);

        }


        [TestMethod]
        public void ObtenerPuntosEnRutaParaCamionDesdeOrigenQueNoExiste()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            string idTruck = "54534P";

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.ExistsTruck(It.IsAny<string>()
            )).Returns(false);
            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.PointsOnRouteTruckFromOrigin(idTruck);
            var result = respuesta.Result as ObjectResult;

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("Truck not found", result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void ObtenerPuntosEnRutaParaCamionDesdeOrigenQueNoTieneRutaDeDestino()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            string idTruck = "54534P";

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.ExistsTruck(It.IsAny<string>()
            )).Returns(true);

            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.GetOriginForTruckRelation(It.IsAny<string>()
             )).Returns((RelationShipBetweenTwoItems)null);
            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();

            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.PointsOnRouteTruckFromOrigin(idTruck);
            var result = respuesta.Result as ObjectResult;

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("A problem ocurried trying to get route", result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void ObtenerPuntosEnRutaParaCamionDesdeOrigenYElServicioGmapDevuelveUnaListaVacia()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            string idTruck = "54534P";

            var getOriginForTruckRelationResponse = new RelationShipBetweenTwoItems()
            {
                From = "1421415P",
                To = "LosAngeles",
                PositionFrom = new decimal[] { 32.825179562976714m, -116.53082287142396m },
                PositionTo = new decimal[] { 33.098180211673146m, -116.99570494526313m }
            };

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.ExistsTruck(It.IsAny<string>()
            )).Returns(true);

            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.GetOriginForTruckRelation(It.IsAny<string>()
             )).Returns(getOriginForTruckRelationResponse);

            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();

            mockGMapSercive.Setup(x =>
          x.getPointsOnRoute(It.IsAny<decimal[]>(), It.IsAny<decimal[]>(), It.IsAny<bool>()
           )).Returns(new List<decimal[]>());

            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.PointsOnRouteTruckFromOrigin(idTruck);
            var result = respuesta.Result as ObjectResult;

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("No points on route", result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }


        [TestMethod]
        public void ObtenerShipsDevuelveUnaListaDeBarcos()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();

            var GetShipsResponse = new List<Ship>()
            {
                new Ship(){Id="1",Description="Ship1",Positions=new decimal[][] {new decimal[]{ 32.74454748492845m,-117.32849121093751m }, new decimal[] { 32.759562025650126m, -117.34222412109374m }} },
                new Ship(){Id="2",Description="Ship2",Positions=new decimal[][] {new decimal[]{ 33.35289977024569m, -118.06594848632812m }, new decimal[] { 33.35920864732515m, -118.05221557617188m } } }
            };

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
          
            mockClienteSpikeNeo4jRepository.Setup(x =>
            x.GetShips()).Returns(GetShipsResponse);

            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();          

            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.GetShips();

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual(2, respuesta.Value.Count());
            
        }

        [TestMethod]
        public void BuscarPorKeyWorksYObtenerUnPuntoGeografico()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            string keyworks = "madrid";

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();          
            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();
          
            mockGMapSercive.Setup(x =>
          x.getPositionForPlace(It.IsAny<string>())).Returns(
               new decimal[] { 40.4167754m,    -3.7037902m }            
           );


            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.SearchLocation(keyworks);

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual(40.4167754m, respuesta.Value[0]);
            Assert.AreEqual(-3.7037902m, respuesta.Value[1]);
        }

        [TestMethod]
        public void BuscarPorKeyWorksConKeyworksVaciaYObtenerMensajeDeError()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            string keyworks = "";

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();
           
            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.SearchLocation(keyworks);
            var result = respuesta.Result as ObjectResult;


            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("Keyworks can not be null or empty", result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }


        [TestMethod]
        public void BuscarPorKeyWorksConKeyworksNulaYObtenerMensajeDeError()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            string keyworks = null;

            var mockClienteSpikeNeo4jRepository = new Mock<IClienteSpikeNeo4jRepository>();
            var mockCustomMapper = new Mock<ICustomMappers>();
            var mockGMapSercive = new Mock<IGmapService>();

            //prueba
            var controller = new Neo4jClientController(mockClienteSpikeNeo4jRepository.Object, mapper, mockCustomMapper.Object, mockGMapSercive.Object);
            var respuesta = controller.SearchLocation(keyworks);
            var result = respuesta.Result as ObjectResult;


            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("Keyworks can not be null or empty", result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }

    }
}
