using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PWS.DAL.Models;

namespace PWS.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "PingOne")]
    public class UserController : ControllerBase
    {
        private PWSContext _pwsContext;
        private readonly ILogger<UserController> _logger;
        public UserController(PWSContext pwsContext, ILogger<UserController> logger)
        {
            _pwsContext = pwsContext;
            _logger = logger;
            _logger.LogDebug("Nlog injected to user controller.");

        }

        [HttpGet]
        [Route("get")]
        public List<UserDetails> GetUsers()
        {
            
            return _pwsContext.UserDetails.ToList();
        }

        [HttpPost]
        [Route("add/{username}")]
        public IActionResult AddNewUser([FromRoute] string username)
        {
            UserDetails user = new UserDetails()
            {
                IsActive = true,
                Username = username
            };
            _pwsContext.UserDetails.Add(user);
            return Ok(_pwsContext.SaveChanges() == 1);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteUser([FromRoute] int id)
        {
            var user = _pwsContext.UserDetails.Select(x => x).Where(x => x.Id == id).SingleOrDefault();
            _pwsContext.UserDetails.Remove(user);
            return Ok(_pwsContext.SaveChanges() == 1);
        }

        [HttpPut]
        [Route("update-status/{id}")]
        public IActionResult UpdateStatus([FromRoute] int id)
        {
            var user = _pwsContext.UserDetails.Select(x => x).Where(x => x.Id == id).SingleOrDefault();
            user.IsActive = !user.IsActive;
            _pwsContext.UserDetails.Update(user);
            return Ok(_pwsContext.SaveChanges() == 1);
        }
    }
}
