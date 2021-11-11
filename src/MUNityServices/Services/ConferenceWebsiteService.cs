using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Database.Context;
using MUNity.Schema.Conference.Website;

namespace MUNity.Services
{
    public class ConferenceWebsiteService
    {
        private MunityContext _context;

        public List<MenuItem> GetMenuItems(string conferenceId)
        {
            return _context.ConferenceWebMenuEntries
                .Where(n => n.Conference.ConferenceId == conferenceId && n.Parent == null)
                .Select(n => new MenuItem()
                {
                    Id = n.ConferenceWebMenuEntryId,
                    PageId = n.TargetedPage.ConferenceWebPageId,
                    Title = n.Title,
                    Items = n.ChildEntries.Select(a => new MenuItem()
                    {
                        Id = a.ConferenceWebMenuEntryId,
                        PageId = a.TargetedPage.ConferenceWebPageId,
                        Title = a.Title
                    }).ToList()
                }).ToList();
        }

        public CreatedPageResult AddPage(string conferenceId, int? parentItemId)
        {
            MUNity.Database.Models.Website.ConferenceWebMenuEntry parent = null;
            if (parentItemId != null)
                parent = _context.ConferenceWebMenuEntries.Find(parentItemId);

            var page = new MUNity.Database.Models.Website.ConferenceWebPage()
            {
                Conference = _context.Conferences.Find(conferenceId),
                CreationDate = DateTime.Now,
                IsIndexPage = false,
                LastUpdateDate = DateTime.Now,
                Title = "New Page"
            };

            var menuItem = new MUNity.Database.Models.Website.ConferenceWebMenuEntry()
            {
                Conference = _context.Conferences.Find(conferenceId),
                Parent = parent,
                TargetedPage = page,
                Title = "New Page"
            };

            _context.ConferenceWebMenuEntries.Add(menuItem);
            _context.ConferenceWebPages.Add(page);
            var recaff = _context.SaveChanges();

            var result = new CreatedPageResult();
            if (recaff > 0)
            {
                result.Success = true;
                result.PageId = page.ConferenceWebPageId;
                result.MenuItemId = menuItem.ConferenceWebMenuEntryId;
            }

            return result;
        }

        public bool RenameMenuEntry(int menuEntryId, string newName)
        {
            var entry = _context.ConferenceWebMenuEntries.Find(menuEntryId);
            if (entry == null)
                return false;

            entry.Title = newName;
            _context.SaveChanges();
            return true;
        }

        public ConferenceWebsiteService(MunityContext context)
        {
            _context = context;
        }
    }
}
