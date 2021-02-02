using Microsoft.AspNetCore.SignalR.Client;
using MUNity.Hubs;
using MUNity.Models.Resolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityClient.Services.SocketHandlers
{
    /// <summary>
    /// The ResaSocketHandler is connecting with the SignalR Socket of the MUNity API.
    /// </summary>
    public class ResaSocketHandler : MUNity.Hubs.ITypedResolutionHub
    {
        public delegate void OnResolutionChanged(Resolution resolution);

        public event OnResolutionChanged SocketResolutionChanged;

        public HubConnection HubConnection { get; set; }

        public List<string> IgnoreTransactions { get; set; }

        private Resolution _resolution;

        private ResaSocketHandler(Resolution resolution)
        {
            _resolution = resolution;
            IgnoreTransactions = new List<string>();

            HubConnection = new HubConnectionBuilder().WithUrl($"{Program.API_URL}/resasocket").Build();
            HubConnection.On<Resolution>(nameof(ResolutionChanged), (newResolution) => ResolutionChanged(newResolution));
            HubConnection.On<string, PreambleParagraph, string>(nameof(PreambleParagraphChanged), (resolutionId, paragraph, tan) => PreambleParagraphChanged(resolutionId, paragraph, tan));
            HubConnection.On<string, OperativeParagraph, string>(nameof(OperativeParagraphChanged), (resolutionId, paragraph, tan) => OperativeParagraphChanged(resolutionId, paragraph, tan));
        }

        public static async Task<ResaSocketHandler> CreateHandler(Resolution resolution)
        {
            var instance = new ResaSocketHandler(resolution);
            await instance.HubConnection.StartAsync();
            return instance;
        }

        public Task ResolutionChanged(Resolution newResolution)
        {
            if (newResolution.ResolutionId != _resolution.ResolutionId) return null;
            // TODO: Tan check

            _resolution.Header = newResolution.Header ?? _resolution.Header;
            _resolution.Preamble = newResolution.Preamble ?? _resolution.Preamble;
            _resolution.OperativeSection = newResolution.OperativeSection ?? _resolution.OperativeSection;
            return null;
        }

        public Task PreambleParagraphChanged(string resolutionId, PreambleParagraph para, string tan)
        {
            if (CanIgnore(tan)) return null;

            if (resolutionId != _resolution.ResolutionId) return null;
            var targetParagraph = _resolution.Preamble.Paragraphs.FirstOrDefault(n => n.PreambleParagraphId == para.PreambleParagraphId);
            if (targetParagraph != null)
            {
                targetParagraph.Text = para.Text;
                targetParagraph.Comments = para.Comments;
            }
            return null;
        }

        public Task OperativeParagraphChanged(string resolutionId, OperativeParagraph para, string tan)
        {
            if (CanIgnore(tan)) return null;

            if (resolutionId != _resolution.ResolutionId) return null;
            var paragraph = _resolution.OperativeSection.Paragraphs.FirstOrDefault(n => n.OperativeParagraphId == para.OperativeParagraphId);
            if (paragraph == null) return null;
            paragraph.Text = para.Text;
            paragraph.Comments = para.Comments;
            //ResolutionChanged?.Invoke(this._resolution);
            return null;
        }

        private bool CanIgnore(string tan)
        {
            if (IgnoreTransactions.Any(n => n == tan))
            {
                IgnoreTransactions.Remove(tan);
                return true;
            }
            return false;
        }

        public Task ResolutionChanged(Resolution resolution, string tan)
        {
            throw new NotImplementedException();
        }

        public Task AmendmentActivatedChanged(string resolutionId, string amendmentId, bool value, string tan)
        {
            throw new NotImplementedException();
        }

        public Task PreambleParagraphTextChanged(string resolutionId, string paragraphId, string text, string tan)
        {
            throw new NotImplementedException();
        }

        public Task OperativeParagraphTextChanged(string resolutionId, string paragraphId, string text, string tan)
        {
            throw new NotImplementedException();
        }
    }
}
