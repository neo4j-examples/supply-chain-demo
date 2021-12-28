using Microsoft.Extensions.Configuration;
using SpikeNeo4j.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Repository.IRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public User Login(string username, string password)
        {
            var user=new User();
            user.Username = _configuration["Login:Username"];
            user.Password = _configuration["Login:Password"];
            if (username == user.Username && password == user.Password) return user;
            return null;        
        }
    }
}
