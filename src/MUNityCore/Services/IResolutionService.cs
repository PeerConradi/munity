using System.Threading.Tasks;
using MongoDB.Driver;
using MUNityCore.Models.Resolution.V2;
using MUNity.Models.Resolution;

namespace MUNityCore.Services
{
    public interface IResolutionService
    {
        Task<Resolution> CreateResolution(string title);

        Task<Resolution> CreatePublicResolution(string title);

        Task<Resolution> GetResolution(string id);

        Task<ResolutionAuth> GetResolutionAuth(string id);

        Task<PreambleParagraph> AddPreambleParagraph(Resolution preamble, string text = "");

        Task<OperativeParagraph> AddOperativeParagraph(Resolution section);

        Task<Resolution> SaveResolution(Resolution resolution);

        Task<int> GetResolutionCount();

        Task<bool> UpdatePreambleParagraph(Resolution resolution, PreambleParagraph newValues);

        Task<bool> UpdateOperativeParagraph(Resolution resolution, OperativeParagraph newValues);

        Task<bool> ResolutionExists(string id);
        Task SetNameInDb(string resolutionId, string text);
    }
}