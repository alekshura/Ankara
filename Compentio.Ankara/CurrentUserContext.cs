namespace Compentio.Ankara
{
    using Compentio.Ankara.Models.Users;
    using Microsoft.AspNetCore.Http;
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;

    public interface ICurrentUserContext
    {
        User CurrentUser { get; }
        void SetCurrentUser(User user);
        void Clear();
    }

    public class CurrentUserContext : ICurrentUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public User CurrentUser
        {
            get
            {
                var userToken = _httpContextAccessor.HttpContext.Request.Cookies["User-Token"];
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.ReadJwtToken(userToken);
                return JsonConvert.DeserializeObject<User>(token.Claims.FirstOrDefault(c => c.Type == "User").Value);
            }
        }

        public void SetCurrentUser(User user)
        {
            var token = GenerateJwtToken(user);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("User-Token", token);
        }

        public void Clear()
        {

        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("this is my custom Secret key for authentication");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                 {
                    new Claim("User", JsonConvert.SerializeObject(user))
                 }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
