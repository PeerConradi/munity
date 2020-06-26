using MUNityAngular.Models.Resolution;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using MUNityAngular.DataHandlers.EntityFramework;
using Microsoft.IdentityModel.Tokens;
using MUNityAngular.Models.Conference;
using MUNityAngular.Models.Core;
using System.IdentityModel.Tokens;
using System.Text;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using MongoDB.Bson.IO;
using MUNityAngular.Models;
using MUNityAngular.Models.Resolution.V2;
using MUNityAngular.Schema.Request.Authentication;
using MUNityAngular.Schema.Response.Authentication;

namespace MUNityAngular.Services
{
    public class AuthService : IAuthService
    {
        private readonly MunCoreContext _context;
        private readonly MunityContext _munityContext;
        private readonly AppSettings _settings;

        public bool CanUserEditConference(User user, Conference conference)
        {
            return true;
        }

        public bool CanUserEditResolution(User user, ResolutionV2 resolution)
        {
            // Is user the owner

            // is user in the accepted group

            // is user participant inside the conference that this document is linked to

            // is user inside the committee that this document is lined to

            return true;
        }

        public bool CanCreateResolution(User user)
        {
            return true;
        }

        public User GetUserOfAuth(string authKey)
        {
            return null;
        }

        public AuthenticationResponse Authenticate(AuthenticateRequest model)
        {
            // that user does not exists go away!
            User user = _context.Users.FirstOrDefault(n => n.Username == model.Username);
            if (user == null)
                return null;

            var passwordCheck = Util.Hashing.PasswordHashing.CheckPassword(model.Password, user.Salt, user.Password);

            // Password is not correct fuck off
            if (!passwordCheck)
                return null;

            // authentication successful so generate jwt token
            var token = GenerateToken(user);
            
            return new AuthenticationResponse(user, token);
        }

        public string GenerateToken(User user)
        {
            var secureKey = Encoding.UTF8.GetBytes(_settings.Secret);
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secureKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public User GetUserOfClaimPrincipal(ClaimsPrincipal principal)
        {
            var claimUsername = principal.Claims.FirstOrDefault(n => n.Type == ClaimTypes.Name);
            if (claimUsername == null)
                return null;

            var username = claimUsername.Value;
            var user = _context.Users.FirstOrDefault(n => n.Username == username);
            return user;
        }


        public AuthService(MunCoreContext context, IOptions<AppSettings> appSettings)
        {
            _settings = appSettings.Value;
            _context = context;
        }
    }
}
