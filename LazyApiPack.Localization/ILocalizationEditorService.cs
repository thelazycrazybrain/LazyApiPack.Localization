using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Localization
{
    public interface ILocalizationEditorService
    {
        void ShowEditor([DisallowNull] string[] localizationDirectories, string? searchPattern = null, EnumerationOptions? options = null);
    }
}
