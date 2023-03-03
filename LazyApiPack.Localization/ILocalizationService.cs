using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace LazyApiPack.Localization
{
    /// <summary>
    /// Provides a Localization Service
    /// </summary>
    public interface ILocalizationService : ILocalization
    {
        event LocalizationChangedEventHandler LocalizationChanged;

        /// <summary>
        /// Loads the metadata from the translation files included in the localizationDictionaries.
        /// </summary>
        /// <param name="localizationDirectories">Path(s) where the service should look for localizations.</param>
        /// <param name="searchPattern">Specifies a file name search pattern that is used to find localization files (e.g. *.lng or *.json).</param>
        /// <param name="options">Options that are used to find pathes (e.g. options.IncludeSubDirectories).</param>
        void Initialize([NotNull] string[] localizationDirectories, string? searchPattern = null, EnumerationOptions? options = null);
        /// <summary>
        /// Gets or sets the current language.
        /// </summary>
        ILocalizationHeader? CurrentLocalization { get; set; }
        ReadOnlyDictionary<string, ILocalizationHeader> AvailableLocalizations { get; }

    }
}