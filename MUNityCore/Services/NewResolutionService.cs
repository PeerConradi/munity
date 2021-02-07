using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MUNityCore.Models.Resolution;
using MUNityCore.Util.Extensions;
using MUNityCore.DataHandlers;
using MUNityCore.DataHandlers.EntityFramework;
using MUNity.Models.Resolution;
using MUNityCore.Models.Resolution.V2;
using MUNity.Extensions.ResolutionExtensions;

namespace MUNityCore.Services
{
    public class NewResolutionService : IResolutionService
    {
        private readonly MunityContext _munityContext;

        private readonly IMongoCollection<Resolution> _resolutions;

        
        public async Task<Resolution> CreateResolution(string title)
        {

            var resolution = new Resolution();
            resolution.Header.Topic = title;
            resolution.ResolutionId = Util.Tools.IdGenerator.RandomString(36);
            // Save in MongoDb
            await _resolutions.InsertOneAsync(resolution);

            var auth = new ResolutionAuth(resolution);
            await _munityContext.ResolutionAuths.AddAsync(auth);
            return resolution;
        }

        public async Task<Resolution> CreatePublicResolution(string title)
        {
            var resolution = new Resolution();
            resolution.ResolutionId = Util.Tools.IdGenerator.RandomString(36);
            resolution.Header.Topic = title;
            // Save in MongoDb
            await _resolutions.InsertOneAsync(resolution);
            var auth = new ResolutionAuth(resolution) {AllowPublicEdit = true, AllowPublicRead = true};
            await _munityContext.ResolutionAuths.AddAsync(auth);
            await _munityContext.SaveChangesAsync();

            return resolution;
        }

        public async Task<Resolution> GetResolution(string id)
        {
            return await _resolutions.Find(n => n.ResolutionId == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdatePreambleParagraph(Resolution resolution,PreambleParagraph newValues)
        {
            var targetParagraph = resolution.Preamble?.Paragraphs?.FirstOrDefault(n => n.PreambleParagraphId == newValues.PreambleParagraphId);
            if (targetParagraph == null) return false;
            // Maybe writing just replacing the List entry would be better idk.
            targetParagraph.Corrected = newValues.Corrected;
            targetParagraph.IsLocked = newValues.IsLocked;
            targetParagraph.Comments = newValues.Comments;
            targetParagraph.Text = newValues.Text;
            await this.SaveResolution(resolution);
            return true;
        }

        public async Task<bool> UpdateOperativeParagraph(Resolution resolution, OperativeParagraph newValues)
        {
            var targetParagraph = resolution.OperativeSection?.FirstOrDefault(n => n.OperativeParagraphId == newValues.OperativeParagraphId);
            if (targetParagraph == null) return false;
            targetParagraph.Children = newValues.Children;
            targetParagraph.IsLocked = newValues.IsLocked;
            targetParagraph.IsVirtual = newValues.IsVirtual;
            targetParagraph.Name = newValues.Name;
            targetParagraph.Comments = newValues.Comments;
            targetParagraph.Text = newValues.Text;
            targetParagraph.Visible = newValues.Visible;
            targetParagraph.Comment = newValues.Comment;
            targetParagraph.Corrected = newValues.Corrected;
            await this.SaveResolution(resolution);
            return true;
        }

        public async Task<bool> ResolutionExists(string id)
        {
            return await this._munityContext.ResolutionAuths.AnyAsync(n => n.ResolutionId == id);
        }

        public async Task<ResolutionAuth> GetResolutionAuth(string id)
        {
            return await _munityContext.ResolutionAuths.Include(n => n.Users).FirstOrDefaultAsync(n => n.ResolutionId == id);
        }

        public async Task<Resolution> DeleteResolution(Resolution resolution)
        {
            return await _resolutions.FindOneAndDeleteAsync(n => n.ResolutionId == resolution.ResolutionId);
        }

        public async Task<PreambleParagraph> AddPreambleParagraph(Resolution resolution, string text = "")
        {
            resolution.Preamble.Paragraphs ??= new System.Collections.ObjectModel.ObservableCollection<PreambleParagraph>();
            var paragraph = new PreambleParagraph {Text = text};
            resolution.Preamble.Paragraphs.Add(paragraph);
            await _resolutions.FindOneAndReplaceAsync(n => n.ResolutionId == resolution.ResolutionId, resolution);
            //await _resolutions.ReplaceOneAsync(n => n.ResolutionId == resolution.ResolutionId, resolution);
            return paragraph;
        }

        public async Task<Resolution> RemovePreambleParagraph(Resolution resolution, string paragraphId)
        {
            var paragraph = resolution.Preamble?.Paragraphs.FirstOrDefault(n =>
                n.PreambleParagraphId == paragraphId);
            if (paragraph != null) resolution.Preamble.Paragraphs.Remove(paragraph);
            return await SaveResolution(resolution);
        }

        public async Task<Resolution> MovePreambleParagraphUp(Resolution resolution, string paragraphId)
        {
            var paragraph = resolution.Preamble?.Paragraphs.FirstOrDefault(n =>
                n.PreambleParagraphId == paragraphId);
            if (paragraph != null)
            {
                var oldIndex = resolution.Preamble.Paragraphs.IndexOf(paragraph);
                if (oldIndex > 0)
                    resolution.Preamble.Paragraphs.Move(oldIndex, oldIndex - 1);
            }
            return await SaveResolution(resolution);
        }

        public async Task<Resolution> MovePreambleParagraphDown(Resolution resolution, string paragraphId)
        {
            var paragraph = resolution.Preamble?.Paragraphs.FirstOrDefault(n =>
                n.PreambleParagraphId == paragraphId);
            if (paragraph != null)
            {
                var oldIndex = resolution.Preamble.Paragraphs.IndexOf(paragraph);
                if (oldIndex < resolution.Preamble.Paragraphs.Count)
                    resolution.Preamble.Paragraphs.Move(oldIndex, oldIndex + 1);
            }

            return await SaveResolution(resolution);
        }

        public async Task<Resolution> SaveResolution(Resolution resolution)
        {
            return await _resolutions.FindOneAndReplaceAsync(n => n.ResolutionId == resolution.ResolutionId, resolution);
        }

        public async Task<OperativeParagraph> AddOperativeParagraph(Resolution resolution)
        {
            var paragraph = new OperativeParagraph();
            resolution.OperativeSection.Paragraphs.Add(paragraph);
            await _resolutions.FindOneAndReplaceAsync(n => n.ResolutionId == resolution.ResolutionId, resolution);
            return paragraph;
        }

        public Task<int> GetResolutionCount()
        {
            return this._munityContext.ResolutionAuths.CountAsync();
        }

        public NewResolutionService(MunityContext munityContext, IMunityMongoDatabaseSettings mongoSettings)
        {
            _munityContext = munityContext;
            var client = new MongoClient(mongoSettings.ConnectionString);
            var database = client.GetDatabase(mongoSettings.DatabaseName);
            _resolutions = database.GetCollection<Resolution>(mongoSettings.ResolutionCollectionName);
        }
    }
}
