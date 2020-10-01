namespace Compentio.Ankara.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Compentio.Ankara.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;


    [ApiController]
    [Authorize]
    [Route("api/session")]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }       

        /// <summary>
        /// Method indicates when there is active user session. In a case of actove session returns Http Forbidden Status Code: 403.  
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(IActionResult), 200)]
        public async Task<IActionResult> HasActiveSession()
        {
            var login = User.Identity.Name.Split('\\').Last();

            var result = await _sessionService.HasActiveSession(login).ConfigureAwait(false);

            if (result)
            {
                return Forbid();
            }

            return Ok();
        }
    }
}
