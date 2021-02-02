using MUNity.Extensions.ResolutionExtensions;
using MUNity.Models.Resolution;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MUNity.Troubleshooting.Resa
{
    /// <summary>
    /// This component searches for errors inside a given Resolution and returns a bug report and will fix some bugs inside the
    /// document.
    /// </summary>
    public class ResolutionTroubleshooting
    {
        private static IEnumerable<IResolutionBug> Bugfinder
        {
            get
            {
                yield return new InvalidResolutionHeader();
                yield return new InvalidPreamble();
                yield return new InvalidOperativeSection();
                yield return new InvalidAmendments();
            }
        }

        /// <summary>
        /// Something in the Header is invalid. For example a string is null, but strings should always be string.Empty or some text.
        /// </summary>
        public class InvalidResolutionHeader : IResolutionBug
        {
            private string detectedError = "";

            public string Description => detectedError;

            /// <summary>
            /// Detects bugs inside the Header of a resolution and returns true if bugs are found.
            /// </summary>
            /// <param name="resolution"></param>
            /// <returns></returns>
            public bool Detect(Resolution resolution)
            {
                Dictionary<string, object> notNullProps = new Dictionary<string, object>()
                {
                    { "Header", resolution.Header },
                    { "AgendaItem", resolution.Header?.AgendaItem },
                    { "CommitteeName", resolution.Header?.CommitteeName },
                    { "FullName", resolution.Header?.FullName },
                    { "Name", resolution.Header?.Name },
                    { "Session", resolution.Header?.Session },
                    { "SubmitterName", resolution.Header?.SubmitterName },
                    { "Supporters", resolution.Header?.Supporters },
                    { "Topic", resolution.Header?.Topic }
                };

                foreach (var element in notNullProps)
                {
                    if (element.Value == null) detectedError += $"{element.Key} is not allowed to be null.\n";
                }
                return notNullProps.ContainsValue(null);
            }

            /// <summary>
            /// Fixes the bugs that where found before.
            /// </summary>
            /// <param name="resolution"></param>
            /// <returns></returns>
            public bool Fix(Resolution resolution)
            {
                if (resolution.Header == null) resolution.Header = new ResolutionHeader();
                if (resolution.Header.AgendaItem == null) resolution.Header.AgendaItem = string.Empty;
                if (resolution.Header.CommitteeName == null) resolution.Header.CommitteeName = string.Empty;
                if (resolution.Header.FullName == null) resolution.Header.FullName = string.Empty;
                if (resolution.Header.Name == null) resolution.Header.Name = string.Empty;
                if (resolution.Header.Session == null) resolution.Header.Session = string.Empty;
                if (resolution.Header.SubmitterName == null) resolution.Header.SubmitterName = string.Empty;
                if (resolution.Header.Supporters == null) resolution.Header.Supporters = new System.Collections.ObjectModel.ObservableCollection<string>();
                if (resolution.Header.Topic == null) resolution.Header.Topic = string.Empty;
                return true;
            }
        }

        /// <summary>
        /// A component to check the preamble for errors.
        /// </summary>
        public class InvalidPreamble : IResolutionBug
        {
            private string output = "";

            public string Description => output;

            public bool Detect(Resolution resolution)
            {
                if (resolution.Preamble == null)
                {
                    output = "Preamble cannot be null!";
                    return true;
                }
                if (resolution.Preamble.Paragraphs == null)
                {
                    output = "Preamble Paragraphs cannot be null";
                    return true;
                }
                if (string.IsNullOrEmpty(resolution.Preamble.PreambleId))
                {
                    output = "Preamble needs to have an Id";
                    return true;
                }
                return false;
            }

            public bool Fix(Resolution resolution)
            {
                if (resolution.Preamble == null) resolution.Preamble = new ResolutionPreamble();
                if (resolution.Preamble.Paragraphs == null) resolution.Preamble.Paragraphs = new System.Collections.ObjectModel.ObservableCollection<PreambleParagraph>();
                if (string.IsNullOrEmpty(resolution.Preamble.PreambleId)) resolution.Preamble.PreambleId = Guid.NewGuid().ToString();
                return true;
            }
        }

        public class InvalidOperativeSection : IResolutionBug
        {

            private string output = "";
            public string Description => output;

            public bool Detect(Resolution resolution)
            {
                if (resolution.OperativeSection == null) return true;
                if (resolution.OperativeSection.AddAmendments == null) return true;
                if (resolution.OperativeSection.ChangeAmendments == null) return true;
                if (resolution.OperativeSection.DeleteAmendments == null) return true;
                if (resolution.OperativeSection.MoveAmendments == null) return true;
                if (string.IsNullOrEmpty(resolution.OperativeSection.OperativeSectionId)) return true;
                if (resolution.OperativeSection.Paragraphs == null) return true;
                return false;
            }

            public bool Fix(Resolution resolution)
            {
                if (resolution.OperativeSection == null) resolution.OperativeSection = new OperativeSection();
                if (resolution.OperativeSection.AddAmendments == null) resolution.OperativeSection.AddAmendments = new ObservableCollection<AddAmendment>();
                if (resolution.OperativeSection.ChangeAmendments == null) resolution.OperativeSection.ChangeAmendments = new ObservableCollection<ChangeAmendment>();
                if (resolution.OperativeSection.DeleteAmendments == null) resolution.OperativeSection.DeleteAmendments = new ObservableCollection<DeleteAmendment>();
                if (resolution.OperativeSection.MoveAmendments == null) resolution.OperativeSection.MoveAmendments = new ObservableCollection<MoveAmendment>();
                if (string.IsNullOrEmpty(resolution.OperativeSection.OperativeSectionId)) resolution.OperativeSection.OperativeSectionId = Guid.NewGuid().ToString();
                if (resolution.OperativeSection.Paragraphs == null) resolution.OperativeSection.Paragraphs = new ObservableCollection<OperativeParagraph>();
                return true;
            }
        }

        public class InvalidAmendments : IResolutionBug
        {
            private string bugs = "";

            public string Description => bugs;

            public bool Detect(Resolution resolution)
            {
                var ghosts = resolution.OperativeSection.WhereParagraph(n => n.IsVirtual && (
                    resolution.OperativeSection.MoveAmendments.All(a => a.NewTargetSectionId != n.OperativeParagraphId) &&
                    resolution.OperativeSection.AddAmendments.All(a => a.TargetSectionId != n.OperativeParagraphId))
                );

                if (ghosts.Any())
                {
                    foreach (var ghost in ghosts)
                    {
                        bugs += $"{ghost.OperativeParagraphId} (Virutal: {ghost.IsVirtual}) is a ghost. There is no amendment referencing this paragraph.\n";
                    }
                }

                return bugs != "";
            }

            public bool Fix(Resolution resolution)
            {
                var ghosts = resolution.OperativeSection.WhereParagraph(n => n.IsVirtual && (resolution.OperativeSection.MoveAmendments.All(a => a.NewTargetSectionId != n.OperativeParagraphId) &&
                    resolution.OperativeSection.AddAmendments.All(a => a.TargetSectionId != n.OperativeParagraphId)));

                if (ghosts.Any())
                {
                    foreach (var ghost in ghosts)
                    {
                        resolution.OperativeSection.RemoveOperativeParagraph(ghost);
                    }
                }

                return true;
            }
        }

        public static (bool isCorrupted, string log) IsResolutionCorrupted(Resolution resolution)
        {
            var result = false;
            string output = "";
            foreach (var finder in Bugfinder)
            {
                if (finder.Detect(resolution))
                {
                    result = true;
                    output += finder.Description;
                }
            }
            return (result, output);
        }

        public static bool FixResolution(Resolution resolution)
        {
            var result = true;
            foreach (var finder in Bugfinder)
            {
                if (!finder.Fix(resolution)) result = false;
            }
            return result;
        }
    }
}
