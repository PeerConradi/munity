﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNityCore.Models.User;
using MUNity.Schema.User;
using MUNity.Database.Context;
using MUNity.Database.Models.User;
using MUNityBase;

namespace MUNity.Services
{
    public class UserService : IUserService
    {
        private readonly MunityContext _context;


        public Task<MunityUser> GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefaultAsync(n => n.UserName.ToLower() == username.ToLower());
        }

        public async Task<int> UpdateUser(MunityUser user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckUsername(string username)
        {
            return await _context.Users.AnyAsync(n => n.UserName.ToLower() == username);
        }

        public async Task<bool> CheckMail(string mail)
        {
            return await _context.Users.AnyAsync(n => n.Email == mail);
        }

        public IEnumerable<MunityUser> GetBannedUsers()
        {
            return this._context.Users.Where(n => n.UserState == MunityUser.EUserState.BANNED);
        }

        public List<MunityUser> GetUserBlock(int blockid)
        {
            return this._context.Users.AsNoTracking().OrderBy(n => n.Lastname).Skip(blockid).Take(100).ToList();
        }

        public List<MUNityCore.Dtos.Users.UserWithRolesDto> UsersWithRoles()
        {
            var users = from user in _context.Users
                        join roleIds in _context.UserRoles on user.Id equals roleIds.UserId
                        join roles in _context.Roles on roleIds.RoleId equals roles.Id
                        select new
                        {
                            UserId = user.Id,
                            Username = user.UserName,
                            RoleId = roles.Id,
                            RoleName = roles.Name
                        };
            var result = users.ToList().GroupBy(n => n.UserId).Select(n =>
            new MUNityCore.Dtos.Users.UserWithRolesDto()
            {
                UserId = n.Key,
                Username = n.First().Username,
                Roles = n.Select(a => new MUNityCore.Dtos.Users.MunityRoleDto()
                {
                    RoleId = a.RoleId,
                    Name = a.RoleName
                }).ToList()
            }).ToList();

            var noRolesUsers = from user in _context.Users
                               where _context.UserRoles.All(n => n.UserId != user.Id)
                               select new MUNityCore.Dtos.Users.UserWithRolesDto()
                               {
                                   Roles = new List<MUNityCore.Dtos.Users.MunityRoleDto>(),
                                   UserId = user.Id,
                                   Username = user.UserName
                               };
            result.AddRange(noRolesUsers);

            return result;
        }



        public Task<int> GetUserCount()
        {
            return this._context.Users.CountAsync();
        }

        public UserPrivacySettings GetUserPrivacySettings(MunityUser user)
        {
            if (user == null) return null;
            return this._context.Users.Include(n => n.PrivacySettings)
                .FirstOrDefault(n => n.Id == user.Id)
                ?.PrivacySettings;
        }

        public UserPrivacySettings InitUserPrivacySettings(MunityUser user)
        {
            if (user == null) return null;
            user.PrivacySettings = new UserPrivacySettings() {User = user};
            this._context.SaveChanges();
            return user.PrivacySettings;
        }

        public void UpdatePrivacySettings(UserPrivacySettings settings)
        {
            this._context.Add(settings);
            this._context.SaveChanges();
        }

        

        public void RemoveUser(MunityUser user)
        {
            this._context.Users.Remove(user);
            this._context.SaveChanges();
        }

        public bool BanUser(MunityUser user)
        {
            if (user == null) return false;
            user.UserState = MunityUser.EUserState.BANNED;
            this._context.SaveChanges();
            return true;
        }

        public IEnumerable<MunityUser> GetAdministrators()
        {
            return this._context.Users.Where(n =>
                n.Auth.AuthLevel == EAuthLevel.Admin || n.Auth.AuthLevel == EAuthLevel.Headadmin);
        }

        public async Task<UserInformation> GetUserInformation(string username)
        {
            var user = await _context.Users.Include(n => n.PrivacySettings).FirstOrDefaultAsync(n =>
                n.UserName == username);
            if (user == null) return null;
            throw new NotImplementedException();
            //return user.AsInformation();
            
        }

        public MunityUser GetUserById(string id)
        {
            return this._context.Users.FirstOrDefault(n => n.Id == id);
        }

        public UserService(MunityContext context)
        {
            _context = context;
        }
    }
}