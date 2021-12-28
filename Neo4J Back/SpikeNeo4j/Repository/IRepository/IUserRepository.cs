using SpikeNeo4j.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Repository.IRepository
{
    public interface IUserRepository
    {
        User Login(string username, string password);
    }
}
