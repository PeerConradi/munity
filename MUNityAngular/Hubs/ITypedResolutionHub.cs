﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Hubs
{
    public interface ITypedResolutionHub
    {
        Task OperativeParagraphAdded(int position, string id, string text);

        Task PreambleParagraphAdded(int position, string id, string text);

        Task OperativeParagraphChanged(string id, string newText);

        Task PreambleParagraphChanged(string id, string newText);

        Task PreambleSectionOrderChanged(IEnumerable<string> newOrder);

        Task OperativeSectionOrderChanged(IEnumerable<string> newOrder);
    }
}
