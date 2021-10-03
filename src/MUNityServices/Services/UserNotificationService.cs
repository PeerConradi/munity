using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MUNity.Database.Context;
using MUNity.Database.Models.User;
using MUNity.Schema.UserNotification;

namespace MUNity.Services
{
    public class UserNotificationService
    {
        private ILogger<UserNotificationService> _logger;

        private UserManager<MunityUser> _userManager;

        private MunityContext _munityContext;

        public async Task<List<UserNotificationItem>> GetLastFiveIntrestingNotifications(ClaimsPrincipal claim)
        {
            if (claim == null)
            {
                _logger.LogWarning($"Getting the last five notifications has been called with an invalid claim (null).");
                return new List<UserNotificationItem>();
            }

            var user = await _userManager.GetUserAsync(claim);
            if (user == null)
            {
                _logger.LogWarning($"Unable to find the user by the given claim {claim.Identity?.Name}");
                return new List<UserNotificationItem>();
            }

            var result = _munityContext.UserNotifications
                .Where(n => n.User.UserName == claim.Identity.Name && n.IsRead == false)
                .Include(n => n.Category)
                .OrderByDescending(n => n.Timestamp)
                .Take(5)
                .Select(n => new UserNotificationItem()
                {
                    Title = n.Title,
                    Timestamp = n.Timestamp,
                    CategoryId = n.Category.UserNotificationCategoryId,
                    CategoryName = n.Category.CategoryName,
                    NotificationId = n.UserNotificationId,
                    Text = n.Text
                })
                .ToList();

            if (result.Count < 5)
            {
                var readNotifications = _munityContext.UserNotifications
                    .Where(n => n.User.UserName == claim.Identity.Name && n.IsRead == true)
                    .Include(n => n.Category)
                    .OrderByDescending(n => n.Timestamp)
                    .Take(5 - result.Count)
                    .Select(n => new UserNotificationItem()
                    {
                        Title = n.Title,
                        Timestamp = n.Timestamp,
                        CategoryId = n.Category.UserNotificationCategoryId,
                        CategoryName = n.Category.CategoryName,
                        NotificationId = n.UserNotificationId,
                        Text = n.Text
                    })
                    .ToList();

                result.AddRange(readNotifications);
            }

            if (result.Count > 0)
                result = result.OrderByDescending(n => n.Timestamp).ToList();

            return result;
        }

        public async Task<int> GetCountOfUnreadNotifications(ClaimsPrincipal claim)
        {
            if (claim == null)
            {
                _logger.LogWarning($"GetCountOfUnreadNotifications was called with a claim null");
                return 0;
            }

            var user = await _userManager.GetUserAsync(claim);
            if (user == null)
            {
                _logger.LogWarning("GetCountOfUnreadNotifications the given user was not found");
                return 0;
            }

            return _munityContext.UserNotifications.Count(n =>
                n.User.UserName == claim.Identity.Name && n.IsRead == false);
        }

        public async Task<int> GetTotalCountOfNotifications(ClaimsPrincipal claim)
        {
            if (claim == null)
            {
                _logger.LogWarning($"GetTotalCountOfNotifications was called with a claim null");
                return 0;
            }

            var user = await _userManager.GetUserAsync(claim);
            if (user == null)
            {
                _logger.LogWarning("GetTotalCountOfNotifications the given user was not found");
                return 0;
            }

            return _munityContext.UserNotifications.Count(n =>
                n.User.UserName == claim.Identity.Name);
        }
        

        public UserNotificationService(UserManager<MunityUser> userManager, MunityContext munityContext, ILogger<UserNotificationService> logger)
        {
            _userManager = userManager;
            _munityContext = munityContext;
            _logger = logger;
        }
    }
}
