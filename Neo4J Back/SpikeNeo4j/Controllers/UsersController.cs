using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SpikeNeo4j.Models;
using SpikeNeo4j.Models.Dtos;
using SpikeNeo4j.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SpikeNeo4j.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepo;

        public UsersController(IConfiguration config, IUserRepository userRepo)
        {           
            _config = config;
            _userRepo = userRepo;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usuarioAuthLoginDto"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public ActionResult<UserToken> Login(UserAuthLoginDto usuarioAuthLoginDto)
        {
            var userFromRepo = _userRepo.Login(usuarioAuthLoginDto.Username, usuarioAuthLoginDto.Password);

            if (userFromRepo == null)
            {
                return Unauthorized("Invalid login attempt");
            }

            return ConstruirToken(userFromRepo);
          
        }

        private UserToken ConstruirToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

          
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddYears(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiracion,
                signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = expiracion
            };

        }
    }
}
