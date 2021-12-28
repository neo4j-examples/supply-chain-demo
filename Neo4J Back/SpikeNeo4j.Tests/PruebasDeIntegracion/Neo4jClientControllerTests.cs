using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SpikeNeo4j.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpikeNeo4j.Tests.PruebasDeIntegracion
{
    [TestClass]
    public class Neo4jClientControllerTests : BasePruebas
    {
        private static readonly string url = "/api/neo4j";

        [TestMethod]
        public async Task ObtenerTodosLosCentrosDeDistribucion()
        {
            var factory = ConstruirWebApplicationFactory();

            var cliente = factory.CreateClient();
            var respuesta = cliente.GetAsync(url + "/listdistributioncenters?zoomLevel=1");

            respuesta.Result.EnsureSuccessStatusCode();

            var centrosDistribucion = JsonConvert
                .DeserializeObject<List<DistributionCenterDto>>(await respuesta.Result.Content.ReadAsStringAsync());

            Assert.AreEqual(34, centrosDistribucion.Count);
        }

        [TestMethod]
        public async Task ObtenerTodosLosCentrosDeDestino()
        {
            var factory = ConstruirWebApplicationFactory();

            var cliente = factory.CreateClient();
            var respuesta = cliente.GetAsync(url + "/listdestinationcenters?zoomLevel=1");

            respuesta.Result.EnsureSuccessStatusCode();

            var centrosDestino = JsonConvert
                .DeserializeObject<List<DestinationCenterDto>>(await respuesta.Result.Content.ReadAsStringAsync());

            Assert.AreEqual(11, centrosDestino.Count);
        }

    }
}
