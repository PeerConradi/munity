using System.Threading.Tasks;
using MongoDB.Driver;
using MUNityCore.Models.Resolution.V2;

namespace MUNityCore.Services
{
    public interface IResolutionService
    {
        Task<ResolutionV2> CreateResolution(string title);

        Task<ResolutionV2> CreatePublicResolution(string title);

        Task<ResolutionV2> GetResolution(string id);

        Task<ResolutionAuth> GetResolutionAuth(string id);

        Task<IPreambleParagraph> AddPreambleParagraph(ResolutionV2 preamble, string text = "");

        Task<IOperativeParagraph> AddOperativeParagraph(ResolutionV2 section);

        Task<ResolutionV2> SaveResolution(ResolutionV2 resolution);

        Task<int> GetResolutionCount();
    }
}