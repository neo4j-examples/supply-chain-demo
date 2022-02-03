using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SpikeNeo4j.Controllers;
using SpikeNeo4j.Models;
using SpikeNeo4j.Models.Dtos;
using SpikeNeo4j.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpikeNeo4j.Tests.PruebasUnitarias
{
    [TestClass]
    public class UserControllerTests : BasePruebas
    {
        [TestMethod]
        public void LoguearseConDatosIncorrectosProduceUnErrorDeAutorizacion()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            var zoomLevel = 0;
            var mockUserRepository = new Mock<IUserRepository>();
            var miConfiguracion = new Dictionary<string, string>
            {
                {"AppSettings:Token","9N7uS2VazGmYOet3iXUwSgz733E40VL5" }
            };

            var configuration = new ConfigurationBuilder().AddInMemoryCollection(miConfiguracion).Build();
            var userAuthInfo = new UserAuthLoginDto() { Username = "neo4j", Password = "malpass" };
            mockUserRepository.Setup(x =>
            x.Login(It.IsAny<string>(), It.IsAny<string>()
            )).Returns((User)null);

            //prueba
            var controller = new UsersController(configuration, mockUserRepository.Object);
            var respuesta = controller.Login(userAuthInfo);
            var result = respuesta.Result as UnauthorizedObjectResult;

            //verficacion
            Assert.IsNotNull(respuesta);
            Assert.AreEqual("Invalid login attempt", result.Value);
            Assert.AreEqual(401, result.StatusCode);
        }

        [TestMethod]
        public void LoguearseConDatosCorrectosDevuelveUnToken()
        {
            //preparacion
            var mapper = ConfigurarAutoMapper();
            var zoomLevel = 0;
            var mockUserRepository = new Mock<IUserRepository>();
            var miConfiguracion = new Dictionary<string, string>
            {
                {"AppSettings:Token","9N7uS2VazGmYOet3iXUwSgz733E40VL5" }
            };

            var configuration = new ConfigurationBuilder().AddInMemoryCollection(miConfiguracion).Build();
            var userAuthInfo = new UserAuthLoginDto() { Username = "neo4j", Password = "neo4jpass" };
            mockUserRepository.Setup(x =>
            x.Login(It.IsAny<string>(), It.IsAny<string>()
            )).Returns(new User()
            {
                Username = "neo4j",
                Password = "neo4jpass",
                Id = 1
            });

            //prueba
            var controller = new UsersController(configuration, mockUserRepository.Object);
            var respuesta = controller.Login(userAuthInfo);
            var result = respuesta.Result as UnauthorizedObjectResult;

            //verficacion
            Assert.IsNotNull(respuesta.Value);
            Assert.IsNotNull(respuesta.Value.Token);
        }
    }

}
