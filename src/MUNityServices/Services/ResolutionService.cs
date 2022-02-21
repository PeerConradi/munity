using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MUNity.Database.Context;
using MUNity.Database.Models.Resolution;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Services
{
    public class ResolutionService : IDisposable
    {
        private MunityContext _context;

        private ILogger<ResolutionService> _logger;

        public ResaElement CreateResolution()
        {
            var element = new ResaElement();
            _context.Resolutions.Add(element);
            _context.SaveChanges();
            _logger.LogInformation("New Resolution with id {0} created.", element.ResaElementId);
            return element;
        }

        public ResaElement CreateResolutionForCommittee(string committeeId)
        {
            var element = new ResaElement()
            {
                Topic = "Neuer Resolutionsentwurf"
            };
            var auth = new ResolutionAuth()
            {
                Resolution = element,
                Committee = _context.Committees.Find(committeeId),
                
            };
            _context.ResolutionAuths.Add(auth);
            _context.Resolutions.Add(element);
            _context.SaveChanges();
            _logger.LogInformation("New Resolution with id {0} for committee {1} created.", element.ResaElementId, committeeId);
            return element;
        }

        public ResaPreambleParagraph CreatePreambleParagraph([NotNull]ResaElement resolution, string text = "")
        {
            if (resolution == null)
                throw new ArgumentNullException("This method CreatePReambleParagraph received an resolution element null");

            _context.Update(resolution);

            var paragraph = new ResaPreambleParagraph()
            {
                ResaElement = resolution,
                OrderIndex = _context.PreambleParagraphs.Count(n => n.ResolutionId == resolution.ResaElementId),
                Text = text
            };
            if (resolution.PreambleParagraphs == null)
            {
                resolution.PreambleParagraphs = new List<ResaPreambleParagraph>();
            }
            resolution.PreambleParagraphs.Add(paragraph);
            _context.PreambleParagraphs.Add(paragraph);
            _context.SaveChanges();
            return paragraph;
        }

        public ResaElement GetResolution(string resolutionId)
        {
            var resolution = _context.Resolutions
                .FirstOrDefault(n => n.ResaElementId == resolutionId);
            if (resolution == null)
            {
                return null;
            }
            resolution.PreambleParagraphs = GetPreambleParagraphs(resolutionId);
            return resolution;
        }

        public bool RemovePreambleParagraph(ResaPreambleParagraph paragraph)
        {
            _context.PreambleParagraphs.Remove(paragraph);
            return _context.SaveChanges() == 1;
        }

        public int UpdatePreambleParagraph(ResaPreambleParagraph paragraph)
        {
            _context.Update(paragraph);
            return _context.SaveChanges();
        }

        public bool MovePreambleParagraphUp(ResaPreambleParagraph paragraph)
        {
            if (paragraph.OrderIndex == 0)
                return false;

            var moveDownElement = _context.PreambleParagraphs
                .FirstOrDefault(n => n.ResolutionId == paragraph.ResolutionId && n.OrderIndex == paragraph.OrderIndex - 1);

            if (moveDownElement != null)
            {
                moveDownElement.OrderIndex++;
            }
            paragraph.OrderIndex = paragraph.OrderIndex - 1;
            _context.SaveChanges();
            return true;
        }

        public bool MovePreambleParagraphDown(ResaPreambleParagraph paragraph)
        {
            if (paragraph.OrderIndex >= _context.PreambleParagraphs.Count(n => n.ResolutionId == paragraph.ResolutionId))
                return false;

            var moveUpElement = _context.PreambleParagraphs
                .FirstOrDefault(n => n.ResolutionId == paragraph.ResolutionId && n.OrderIndex == paragraph.OrderIndex + 1);

            if (moveUpElement != null)
            {
                moveUpElement.OrderIndex--;
            }
            paragraph.OrderIndex = paragraph.OrderIndex + 1;
            _context.SaveChanges();
            return true;
        }

        public int UpdateResaElement(ResaElement element)
        {
            _context.Update(element);
            return _context.SaveChanges();
        }

        public IList<ResaPreambleParagraph> GetPreambleParagraphs(string resolutionId)
        {
            return _context.PreambleParagraphs.Where(n => n.ResolutionId == resolutionId).OrderBy(n => n.OrderIndex).ToList();
        }

        public void Dispose()
        {
            this._context?.Dispose();
            this._context = null;
            _logger.LogDebug("A ResolutionService has been disposed!");
        }

        public ResolutionService(MunityContext context, ILogger<ResolutionService> logger)
        {
            this._context = context;
            this._logger = logger;
        }
    }
}
