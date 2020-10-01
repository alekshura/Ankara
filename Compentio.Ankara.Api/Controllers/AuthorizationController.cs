namespace Compentio.Ankara.Controllers
{
    using System.Linq;
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
            var login = User.Identity.Name.Split('\\').Last();

            var result = await _authorizationService.Logon(login).ConfigureAwait(false);

           if (result == null)
           {
                return Forbid();
           }

           return Ok(result.User);
        }
        /// <summary>
        /// Method used for user logout from current session
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        [ProducesResponseType(typeof(IActionResult), 200)]
        public async Task<IActionResult> Logout()
        {
            var login = User.Identity.Name.Split('\\').Last();
            await _authorizationService.Logout(login).ConfigureAwait(false);
            return Ok();
        }
    }
}
