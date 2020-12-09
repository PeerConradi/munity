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
using MUNityCore.Models.Resolution.V2;

namespace MUNityCore.Services
{
    public class NewResolutionService : IResolutionService
    {
        private readonly MunityContext _munityContext;

        private readonly IMongoCollection<ResolutionV2> _resolutions;

        
        public async Task<ResolutionV2> CreateResolution(string title)
        {
            
            var resolution = new ResolutionV2
            {
                ResolutionId = Util.Tools.IdGenerator.RandomString(36), Header = {Topic = title}
            };
            // Save in MongoDb
            await _resolutions.InsertOneAsync(resolution);

            var auth = new ResolutionAuth(resolution);
            await _munityContext.ResolutionAuths.AddAsync(auth);
            return resolution;
        }

        public async Task<ResolutionV2> CreatePublicResolution(string title)
        {
            var resolution = new ResolutionV2
            {
                ResolutionId = Util.Tools.IdGenerator.RandomString(36), Header = {Topic = title}
            };
            // Save in MongoDb
            await _resolutions.InsertOneAsync(resolution);
            var auth = new ResolutionAuth(resolution) {AllowPublicEdit = true, AllowPublicRead = true};
            await _munityContext.ResolutionAuths.AddAsync(auth);
            await _munityContext.SaveChangesAsync();

            return resolution;
        }

        public async Task<ResolutionV2> GetResolution(string id)
        {
            return await _resolutions.Find(n => n.ResolutionId == id).FirstOrDefaultAsync();
        }

        public async Task<ResolutionAuth> GetResolutionAuth(string id)
        {
            return await _munityContext.ResolutionAuths.Include(n => n.Users).FirstOrDefaultAsync(n => n.ResolutionId == id);
        }

        public async Task<ResolutionV2> DeleteResolution(ResolutionV2 resolution)
        {
            return await _resolutions.FindOneAndDeleteAsync(n => n.ResolutionId == resolution.ResolutionId);
        }

        public async Task<IPreambleParagraph> AddPreambleParagraph(ResolutionV2 resolution, string text = "")
        {
            resolution.Preamble.Paragraphs ??= new List<PreambleParagraph>();
            var paragraph = new PreambleParagraph {Text = text};
            resolution.Preamble.Paragraphs.Add(paragraph);
            await _resolutions.FindOneAndReplaceAsync(n => n.ResolutionId == resolution.ResolutionId, resolution);
            //await _resolutions.ReplaceOneAsync(n => n.ResolutionId == resolution.ResolutionId, resolution);
            return paragraph;
        }

        public async Task<ResolutionV2> RemovePreambleParagraph(ResolutionV2 resolution, string paragraphId)
        {
            var paragraph = resolution.Preamble?.Paragraphs.FirstOrDefault(n =>
                n.PreambleParagraphId == paragraphId);
            if (paragraph != null) resolution.Preamble.Paragraphs.Remove(paragraph);
            return await SaveResolution(resolution);
        }

        public async Task<ResolutionV2> MovePreambleParagraphUp(ResolutionV2 resolution, string paragraphId)
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

        public async Task<ResolutionV2> MovePreambleParagraphDown(ResolutionV2 resolution, string paragraphId)
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

        public async Task<ResolutionV2> SaveResolution(ResolutionV2 resolution)
        {
            var options = new FindOneAndReplaceOptions<ResolutionV2>
            {
                ReturnDocument = ReturnDocument.After
            };
            return await _resolutions.FindOneAndReplaceAsync<ResolutionV2>(n => n.ResolutionId == resolution.ResolutionId, resolution, options);
        }

        public async Task<IOperativeParagraph> AddOperativeParagraph(ResolutionV2 resolution)
        {
            var paragraph = new OperativeParagraph();
            resolution.OperativeSection.Paragraphs.Add(paragraph);
            await _resolutions.FindOneAndReplaceAsync(n => n.ResolutionId == resolution.ResolutionId, resolution);
            await _resolutions.ReplaceOneAsync(n => n.ResolutionId == resolution.ResolutionId, resolution);
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
            _resolutions = database.GetCollection<ResolutionV2>(mongoSettings.ResolutionCollectionName);
        }
    }
}
