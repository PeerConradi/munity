using MUNityCore.Models.Resolution;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens;
using System.Text;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson.IO;
using System.Threading.Tasks;
using MUNityCore.DataHandlers.EntityFramework;
using MUNityCore.Models;
using MUNityCore.Models.Conference;
using MUNityCore.Models.Core;
using MUNityCore.Models.Resolution.V2;
using MUNityCore.Schema.Request;
using MUNityCore.Schema.Request.Authentication;
using MUNityCore.Schema.Response.Authentication;

namespace MUNityCore.Services
{
    public class AuthService : IAuthService
    {
        private readonly MunityContext _context;
        private readonly AppSettings _settings;

        public bool CanUserEditConference(User user, Conference conference)
        {
            var participations = _context.Participations.Where(n => n.Role.Conference == conference &&
                                                                    n.User == user);
            var list = participations.ToList();
            var canEdit = participations.Any(n => n.Role.RoleAuth.CanEditConferenceSettings);
            return canEdit;
        }


        public bool CanUserEditResolution(User user, ResolutionV2 resolution)
        {
            // Is user the owner

            // is user in the accepted group

            // is user participant inside the conference that this document is linked to

            // is user inside the committee that this document is lined to

            return true;
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

        public Task<UserAuth> GetAuth(int authid)
        {
            return this._context.UserAuths.FirstOrDefaultAsync(n => n.UserAuthId == authid);
        }

        public Task<int> SetUserAuth(User user, UserAuth auth)
        {
            user.Auth = auth;
            return this._context.SaveChangesAsync();
        }

        public User GetUserWithAuthByClaimPrincipal(ClaimsPrincipal principal)
        {
            var claimUsername = principal.Claims.FirstOrDefault(n => n.Type == ClaimTypes.Name);
            if (claimUsername == null)
                return null;

            var username = claimUsername.Value;
            var user = _context.Users.Include(n => n.Auth).FirstOrDefault(n => n.Username == username);
            return user;
        }

        public bool IsUserPrincipalAdmin(ClaimsPrincipal principal)
        {
            var claimUsername = principal.Claims.FirstOrDefault(n => n.Type == ClaimTypes.Name);
            if (claimUsername == null) return false;

            var username = claimUsername.Value;
            var user = _context.Users.Include(n => n.Auth).FirstOrDefault(n => n.Username == username);

            if (user.Auth == null) return false;

            return user.Auth.AuthLevel == UserAuth.EAuthLevel.Headadmin || user.Auth.AuthLevel == UserAuth.EAuthLevel.Admin;
        }

        public UserAuth CreateAuth(string name)
        {
            var auth = new UserAuth(name);
            this._context.UserAuths.Add(auth);
            this._context.SaveChanges();
            return auth;
        }

        public UserAuth CreateUserAuth(AdminSchema.CreateUserAuthBody request)
        {
            var auth = new UserAuth(request);
            this._context.UserAuths.Add(auth);
            return auth;
        }

        public AuthService(MunityContext context, IOptions<AppSettings> appSettings)
        {
            _settings = appSettings.Value;
            _context = context;
        }

        
    }
}
