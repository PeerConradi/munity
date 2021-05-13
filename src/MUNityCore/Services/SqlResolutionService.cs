using MUNityCore.DataHandlers.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityCore.Models.Resolution.V2;
using MUNityCore.Models.Resolution.SqlResa;
using Microsoft.EntityFrameworkCore;
using MUNity.Models.Resolution;
using MUNity.Schema.Simulation;
using MUNity.Schema.Simulation.Resolution;
using MUNityCore.Extensions.CastExtensions;

namespace MUNityCore.Services
{
    public class SqlResolutionService
    {
        private MunityContext _context;

        public SqlResolutionService(MunityContext context)
        {
            this._context = context;
        }

        public async Task<string> CreatePublicResolutionAsync(string name)
        {
            var resolution = new ResaElement()
            {
                Topic = name,
                Name = name,
                FullName = name
            };

            this._context.Resolutions.Add(resolution);
            await this._context.SaveChangesAsync();
            return resolution.ResaElementId;
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

        public async Task<bool> SetNameAsync(string resolutionId, string name)
        {
            var reso = await this._context.Resolutions.FirstOrDefaultAsync(n => n.ResaElementId == resolutionId);
            if (reso == null) return false;
            reso.Name = name;
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetFullNameAsync(string resolutionId, string fullName)
        {
            var reso = await this._context.Resolutions.FirstOrDefaultAsync(n => n.ResaElementId == resolutionId);
            if (reso == null) return false;
            reso.FullName = fullName;
            await this._context.SaveChangesAsync();
            return true;
        }

        internal bool RemoveChangeAmendment(string amendmentId)
        {
            var amendment = _context.ChangeAmendments.Find(amendmentId);
            if (amendment != null)
            {
                _context.ChangeAmendments.Remove(amendment);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        internal bool SubmitDeleteAmendment(string amendmentId)
        {
            var amendment = _context.DeleteAmendments.Include(n => n.TargetParagraph)
                .FirstOrDefault(n => n.ResaAmendmentId == amendmentId);
            if (amendment?.TargetParagraph != null)
            {
                var moveAmendmentsToRemove = _context.MoveAmendments
                    .Where(n => n.SourceParagraph.ResaOperativeParagraphId == amendment.TargetParagraph.ResaOperativeParagraphId);
                moveAmendmentsToRemove.ForEachAsync(n => RemoveMoveAmendment(n.ResaAmendmentId));
                
                var deleteAmendmentsToRemove = _context.DeleteAmendments
                    .Where(n => n.TargetParagraph.ResaOperativeParagraphId == amendment.TargetParagraph.ResaOperativeParagraphId);

                _context.DeleteAmendments.RemoveRange(deleteAmendmentsToRemove);
                var changeAmendmentsToRemove = _context.ChangeAmendments
                    .Where(n => n.TargetParagraph.ResaOperativeParagraphId == amendment.TargetParagraph.ResaOperativeParagraphId);
                _context.ChangeAmendments.RemoveRange(changeAmendmentsToRemove);

                _context.OperativeParagraphs.Remove(amendment.TargetParagraph);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        internal void AllowOnlineAmendments(string resolutionId)
        {
            var auth = this._context.ResolutionAuths.FirstOrDefault(n => n.ResolutionId == resolutionId);
            if (auth != null)
            {
                auth.AllowOnlineAmendments = true;
                _context.SaveChanges();
            }
        }

        internal void DisableOnlineAmendments(string resolutionId)
        {
            var auth = this._context.ResolutionAuths.FirstOrDefault(n => n.ResolutionId == resolutionId);
            if (auth != null)
            {
                auth.AllowOnlineAmendments = false;
                _context.SaveChanges();
            }
        }

        internal void EnablePublicEdit(string resolutionId)
        {
            var auth = this._context.ResolutionAuths.FirstOrDefault(n => n.ResolutionId == resolutionId);
            if (auth != null)
            {
                auth.AllowPublicEdit = true;
                _context.SaveChanges();
            }
        }

        internal async Task<bool> SetSupportersAsync(string resolutionId, string text)
        {
            var reso = await this._context.Resolutions.FirstOrDefaultAsync(n => n.ResaElementId == resolutionId);
            if (reso == null)
                return false;

            reso.SupporterNames = text;
            await this._context.SaveChangesAsync();
            return true;
        }

        internal void DisablePublicEdit(string resolutionId)
        {
            var auth = this._context.ResolutionAuths.FirstOrDefault(n => n.ResolutionId == resolutionId);
            if (auth != null)
            {
                auth.AllowPublicEdit = false;
                _context.SaveChanges();
            }
        }

        internal ResolutionSmallInfo GetResolutionInfo(string resolutionId)
        {
            var resolutions = from auth in _context.ResolutionAuths
                              join resa in _context.Resolutions on auth.ResolutionId equals resa.ResaElementId
                              where resa.ResaElementId == resolutionId
                              select new ResolutionSmallInfo()
                              {
                                  AllowAmendments = auth.AllowOnlineAmendments,
                                  AllowPublicEdit = auth.AllowPublicEdit,
                                  LastChangedTime = resa.CreatedDate,
                                  Name = resa.Topic,
                                  ResolutionId = resa.ResaElementId
                              };
            return resolutions.FirstOrDefault();
        }

        internal bool SubmitMoveAmendment(string amendmentId)
        {
            var amendment = _context.MoveAmendments
                .Include(n => n.VirtualParagraph)
                .Include(n => n.SourceParagraph)
                .FirstOrDefault(n => n.ResaAmendmentId == amendmentId);
            if (amendment != null)
            {
                _context.MoveAmendments.Remove(amendment);
                if (amendment.SourceParagraph != null)
                    _context.OperativeParagraphs.Remove(amendment.SourceParagraph);
                if (amendment.VirtualParagraph != null)
                {
                    amendment.VirtualParagraph.IsVirtual = false;
                    amendment.VirtualParagraph.Visible = true;
                    amendment.VirtualParagraph.IsLocked = false;
                }
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        internal bool SubmitChangeAmendment(string amendmentId)
        {
            var amendment = _context.ChangeAmendments.Include(n => n.TargetParagraph)
                .FirstOrDefault(n => n.ResaAmendmentId == amendmentId);
            if (amendment?.TargetParagraph != null)
            {
                amendment.TargetParagraph.Text = amendment.NewText;
                _context.ChangeAmendments.Remove(amendment);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        internal bool RemoveDeleteAmendment(string amendmentId)
        {
            var amendment = _context.DeleteAmendments.Find(amendmentId);
            if (amendment != null)
            {
                _context.DeleteAmendments.Remove(amendment);
                _context.SaveChanges();
            }
            return true;
        }

        public async Task<bool> SetTopicAsync(string resolutionId, string topic)
        {
            var reso = await this._context.Resolutions.FirstOrDefaultAsync(n => n.ResaElementId == resolutionId);
            if (reso == null) return false;
            reso.Topic = topic;
            await this._context.SaveChangesAsync();
            return true;
        }

        internal bool RemoveMoveAmendment(string amendmentId)
        {
            var amendment = _context.MoveAmendments.Include(n => n.VirtualParagraph)
                .FirstOrDefault(n => n.ResaAmendmentId == amendmentId);
            if (amendment != null)
            {
                if (amendment.VirtualParagraph != null)
                    _context.OperativeParagraphs.Remove(amendment.VirtualParagraph);
                _context.MoveAmendments.Remove(amendment);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> SetAgendaItem(string resolutionId, string agendaItem)
        {
            var reso = await this._context.Resolutions.FirstOrDefaultAsync(n => n.ResaElementId == resolutionId);
            if (reso == null) return false;
            reso.AgendaItem = agendaItem;
            await this._context.SaveChangesAsync();
            return true;
        }

        internal void ActivateAmendment(string amendmentId)
        {
            // Deactivate all other amendments if needed:
            //var resolutionId = _context.Amendments.FirstOrDefault(n => n.ResaAmendmentId == amendmentId).Resolution.ResaElementId;
            //if (resolutionId == null) return;
            //_context.Amendments.Where(n => n.Resolution.ResaElementId == resolutionId).ForEachAsync(n => n.Activated = false);
            var amendment = _context.Amendments.FirstOrDefault(n => n.ResaAmendmentId == amendmentId);
            if (amendment != null)
            {
                if (amendment is ResaMoveAmendment)
                {
                    var ma = _context.MoveAmendments.Include(n => n.VirtualParagraph)
                        .FirstOrDefault(n => n.ResaAmendmentId == amendment.ResaAmendmentId);
                    ma.VirtualParagraph.Visible = true;
                }
                else if (amendment is ResaAddAmendment)
                {
                    var ad = _context.AddAmendments.Include(n => n.VirtualParagraph)
                        .FirstOrDefault(n => n.ResaAmendmentId == amendment.ResaAmendmentId);
                    if (ad != null)
                    {
                        ad.VirtualParagraph.Visible = true;
                    }
                }
                amendment.Activated = true;
                this._context.SaveChanges();
            }
        }

        internal bool SubmitAddAmendment(string amendmentId)
        {
            var amendment = _context.AddAmendments.Include(n => n.VirtualParagraph)
                .FirstOrDefault(n => n.ResaAmendmentId == amendmentId);
            if (amendment != null)
            {
                if (amendment.VirtualParagraph != null)
                {
                    amendment.VirtualParagraph.IsVirtual = false;
                    amendment.VirtualParagraph.Visible = false;
                    amendment.VirtualParagraph.IsLocked = false;
                    amendment.VirtualParagraph = null;
                }
                _context.AddAmendments.Remove(amendment);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        internal bool RemoveAddAmendment(string amendmentId)
        {
            var amendment = _context.AddAmendments.Include(n => n.VirtualParagraph)
                .FirstOrDefault(n => n.ResaAmendmentId == amendmentId);
            if (amendment != null)
            {
                if (amendment.VirtualParagraph != null)
                    _context.OperativeParagraphs.Remove(amendment.VirtualParagraph);
                _context.AddAmendments.Remove(amendment);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> SetSession(string resolutionId, string agendaItem)
        {
            var reso = await this._context.Resolutions.FirstOrDefaultAsync(n => n.ResaElementId == resolutionId);
            if (reso == null) return false;
            reso.Session = agendaItem;
            await this._context.SaveChangesAsync();
            return true;
        }

        internal void DeactivateAmendment(string amendmentId)
        {
            var amendment = _context.Amendments.FirstOrDefault(n => n.ResaAmendmentId == amendmentId);
            if (amendment != null)
            {
                amendment.Activated = false;
                if (amendment is ResaMoveAmendment)
                {
                    var ma = _context.MoveAmendments.Include(n => n.VirtualParagraph)
                        .FirstOrDefault(n => n.ResaAmendmentId == amendment.ResaAmendmentId);
                    if (ma != null)
                    {
                        ma.VirtualParagraph.Visible = false;
                    }
                }
                else if (amendment is ResaAddAmendment)
                {
                    var ad = _context.AddAmendments.Include(n => n.VirtualParagraph)
                        .FirstOrDefault(n => n.ResaAmendmentId == amendment.ResaAmendmentId);
                    if (ad != null)
                    {
                        ad.VirtualParagraph.Visible = false;
                    }
                    
                }
                this._context.SaveChanges();
            }
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

        internal ResolutionAuth CreateSimulationResolution(CreateSimulationResolutionRequest body, string submitter = "")
        {
            return CreateSimulationResolution(body.SimulationId, body.Titel, submitter);
        }

        internal ResolutionAuth CreateSimulationResolution(int simulationId, string title, string submitter = "")
        {
            var resolution = new ResaElement()
            {
                Name = title,
                FullName = title,
                Topic = title,
                SubmitterName = submitter
            };

            var simulation = _context.Simulations.Find(simulationId);

            var auth = new ResolutionAuth()
            {
                ResolutionId = resolution.ResaElementId,
                AllowCommitteeRead = true,
                AllowConferenceRead = true,
                AllowOnlineAmendments = true,
                AllowPublicEdit = true,
                AllowPublicRead = true,
                Simulation = simulation,
                Name = title
            };
            _context.Resolutions.Add(resolution);
            _context.ResolutionAuths.Add(auth);
            _context.SaveChanges();
            return auth;
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

            int orderIndex = 0;
            if (_context.PreambleParagraphs.Any(n => n.ResaElement.ResaElementId == resolutionId))
            {
                orderIndex = _context.PreambleParagraphs.Where(n => n.ResaElement.ResaElementId == resolutionId)
                    .Max(n => n.OrderIndex) + 1;
            }

            var newParagraph = new ResaPreambleParagraph()
            {
                ResaElement = resolution,
                Text = text,
                OrderIndex = orderIndex
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

        public bool RemoveOperativeParagraph(string operativeParagraphId)
        {
            var deleteAmendments = this._context.DeleteAmendments.Where(n => n.TargetParagraph.ResaOperativeParagraphId == operativeParagraphId);
            this._context.DeleteAmendments.RemoveRange(deleteAmendments);
            var changeAmendments = this._context.ChangeAmendments.Where(n => n.TargetParagraph.ResaOperativeParagraphId == operativeParagraphId);
            this._context.ChangeAmendments.RemoveRange(changeAmendments);
            var moveAmendments = this._context.MoveAmendments.Where(n => n.SourceParagraph.ResaOperativeParagraphId == operativeParagraphId);
            if (moveAmendments.Any())
            {
                moveAmendments.ForEachAsync(n =>
                {
                    var toRemove = this._context.OperativeParagraphs.FirstOrDefault(a => a.ResaOperativeParagraphId == n.VirtualParagraph.ResaOperativeParagraphId);
                    this._context.OperativeParagraphs.Remove(toRemove);
                });
            }
            this._context.MoveAmendments.RemoveRange(moveAmendments);
            var paragraph = this._context.OperativeParagraphs.Find(operativeParagraphId);
            if (paragraph != null)
            {
                this._context.Remove(paragraph);
            }
            this._context.SaveChanges();
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

        public bool ReorderOperative(string resolutionId, List<string> list)
        {
            var isCountMatching = this._context.OperativeParagraphs.Count(n => n.Resolution.ResaElementId == resolutionId) == list.Count;
            if (!isCountMatching) return false;
            bool errored = false;
            for (int i=0;i<list.Count;i++)
            {
                var element = this._context.OperativeParagraphs.Find(list[i]);
                if (element != null)
                    element.OrderIndex = i;
                else
                    errored = true;
            }

            if (!errored)
                _context.SaveChanges();
            return !errored;
        }

        public ResaOperativeParagraph CreateOperativeParagraph(string resolutionId, string text = "")
        {
            var resolution = this._context.Resolutions.Find(resolutionId);
            if (resolution == null) return null;

            int orderIndex = 0;
            if (_context.OperativeParagraphs.Any(n => n.Resolution.ResaElementId == resolutionId))
            {
                orderIndex = _context.OperativeParagraphs.Where(n => n.Resolution.ResaElementId == resolutionId)
                    .Max(n => n.OrderIndex) + 1;
            }

            var element = new ResaOperativeParagraph()
            {
                Resolution = resolution,
                OrderIndex = orderIndex,
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
            var paragraph = this._context.OperativeParagraphs
                .Include(n => n.Resolution).FirstOrDefault(n => n.ResaOperativeParagraphId == resaOperativeParagraphId);
            if (paragraph == null)
            {
                Console.WriteLine("Zielparagraphen nicht gefunden: " + resaOperativeParagraphId);
                return null;
            }
            var amendment = new ResaDeleteAmendment()
            {
                TargetParagraph = paragraph,
                SubmitterName = submitterName,
                Resolution = this._context.OperativeParagraphs.Find(resaOperativeParagraphId).Resolution,
                SubmitTime = DateTime.Now,
            };
            this._context.DeleteAmendments.Add(amendment);
            this._context.SaveChanges();
            return amendment;
        }

        public ResaChangeAmendment CreateChangeAmendment(string resaOperativeParagraphId, string submitterName, string newText)
        {
            var paragraph = this._context.OperativeParagraphs
                .Include(n => n.Resolution).FirstOrDefault(n => n.ResaOperativeParagraphId == resaOperativeParagraphId);
            if (paragraph == null) return null;
            var amendment = new ResaChangeAmendment()
            {
                TargetParagraph = paragraph,
                SubmitterName = submitterName,
                Resolution = paragraph.Resolution,
                NewText = newText,
                SubmitTime = DateTime.Now
            };
            this._context.ChangeAmendments.Add(amendment);
            this._context.SaveChanges();
            return amendment;
        }

        public ResaMoveAmendment CreateMoveAmendment(string resaOperativeParagraphId, string submitterName, int position)
        {
            
            var targetParagraph = this._context.OperativeParagraphs
                .Include(n => n.Resolution).FirstOrDefault(n => n.ResaOperativeParagraphId == resaOperativeParagraphId);

            var allParagraphs = this._context.OperativeParagraphs
                .Where(n => n.Resolution.ResaElementId == targetParagraph.Resolution.ResaElementId)
                .OrderBy(n => n.OrderIndex).ToList(); ;
            // Cleanup the order
            int index = 0;
            allParagraphs.ForEach(n =>
            {
                if (index < position)
                {
                    n.OrderIndex = index;
                }
                else if (index >= position)
                {
                    n.OrderIndex = index + 1;
                }
                index++;
            });

            if (targetParagraph == null) return null;
            var virtualParagraph = new ResaOperativeParagraph()
            {
                OrderIndex = position,
                IsVirtual = true,
                IsLocked = true,
                Parent = targetParagraph.Parent,
                Text = targetParagraph.Text,
                Resolution = targetParagraph.Resolution
            };

            var amendment = new ResaMoveAmendment()
            {
                SourceParagraph = targetParagraph,
                VirtualParagraph = virtualParagraph,
                Resolution = targetParagraph.Resolution,
                SubmitterName = submitterName,
                SubmitTime = DateTime.Now
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
            if (resolution == null)
            {
                Console.WriteLine($"Unable to create AddAmendment because the Resolution: {resolutionId} was not found.");
                return null;
            }
            
            var virtualParagraph = new ResaOperativeParagraph()
            {
                OrderIndex = index,
                IsVirtual = true,
                IsLocked = true,
                Text = newText,
                Resolution = resolution,
                Visible = false
            };

            var amendment = new ResaAddAmendment()
            {
                Resolution = resolution,
                SubmitterName = submitter,
                VirtualParagraph = virtualParagraph,
                Activated = false,
                SubmitTime = DateTime.Now
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

        public async Task<ResolutionHeader> GetHeaderDto(string resolutionId)
        {
            var resolutionElement = await this._context.Resolutions.FindAsync(resolutionId);
            if (resolutionElement == null) return null;
            var header = new ResolutionHeader()
            {
                AgendaItem = resolutionElement.AgendaItem,
                CommitteeName = resolutionElement.CommitteeName,
                FullName = resolutionElement.FullName,
                Name = resolutionElement.Name,
                ResolutionHeaderId = resolutionElement.ResaElementId + "_header",
                Session = resolutionElement.Session,
                SubmitterName = resolutionElement.SubmitterName,
                Topic = resolutionElement.Topic,
                SupporterNames = resolutionElement.SupporterNames
            };
            return header;
        }

        public async Task<ResolutionPreamble> GetPreambleDto(string resolutionId)
        {
            var resolutionDb = this._context.Resolutions.Find(resolutionId);
            if (resolutionDb == null) return null;

            var preamble = new ResolutionPreamble()
            {
                PreambleId = resolutionDb.ResaElementId + "_preamble",
                Paragraphs = await _context.PreambleParagraphs.Where(n => n.ResaElement.ResaElementId == resolutionId).OrderBy(n => n.OrderIndex).Select(n => new PreambleParagraph()
                {
                    Comment = n.Comment,
                    Corrected = n.IsCorrected,
                    IsLocked = n.IsLocked,
                    PreambleParagraphId = n.ResaPreambleParagraphId,
                    Text = n.Text
                }).ToListAsync()
            };
            return preamble;
        }

        public async Task<List<AddAmendment>> GetAddAmendmentsDto(string resolutionId)
        {
            var filtered = _context.AddAmendments
                .Include(n => n.VirtualParagraph)
                .Where(n => n.Resolution.ResaElementId == resolutionId);
            var amendments = await filtered.Select(n => n.ToModel()).ToListAsync();
            return amendments;
        }

        public async Task<List<ChangeAmendment>> GetChangeAmendmentsDto(string resolutionId)
        {
            var filtered = _context.ChangeAmendments
                .Include(n => n.TargetParagraph)
                .Where(n => n.Resolution.ResaElementId == resolutionId);
            var amendments = await filtered.Select(n => n.ToModel()).ToListAsync();
            return amendments;
        }

        public async Task<List<DeleteAmendment>> GetDeleteAmendemtsDto(string resolutionId)
        {
            var filtered = _context.DeleteAmendments
                .Include(n => n.TargetParagraph)
                .Where(n => n.Resolution.ResaElementId == resolutionId);
            var amendments = await filtered.Select(n => n.ToModel()).ToListAsync();
            return amendments;
        }

        public async Task<List<MoveAmendment>> GetMoveAmendmentsDto(string resolutionId)
        {
            var filtered = _context.MoveAmendments.Include(n => n.SourceParagraph)
                .Include(n => n.VirtualParagraph)
                .Where(n => n.Resolution.ResaElementId == resolutionId);
            var amendments = await filtered.Select(n => n.ToModel()).ToListAsync();
            return amendments;
        }
        
        public async Task<List<OperativeParagraph>> GetOperativeParagraphs(string resolutionId)
        {
            var resolutionDb = await this._context.Resolutions.FindAsync(resolutionId);
            if (resolutionDb == null) return null;

            var paragraphs = await _context.OperativeParagraphs
                .Where(n => n.Resolution.ResaElementId == resolutionId && n.Parent == null).OrderBy(n => n.OrderIndex).Select(n => 
                new OperativeParagraph()
                {
                    Comment = n.Comment,
                    Corrected = n.Corrected,
                    IsLocked = n.IsLocked,
                    IsVirtual = n.IsVirtual,
                    Name = n.Name,
                    OperativeParagraphId = n.ResaOperativeParagraphId,
                    Text = n.Text,
                    Visible = n.Visible,
                }).ToListAsync();
            return paragraphs;
        }

        public async Task<Resolution> GetResolutionDtoAsync(string resolutionId)
        {
            var resolutionDb = this._context.Resolutions.Find(resolutionId);
            if (resolutionDb == null) return null;
            var dto = new Resolution()
            {
                Date = resolutionDb.CreatedDate,
                Header = await GetHeaderDto(resolutionId),

                Preamble = await GetPreambleDto(resolutionId),
                ResolutionId = resolutionDb.ResaElementId,
                OperativeSection = new OperativeSection()
                {
                    OperativeSectionId = resolutionDb.ResaElementId + "_os",

                    AddAmendments = await GetAddAmendmentsDto(resolutionId),

                    ChangeAmendments = await GetChangeAmendmentsDto(resolutionId),

                    DeleteAmendments = await GetDeleteAmendemtsDto(resolutionId),

                    MoveAmendments = await GetMoveAmendmentsDto(resolutionId),

                    Paragraphs = await GetOperativeParagraphs(resolutionId)
                }
            };
            return dto;
        }

        public int ResolutionCount()
        {
            return this._context.Resolutions.Count();
        }

        public ResaAmendment CreateAmendment(MUNityCore.Dtos.Resolutions.CreateAmendmentPattern pattern)
        {
            switch (pattern.AmendmentType)
            {
                case Dtos.Resolutions.EAmendmentTypes.Add:
                    return this.CreateAddAmendment(pattern.ResolutionId, pattern.NewIndex, pattern.SubmitterName, pattern.NewValue);
                case Dtos.Resolutions.EAmendmentTypes.Change:
                    return this.CreateChangeAmendment(pattern.ParagraphId, pattern.SubmitterName, pattern.NewValue);
                case Dtos.Resolutions.EAmendmentTypes.Delete:
                    return this.CreateDeleteAmendment(pattern.ParagraphId, pattern.SubmitterName);
                case Dtos.Resolutions.EAmendmentTypes.Move:
                    return this.CreateMoveAmendment(pattern.ParagraphId, pattern.SubmitterName, pattern.NewIndex);
                default:
                    Console.WriteLine($"Nicht unterstützter Antragstyp: {pattern.AmendmentType.ToString()}");
                    return null;
            }
        }

        internal List<ResolutionAuth> GetAuthsWithSimulationsOrResolutions()
        {
            return _context.ResolutionAuths.Include(n => n.Resolution)
                .Include(n => n.Simulation).ToList();
        }
    }
}
