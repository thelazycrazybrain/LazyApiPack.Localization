using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace LazyApiPack.Localization
{
  
    /// <summary>
    /// Provides a service for localization.
    /// </summary>
    public interface ILocalizationService
    {
        /// <summary>
        /// Fires when the localization has changed.
        /// </summary>
        event LocalizationChangedEventHandler LocalizationChanged;

        /// <summary>
        /// Adds a list of localization files to the service.
        /// </summary>
        /// <param name="localizationFiles">List of language files.</param>
        void AddLocalizations([DisallowNull] IEnumerable<string> localizationFiles);

        /// <summary>
        /// Adds a localization file to the service.
        /// </summary>
        /// <param name="localizationFiles">The language file.</param>
        void AddLocalizations([DisallowNull] string localizationFile);

        /// <summary>
        /// Adds a localization resource to the service.
        /// </summary>
        /// <param name="assembly">Assembly that contains the resource.</param>
        /// <param name="fullName">Fully qualified path to the resource.</param>
        void AddLocalizations(Assembly assembly, [DisallowNull] string fullName);

        /// <summary>
        /// Adds a localization resource to the service.
        /// </summary>
        /// <param name="assembly">Assembly that contains the resource.</param>
        /// <param name="fullNames">Fully qualified paths to the resource.</param>
        void AddLocalizations(Assembly assembly, [DisallowNull] IEnumerable<string> fullNames);

        /// <summary>
        /// Removes the localizations from a specific module.
        /// </summary>
        /// <param name="moduleId">Unique id of the module that needs to be removed.</param>
        void RemoveModule(string moduleId);
        /// <summary>
        /// Gets or sets the current language.
        /// </summary>
        ILocalizationHeader? CurrentLocalization { get; set; }
        /// <summary>
        /// Contains meta-data of all found translation files.
        /// </summary>
        IEnumerable<ILocalizationHeader> AvailableLocalizations { get; }

        /// <summary>
        /// Gets the translated text for the group and id
        /// </summary>
        /// <param name="group">E.g. "Captions"</param>
        /// <param name="id">E.g. "WelcomeMessageCaption"</param>
        /// <returns>The translated text or null.</returns>
        string? GetTranslation(string group, string id);

    }
    
}