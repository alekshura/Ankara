namespace Compentio.Ankara.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Compentio.Ankara.Models.Users;
    using Compentio.Ankara.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;


    [ApiController]
    [Authorize]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        /// <summary>
        /// Method returns Users from Database for list of departments.
        /// Example: /api/users?departmentIds=1,2,3.
        /// </summary>
        /// <param name="departmentIds">Id fof department for which to search users</param>
        /// <param name="active">For True method returns only active users</param>
        /// <returns></returns>
        [HttpGet("")]
        public async Task<IEnumerable<User>> GetUsers([FromQuery] IEnumerable<long> departmentIds, 
            [FromQuery] bool? active = false)
        {
            return await _usersService.GetUsers(departmentIds, active).ConfigureAwait(false);
        }

        /// <summary>
        /// Modifies user data in the application
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("")]
        public async Task<IActionResult> ModifyUser([FromBody] User user)
        {
            await _usersService.ModifyUser(user, User.Identity.Name).ConfigureAwait(false);
            return Ok();
        }

        /// <summary>
        /// Remove user from application
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("{userId}")]
        public async Task<IActionResult> RemoveUser([FromRoute] long userId)
        {
            await _usersService.RemoveUser(userId, User.Identity.Name).ConfigureAwait(false);
            return Ok();
        }

        /// <summary>
        /// Add user to the application
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("")]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            await _usersService.AddUser(user, User.Identity.Name).ConfigureAwait(false);
            return Ok();
        }

        [HttpGet("roles")]
        public async Task<IEnumerable<dynamic>> GetUsersRoles() => await _usersService.GetRoles().ConfigureAwait(false);
    }
}
