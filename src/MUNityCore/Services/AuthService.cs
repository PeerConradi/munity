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
using System.Threading.Tasks;
using MUNityCore.DataHandlers.EntityFramework;
using MUNityCore.Models;
using MUNityCore.Models.Conference;
using MUNityCore.Models.User;
using MUNityCore.Models.Resolution.V2;
using MUNity.Schema.Authentication;
using MUNity.Schema.User;

namespace MUNityCore.Services
{
    public class AuthService : IAuthService
    {
        private readonly MunityContext _context;
        private readonly AppSettings _settings;

        public bool CanUserEditConference(MunityUser user, Conference conference)
        {
            var participations = _context.Participations.Where(n => n.Role.Conference == conference &&
                                                                    n.User == user);
            var list = participations.ToList();
            var canEdit = participations.Any(n => n.Role.RoleAuth.CanEditConferenceSettings);
            return canEdit;
        }


        public bool CanUserEditResolution(MunityUser user, MUNity.Models.Resolution.Resolution resolution)
        {
            // Is user the owner

            // is user in the accepted group

            // is user participant inside the conference that this document is linked to

            // is user inside the committee that this document is lined to

            return true;
        }

        public AuthenticationResponse Authenticate(MUNity.Schema.Authentication.AuthenticateRequest model)
        {
            // that user does not exists go away!
            MunityUser user = _context.Users.FirstOrDefault(n => n.UserName == model.Username);
            if (user == null)
                return null;

            var passwordCheck = Util.Hashing.PasswordHashing.CheckPassword(model.Password, "", user.PasswordHash);

            // Password is not correct fuck off
            if (!passwordCheck)
                return null;

            // authentication successful so generate jwt token
            var token = GenerateToken(user);
            
            return new AuthenticationResponse() { FirstName = user.Forename, LastName = user.Lastname, Username = user.UserName, Token = token};
        }

        public string GenerateToken(MunityUser user)
        {
            var secureKey = Encoding.UTF8.GetBytes(_settings.Secret);
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secureKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        

        public MunityUser GetUserOfClaimPrincipal(ClaimsPrincipal principal)
        {
            var claimUsername = principal.Claims.FirstOrDefault(n => n.Type == ClaimTypes.Name);
            if (claimUsername == null)
                return null;

            var username = claimUsername.Value;
            var user = _context.Users.FirstOrDefault(n => n.UserName == username);
            return user;
        }

        public Task<MunityUserAuth> GetAuth(int authid)
        {
            return this._context.UserAuths.FirstOrDefaultAsync(n => n.MunityUserAuthId == authid);
        }

        public IEnumerable<MunityUserAuth> GetAuths()
        {
            return this._context.UserAuths;
        }

        public Task<int> SetUserAuth(MunityUser user, MunityUserAuth auth)
        {
            user.Auth = auth;
            return this._context.SaveChangesAsync();
        }

        public MunityUser GetUserWithAuthByClaimPrincipal(ClaimsPrincipal principal)
        {
            var claimUsername = principal.Claims.FirstOrDefault(n => n.Type == ClaimTypes.Name);
            if (claimUsername == null)
                return null;

            var username = claimUsername.Value;
            var user = _context.Users.Include(n => n.Auth).FirstOrDefault(n => n.UserName == username);
            return user;
        }

        public bool IsUserPrincipalAdmin(ClaimsPrincipal principal)
        {
            var claimUsername = principal.Claims.FirstOrDefault(n => n.Type == ClaimTypes.Name);
            if (claimUsername == null) return false;

            var username = claimUsername.Value;
            var user = _context.Users.Include(n => n.Auth).FirstOrDefault(n => n.UserName == username);

            if (user.Auth == null) return false;

            return user.Auth.AuthLevel == EAuthLevel.Headadmin || user.Auth.AuthLevel == EAuthLevel.Admin;
        }

        public MunityUserAuth CreateAuth(string name)
        {
            var auth = new MunityUserAuth(name);
            this._context.UserAuths.Add(auth);
            this._context.SaveChanges();
            return auth;
        }

        public List<MUNityCore.Models.User.UserRole> Roles()
        {
            return this._context.Roles.ToList();
        }

        public bool CreateAdminRole()
        {
            var role = new MUNityCore.Models.User.UserRole();
            role.Name = "Admin";
            role.NormalizedName = "ADMIN";
            this._context.Roles.Add(role);
            return this._context.SaveChanges() == 1;
        }

        public MunityUser GetUser(string mail)
        {
            return this._context.Users.FirstOrDefault(n => n.NormalizedEmail == mail.ToUpper());
        }

        public AuthService(MunityContext context, IOptions<AppSettings> appSettings)
        {
            _settings = appSettings.Value;
            _context = context;
        }

        
    }
}
