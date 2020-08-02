namespace Compentio.Ankara.Controllers
{
    using System.Threading.Tasks;
    using Compentio.Ankara.Models.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;


    [ApiController]
    [Authorize]
    [Route("api/authorization")]
    public class AuthorizationController : ControllerBase
    {
        private readonly Services.IAuthorizationService _authorizationService;

        public AuthorizationController(Services.IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Method provides logon for user that is in Active directory. 
        /// It check for active user with AD user name in database
        /// </summary>
        /// <returns></returns>
        [HttpPost("logon")]
        [ProducesResponseType(typeof(User), 200)]
        public async Task<IActionResult> Logon()
        {
            var result = await _authorizationService.Logon(User.Identity.Name).ConfigureAwait(false);

           if (result == null)
           {
                return Forbid();
           }

           return Ok(result);
        }

        [HttpPost("logout")]
        [ProducesResponseType(typeof(IActionResult), 200)]
        public async Task<IActionResult> Logout()
        {
            await _authorizationService.Logout().ConfigureAwait(false);
            return Ok();
        }
    }
}
