using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MUNity.Database.Context;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Conference.Roles;
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

        public ResaElement CreateResolutionForCommittee(string committeeId, int? roleId)
        {
            ConferenceDelegateRole role = null;

            if (roleId != null)
            {
                role = _context.Delegates.FirstOrDefault(n => n.RoleId == roleId.Value);
            }

            Committee committee = _context.Committees.FirstOrDefault(n => n.CommitteeId == committeeId);

            var element = new ResaElement()
            {
                Topic = "Neuer Resolutionsentwurf",
                SubmitterName = role?.RoleName ?? "-",
                SubmitterRole = role,
                CommitteeName = committee?.Name ?? "-",
            };
            var auth = new ResolutionAuth()
            {
                Resolution = element,
                Committee = committee,
                
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

        public ResaOperativeParagraph CreateOperativeParagraph([NotNull] ResaElement resolution, string text = "")
        {
            _context.Update(resolution);
            var operativeParagraph = new ResaOperativeParagraph()
            {
                Resolution = resolution,
                OrderIndex = _context.OperativeParagraphs.Count(n => n.Resolution.ResaElementId == resolution.ResaElementId),
                Text = text
            };
            resolution.OperativeParagraphs.Add(operativeParagraph);
            _context.OperativeParagraphs.Add(operativeParagraph);
            _context.SaveChanges();
            return operativeParagraph;
        }

        public ResaElement GetResolution(string resolutionId)
        {
            var resolution = _context.Resolutions
                .Include(n => n.SubmitterRole)
                .FirstOrDefault(n => n.ResaElementId == resolutionId);
            if (resolution == null)
            {
                return null;
            }
            resolution.PreambleParagraphs = GetPreambleParagraphs(resolutionId);
            resolution.OperativeParagraphs = GetOperativeParagraphs(resolution);
            return resolution;
        }

        public bool RemovePreambleParagraph(ResaPreambleParagraph paragraph)
        {
            _context.PreambleParagraphs.Remove(paragraph);
            foreach(var pAfter in _context.PreambleParagraphs.Where(n => n.ResolutionId == paragraph.ResolutionId && n.OrderIndex > paragraph.OrderIndex))
            {
                pAfter.OrderIndex--;
            }
            return _context.SaveChanges() > 0;
        }

        public void RemoveOperativeParagraph(ResaOperativeParagraph paragraph)
        {
            foreach (var child in paragraph.Children)
            {
                RemoveOperativeParagraph(child);
            }
            var addAmendemts = _context.ResolutionAddAmendments
                .Where(n => n.VirtualParagraph.ResaOperativeParagraphId == paragraph.ResaOperativeParagraphId)
                .ToList();

            if (paragraph.Resolution != null)
            {
                paragraph.Resolution.OperativeParagraphs.Remove(paragraph);
                if (addAmendemts.Count > 0)
                {
                    foreach (var addAmendment in addAmendemts)
                    {
                        paragraph.Resolution.AddAmendments.Remove(addAmendment);
                    }
                }
            }

            if (paragraph.Parent != null)
            {
                paragraph.Parent.Children.Remove(paragraph);
            }

            var deleteSupporters = paragraph.DeleteAmendments.SelectMany(n => n.Supporters);
            _context.ResolutionAmendmentsSupporters.RemoveRange(deleteSupporters);

            var changeSupporters = paragraph.ChangeAmendments.SelectMany(n => n.Supporters);
            _context.ResolutionAmendmentsSupporters.RemoveRange(changeSupporters);

            var moveSupporters = paragraph.MoveAmendments.SelectMany(n => n.Supporters);
            _context.ResolutionAmendmentsSupporters.RemoveRange(moveSupporters);

            _context.ResolutionMoveAmendments.RemoveRange(paragraph.MoveAmendments);
            _context.ResolutionDeleteAmendments.RemoveRange(paragraph.DeleteAmendments);
            _context.ResolutionChangeAmendments.RemoveRange(paragraph.ChangeAmendments);

            _context.RemoveRange(addAmendemts);
            _context.OperativeParagraphs.Remove(paragraph);
            _context.SaveChanges();

            paragraph.MoveAmendments?.Clear();
            paragraph.DeleteAmendments?.Clear();
            paragraph.ChangeAmendments?.Clear();
            paragraph.Children?.Clear();
        }

        public int UpdatePreambleParagraph(ResaPreambleParagraph paragraph)
        {
            _context.Update(paragraph);
            return _context.SaveChanges();
        }

        public int UpdateOperativeParagraph(ResaOperativeParagraph paragraph)
        {
            _context.Update(paragraph);
            return _context.SaveChanges();
        }

        public bool MovePreambleParagraphUp(ResaPreambleParagraph paragraph)
        {
            if (paragraph.OrderIndex == 0)
                return false;

            var moveDownElement = _context.PreambleParagraphs
                .FirstOrDefault(n => n.ResolutionId == paragraph.ResolutionId && 
                n.OrderIndex == paragraph.OrderIndex - 1);

            if (moveDownElement != null)
            {
                moveDownElement.OrderIndex++;
            }
            paragraph.OrderIndex = paragraph.OrderIndex - 1;
            _context.SaveChanges();
            return true;
        }

        public void MoveOperativeParagraphUp(ResaOperativeParagraph paragraph)
        {
            if (paragraph.OrderIndex == 0)
                return;

            var moveDownElement = _context.OperativeParagraphs
                .FirstOrDefault(n => n.Resolution.ResaElementId == paragraph.Resolution.ResaElementId &&
                n.Parent == paragraph.Parent &&
                n.OrderIndex == paragraph.OrderIndex - 1);

            if (moveDownElement != null)
            {
                moveDownElement.OrderIndex--;
            }
            paragraph.OrderIndex = paragraph.OrderIndex - 1;
            _context.SaveChanges();
        }

        public void AddChangeAmendment(ResaOperativeParagraph paragraph, int roleId, string newText)
        {
            _context.Update(paragraph);
            var changeAmendment = new ResaChangeAmendment()
            {
                NewText = newText,
                Resolution = paragraph.Resolution,
                Submitter = _context.Delegates.Find(roleId),
                TargetParagraph = paragraph
            };
            paragraph.ChangeAmendments.Add(changeAmendment);
            _context.ResolutionChangeAmendments.Add(changeAmendment);
            _context.SaveChanges();
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

        public void SupportAmendment(ResaAmendment amendment, int roleId)
        {
            _context.Update(amendment);
            var support = new ResaAmendmentSupporter()
            {
                Amendment = amendment,
                Role = _context.Delegates.Find(roleId),
                SupportTimestamp = DateTime.Now,
            };
            amendment.Supporters.Add(support);
            _context.SaveChanges();
        }

        public void RevokeDeleteAmendment(ResaDeleteAmendment amendment)
        {
            _context.Update(amendment);
            amendment.TargetParagraph.DeleteAmendments.Remove(amendment);
            _context.ResolutionDeleteAmendments.Remove(amendment);
            _context.SaveChanges();
        }

        public void SupportResolution(ResaElement resolution, int roleId)
        {
            _context.Update(resolution);
            var support = new ResaSupporter()
            {
                Role = _context.Delegates.Find(roleId),
                Resolution = resolution,
            };
            resolution.Supporters.Add(support);
            _context.ResolutionSupporters.Add(support);
            _context.SaveChanges();
        }

        public void RevokeSupport(ResaElement resolution, int roleId)
        {
            var support = resolution.Supporters.FirstOrDefault(n => n.Role.RoleId == roleId);
            if (support != null)
            {
                resolution.Supporters.Remove(support);
                _context.Update(support);
                _context.ResolutionSupporters.Remove(support);
                _context.SaveChanges();
            }


        }

        public void RevokeSupport(ResaAmendment amendment, int roleId)
        {
            _context.Update(amendment);
            var support = amendment.Supporters.FirstOrDefault(n => n.Role.RoleId == roleId);
            if (support == null)
            {
                support = _context.ResolutionAmendmentsSupporters.FirstOrDefault(n => n.Role.RoleId == roleId);
            }

            if (support != null)
            {
                amendment.Supporters.Remove(support);
                foreach(var toRemove in amendment.Supporters.Where(n => n.Role != null && n.Role.RoleId == roleId))
                {
                    amendment.Supporters.Remove(toRemove);
                }
                _context.SaveChanges();
            }

        }

        public void MoveOperativeParagraphDown(ResaOperativeParagraph paragraph)
        {
            var moveUpElement = _context.OperativeParagraphs
                .FirstOrDefault(n => n.Resolution.ResaElementId == paragraph.Resolution.ResaElementId &&
                n.Parent == paragraph.Parent &&
                n.OrderIndex == paragraph.OrderIndex + 1);

            if (moveUpElement != null)
            {
                moveUpElement.OrderIndex--;
            }
            paragraph.OrderIndex = paragraph.OrderIndex + 1;
            _context.SaveChanges();
        }

        public int UpdateResaElement(ResaElement element)
        {
            _context.Update(element);
            return _context.SaveChanges();
        }

        public void SubmitDeleteAmendment(ResaDeleteAmendment amendment)
        {
            // Remove
            this.RemoveOperativeParagraph(amendment.TargetParagraph);
        }

        public IList<ResaPreambleParagraph> GetPreambleParagraphs(string resolutionId)
        {
            return _context.PreambleParagraphs
                .Where(n => n.ResolutionId == resolutionId)
                .OrderBy(n => n.OrderIndex)
                .ToList();
        }

        public IList<ResaOperativeParagraph> GetOperativeParagraphs(ResaElement resolution, ResaOperativeParagraph parent = null)
        {
            var paragraphs = _context.OperativeParagraphs
                .Where(n => n.Resolution == resolution && n.Parent == parent)
                .Include(n => n.ChangeAmendments)
                .Include(n => n.MoveAmendments)
                .OrderBy(n => n.OrderIndex)
                .ToList();
            foreach (var paragraph in paragraphs)
            {
                paragraph.Resolution = resolution;
                paragraph.DeleteAmendments = GetDeleteAmendments(paragraph);
            }
            return paragraphs;
        }

        public IList<ResaDeleteAmendment> GetDeleteAmendments(ResaOperativeParagraph paragraph)
        {
            return paragraph.DeleteAmendments = _context.ResolutionDeleteAmendments
                .Where(n => n.TargetParagraph == paragraph)
                .Include(n => n.Submitter)
                .Include(n => n.Supporters).ThenInclude(n => n.Role)
                .ToList();
        }

        public void AddRemoveAmendment(ResaOperativeParagraph paragraph, int roleId)
        {
            _context.Update(paragraph);
            var amendment = new ResaDeleteAmendment()
            {
                Resolution = paragraph.Resolution,
                Submitter = _context.Delegates.Find(roleId),
                TargetParagraph = paragraph
            };
            paragraph.DeleteAmendments.Add(amendment);
            _context.SaveChanges();
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
