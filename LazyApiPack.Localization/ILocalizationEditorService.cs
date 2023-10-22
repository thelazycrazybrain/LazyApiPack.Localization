using System.Diagnostics.CodeAnalysis;

namespace LazyApiPack.Localization
{
    public interface ILocalizationEditorService
    {
        void ShowEditor([DisallowNull] string[] localizationDirectories, string? searchPattern = null, EnumerationOptions? options = null);
    }
}
