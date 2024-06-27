using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SQL_API.Context;
using SQL_API.Helper;
using SQL_API.Models;
using System.Data;

namespace SQL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtControlController : ControllerBase
    {
        private readonly JwtOptions _jwtOptions;
        private readonly ApplicationDbContext _Context;
        public JwtControlController(IOptions<JwtOptions> jwtOptions,ApplicationDbContext context)
        {
            _jwtOptions = jwtOptions.Value;
            _Context = context;
        }
        [HttpGet("{name}/{password}")]
        public IActionResult AuthCheck(string name, string password)
        {
            var user=Helpers.UserCheck(_Context, name, password);
            if(user.Count>0)
            {
                var token = Helpers.CreateToken(user[0],_jwtOptions);
                return Ok(token);
            }
            return BadRequest();
        }

        
    }
    
}
