using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using Neo4j.Driver;
using Neo4jClient;
using SpikeNeo4j.Repository.IRepository;
using SpikeNeo4j.Repository;
using SpikeNeo4j.Mapper;
using SpikeNeo4j.Mapper.IMappers;
using SpikeNeo4j.Helpers.IMathHelpers;
using SpikeNeo4j.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SpikeNeo4j.Services.IServices;
using SpikeNeo4j.Services;

namespace SpikeNeo4j
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddSingleton(GetGraphClient);
            services.AddScoped<IProblemTypeRepository, ProblemTypeRepository>();
            services.AddScoped<IClienteSpikeNeo4jRepository, ClienteSpikeNeo4jRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICustomMappers, CustomMappers>();
            services.AddScoped<IMathHelper, MathHelper>();
            services.AddScoped<IGmapService, GmapService>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddAutoMapper(typeof(Mappers));
            services.AddControllers();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SpikeNeo4j", Version = "v1" });
            });
        }

        private IGraphClient GetGraphClient(IServiceProvider provider)
        {
            //Create our IGraphClient instance.
            var client = new BoltGraphClient(Configuration["Neo4j:Host"], Configuration["Neo4j:User"], Configuration["Neo4j:Pass"]);
            //We have to connect - as this is fully async, we need to 'Wait()'
            client.ConnectAsync().Wait();

            return client;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseCors(options=>
            {
                //options.WithOrigins("http://localhost:4200", "https://neo4j-dev.azurewebsites.net", "https://s3angularneo4j.s3.eu-central-1.amazonaws.com");
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SpikeNeo4j v1"));

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });           
        }
    }
}
