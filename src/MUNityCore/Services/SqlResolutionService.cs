using MUNityCore.DataHandlers.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityCore.Models.Resolution.V2;
using MUNityCore.Models.Resolution.SqlResa;
using Microsoft.EntityFrameworkCore;
using MUNity.Models.Resolution;

namespace MUNityCore.Services
{
    public class SqlResolutionService
    {
        private MunityContext _context;

        public SqlResolutionService(MunityContext context)
        {
            this._context = context;
        }

        public async Task CreatePublicResolutionAsync(string name)
        {
            var resolution = new ResaElement()
            {
                Topic = name,
                Name = name,
                FullName = name
            };

            this._context.Resolutions.Add(resolution);
            await this._context.SaveChangesAsync();
        }

        public async Task<bool> ChangeTopicAsync(string resolutionId, string topic
            )
        {
            var reso = await this._context.Resolutions.FirstOrDefaultAsync(n => n.ResaElementId == resolutionId);
            if (reso == null) return false;
            reso.Topic = topic;
            await this._context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> SetSubmitterNameAsync(string resolutionId, string newSubmitterName)
        {
            var reso = await this._context.Resolutions.FindAsync(resolutionId);
            if (reso == null) return false;
            reso.SubmitterName = newSubmitterName;
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetCommitteeNameAsync(string resolutionId, string v)
        {
            var reso = await this._context.Resolutions.FindAsync(resolutionId);
            if (reso == null) return false;
            reso.CommitteeName = v;
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddSupporterAsync(string resolutionId, string name)
        {
            var resolution = await this._context.Resolutions.FindAsync(resolutionId);
            if (resolution == null) return false;
            var supporterExist = await this._context.ResolutionSupporters.AnyAsync(n => n.Resolution.ResaElementId == resolutionId && n.Name == name);
            if (supporterExist) return false;
            var newSupporter = new ResaSupporter()
            {
                Name = name,
                Resolution = resolution
            };
            await this._context.ResolutionSupporters.AddAsync(newSupporter);
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveSupporterAsync(string resaSupporterId)
        {
            var supporter = await _context.ResolutionSupporters.FindAsync(resaSupporterId);
            if (supporter == null) return false;
            this._context.ResolutionSupporters.Remove(supporter);
            await this._context.SaveChangesAsync();
            return true;
        }

        public ResaPreambleParagraph CreatePreambleParagraph(string resolutionId, string text = "")
        {
            var resolution = this._context.Resolutions.Find(resolutionId);
            if (resolution == null) return null;

            var newParagraph = new ResaPreambleParagraph()
            {
                ResaElement = resolution,
                Text = text,
                OrderIndex = resolution.PreambleParagraphs?.Count ?? 0
            };
            this._context.PreambleParagraphs.Add(newParagraph);
            this._context.SaveChanges();
            return newParagraph;
        }

        public bool SetPreambleParagraphText(string resaPreambleParagraphId, string v)
        {
            var paragraph = this._context.PreambleParagraphs.Find(resaPreambleParagraphId);
            if (paragraph == null) return false;
            paragraph.Text = v;
            _context.SaveChanges();
            return true;
        }

        public bool SetPreambleParagraphComment(string resaPreambleParagraphId, string v)
        {
            var paragraph = this._context.PreambleParagraphs.Find(resaPreambleParagraphId);
            if (paragraph == null) return false;
            paragraph.Comment = v;
            _context.SaveChanges();
            return true;
        }

        public bool RemovePreambleParagraph(string resaPreambleParagraphId)
        {
            var paragraph = this._context.PreambleParagraphs.Find(resaPreambleParagraphId);
            if (paragraph == null) return true;
            this._context.PreambleParagraphs.Remove(paragraph);
            _context.SaveChanges();
            return true;
        }

        public bool ReorderPreamble(string resolutionId, List<string> list)
        {
            var isCountMatching = this._context.PreambleParagraphs.Count(n => n.ResaElement.ResaElementId == resolutionId) == list.Count;
            if (!isCountMatching) return false;
            bool errored = false;
            for (int i=0;i<list.Count;i++)
            {
                var element = this._context.PreambleParagraphs.Find(list[i]);
                if (element != null)
                {
                    element.OrderIndex = i;
                }
                else
                {
                    errored = true;
                }
            }

            if (!errored)
                _context.SaveChanges();
            return !errored;
        }

        public ResaOperativeParagraph CreateOperativeParagraph(string resolutionId, string text = "")
        {
            var resolution = this._context.Resolutions.Find(resolutionId);
            if (resolution == null) return null;
            var element = new ResaOperativeParagraph()
            {
                Resolution = resolution,
                OrderIndex = resolution.OperativeParagraphs?.Count ?? 0,
                Text = text
            };
            _context.OperativeParagraphs.Add(element);
            _context.SaveChanges();
            return element;
        }

        public bool SetOperativeParagraphText(string resaOperativeParagraphId, string v)
        {
            var paragraph = this._context.OperativeParagraphs.Find(resaOperativeParagraphId);
            if (paragraph == null) return false;
            paragraph.Text = v;
            _context.SaveChanges();
            return true;
        }

        public bool SetOperativeParagraphComment(string resaOperativeParagraphId, string v)
        {
            var paragraph = this._context.OperativeParagraphs.Find(resaOperativeParagraphId);
            if (paragraph == null) return false;
            paragraph.Comment = v;
            _context.SaveChanges();
            return true;
        }

        public ResaOperativeParagraph CreateSubOperativeParagraph(string parentId)
        {
            var parent = _context.OperativeParagraphs.Find(parentId);
            if (parent == null) return null;
            var newParagraph = new ResaOperativeParagraph()
            {
                Parent = parent,
                OrderIndex = parent.Children?.Count ?? 0,
                Resolution = parent.Resolution
            };
            _context.OperativeParagraphs.Add(newParagraph);
            _context.SaveChanges();
            return newParagraph;
        }

        public ResaDeleteAmendment CreateDeleteAmendment(string resaOperativeParagraphId, string submitterName)
        {
            var paragraph = this._context.OperativeParagraphs.Find(resaOperativeParagraphId);
            if (paragraph == null) return null;
            var amendment = new ResaDeleteAmendment()
            {
                TargetParagraph = paragraph,
                SubmitterName = submitterName,
                Resolution = paragraph.Resolution
            };
            this._context.DeleteAmendments.Add(amendment);
            this._context.SaveChanges();
            return amendment;
        }

        public ResaChangeAmendment CreateChangeAmendment(string resaOperativeParagraphId, string submitterName, string newText)
        {
            var paragraph = this._context.OperativeParagraphs.Find(resaOperativeParagraphId);
            if (paragraph == null) return null;
            var amendment = new ResaChangeAmendment()
            {
                TargetParagraph = paragraph,
                SubmitterName = submitterName,
                Resolution = paragraph.Resolution,
                NewText = newText
            };
            this._context.ChangeAmendments.Add(amendment);
            this._context.SaveChanges();
            return amendment;
        }

        public ResaMoveAmendment CreateMoveAmendment(string resaOperativeParagraphId, int position)
        {
            var targetParagraph = this._context.OperativeParagraphs.Find(resaOperativeParagraphId);
            if (targetParagraph == null) return null;
            var virtualParagraph = new ResaOperativeParagraph()
            {
                OrderIndex = position + 1,
                IsVirtual = true,
                IsLocked = true,
                Parent = targetParagraph.Parent,
                Text = targetParagraph.Text,
                Resolution = targetParagraph.Resolution
            };
            var addOrderIndexParagraphs = this._context.OperativeParagraphs.Where(n => n.OrderIndex > position
            && n.Resolution.ResaElementId == targetParagraph.Resolution.ResaElementId &&
            n.Parent == targetParagraph.Parent);
            foreach(var p in addOrderIndexParagraphs)
            {
                p.OrderIndex = p.OrderIndex + 1;
            }

            var amendment = new ResaMoveAmendment()
            {
                SourceParagraph = targetParagraph,
                VirtualParagraph = virtualParagraph,
                Resolution = targetParagraph.Resolution
            };
            this._context.MoveAmendments.Add(amendment);
            this._context.SaveChanges();
            return amendment;
        }

        /// <summary>
        /// Creates an add amendment for a paragraph to be added into the first level!
        /// </summary>
        /// <param name="resolutionId"></param>
        /// <param name="index"></param>
        /// <param name="submitter"></param>
        /// <param name="newText"></param>
        /// <returns></returns>
        public ResaAddAmendment CreateAddAmendment(string resolutionId, int index, string submitter, string newText)
        {
            var resolution = this._context.Resolutions.Find(resolutionId);
            if (resolution == null) return null;
            var virtualParagraph = new ResaOperativeParagraph()
            {
                OrderIndex = index,
                IsVirtual = true,
                IsLocked = true,
                Text = newText,
                Resolution = resolution,
            };

            var amendment = new ResaAddAmendment()
            {
                Resolution = resolution,
                SubmitterName = submitter,
                VirtualParagraph = virtualParagraph
            };

            var addOrderIndexParagraphs = this._context.OperativeParagraphs.Where(n => n.OrderIndex >= index
            && n.Resolution.ResaElementId == resolutionId &&
            n.Parent == null);
            foreach (var p in addOrderIndexParagraphs)
            {
                p.OrderIndex = p.OrderIndex + 1;
            }

            this._context.OperativeParagraphs.Add(virtualParagraph);
            this._context.AddAmendments.Add(amendment);
            this._context.SaveChanges();
            return amendment;
        }

        public Resolution GetResolutionDto(string resolutionId)
        {
            var resolutionDb = this._context.Resolutions.Find(resolutionId);
            if (resolutionDb == null) return null;
            var dto = new Resolution()
            {
                Date = resolutionDb.CreatedDate,
                Header = new ResolutionHeader()
                {
                    AgendaItem = resolutionDb.AgendaItem,
                    CommitteeName = resolutionDb.CommitteeName,
                    FullName = resolutionDb.FullName,
                    Name = resolutionDb.Name,
                    ResolutionHeaderId = resolutionDb.ResaElementId + "_header",
                    Session = resolutionDb.Session,
                    SubmitterName = resolutionDb.SubmitterName,
                    Topic = resolutionDb.Topic,
                    Supporters = resolutionDb.Supporters.Select(n => new ResolutionSupporter()
                    {
                        Name = n.Name,
                        ResolutionSupporterId = n.ResaSupporterId
                    }).ToList()
                },

                Preamble = new ResolutionPreamble()
                {
                    PreambleId = resolutionDb.ResaElementId + "_preamble",
                    Paragraphs = resolutionDb.PreambleParagraphs.OrderBy(n => n.OrderIndex).Select(n => new PreambleParagraph()
                    {
                        Comment = n.Comment,
                        Corrected = n.IsCorrected,
                        IsLocked = n.IsLocked,
                        PreambleParagraphId = n.ResaPreambleParagraphId,
                        Text = n.Text
                    }).ToList()
                },

                OperativeSection = new OperativeSection()
                {
                    OperativeSectionId = resolutionDb.ResaElementId + "_os",

                    AddAmendments = resolutionDb.Amendments.OfType<ResaAddAmendment>().Select(n => new AddAmendment()
                    {
                        Name = "AddAmendment",
                        Activated = n.Activated,
                        Id = n.ResaAmendmentId,
                        SubmitterName = n.SubmitterName,
                        TargetSectionId = n.VirtualParagraph.ResaOperativeParagraphId,
                        SubmitTime = n.SubmitTime,
                        Type = n.ResaAmendmentType
                    }).ToList(),

                    ChangeAmendments = resolutionDb.Amendments.OfType<ResaChangeAmendment>().Select(n => new ChangeAmendment()
                    {
                        Activated = n.Activated,
                        Id = n.ResaAmendmentId,
                        Name = "ChangeAmendment",
                        NewText = n.NewText,
                        SubmitterName = n.SubmitterName,
                        SubmitTime = n.SubmitTime,
                        TargetSectionId = n.TargetParagraph.ResaOperativeParagraphId,
                        Type = n.ResaAmendmentType
                    }).ToList(),

                    DeleteAmendments = resolutionDb.Amendments.OfType<ResaDeleteAmendment>().Select(n => new DeleteAmendment()
                    {
                        Activated = n.Activated,
                        Id = n.ResaAmendmentId,
                        Name = "DeleteAmendment",
                        SubmitterName = n.SubmitterName,
                        SubmitTime = n.SubmitTime,
                        TargetSectionId = n.TargetParagraph.ResaOperativeParagraphId,
                        Type = n.ResaAmendmentType
                    }).ToList(),

                    MoveAmendments = resolutionDb.Amendments.OfType<ResaMoveAmendment>().Select(n => new MoveAmendment()
                    {
                        Activated = n.Activated,
                        Id = n.ResaAmendmentId,
                        Name = "MoveAmendment",
                        NewTargetSectionId = n.VirtualParagraph.ResaOperativeParagraphId,
                        SubmitterName = n.SubmitterName,
                        SubmitTime = n.SubmitTime,
                        TargetSectionId = n.SourceParagraph.ResaOperativeParagraphId,
                        Type = n.ResaAmendmentType
                    }).ToList(),

                    Paragraphs = resolutionDb.OperativeParagraphs.Where(n => n.Parent == null).Select(n => new OperativeParagraph()
                    {
                        Comment = n.Comment,
                        Corrected = n.Corrected,
                        IsLocked = n.IsLocked,
                        IsVirtual = n.IsVirtual,
                        Name = n.Name,
                        OperativeParagraphId = n.ResaOperativeParagraphId,
                        Text = n.Text,
                        Visible = n.Visible,
                    }).ToList()
                }
            };
            return dto;
        }
    }
}
