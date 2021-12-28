using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using SpikeNeo4j.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SpikeNeo4j.Tests
{
    public class BasePruebas
    {
        protected IMapper ConfigurarAutoMapper()
        {
            var config = new MapperConfiguration(options =>
            {
                options.AddProfile(new Mappers());
            });
            return config.CreateMapper();
        }

        protected WebApplicationFactory<Startup> ConstruirWebApplicationFactory(bool ignorarSeguridad = true)
        {
            var factory = new WebApplicationFactory<Startup>();

            factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    //var descriptorDBContext = services.SingleOrDefault(d =>
                    //d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                    //if (descriptorDBContext != null)
                    //{
                    //    services.Remove(descriptorDBContext);
                    //}

                    //services.AddDbContext<ApplicationDbContext>(options =>
                    //options.UseInMemoryDatabase(nombreBD));

                    if (ignorarSeguridad)
                    {
                        services.AddSingleton<IAuthorizationHandler, AllowAnonymousHandler>();

                        services.AddControllers(options =>
                        {
                            //options.Filters.Add(new UsuarioFalsoFiltro());
                        });
                    }
                });
            });

            return factory;
        }

    }
}
