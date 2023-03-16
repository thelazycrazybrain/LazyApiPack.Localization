using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace LazyApiPack.Localization
{
    /// <summary>
    /// Provides a service for localization.
    /// </summary>
    public interface ILocalizationService : ILocalization
    {
        /// <summary>
        /// Fires when the localization has changed.
        /// </summary>
        event LocalizationChangedEventHandler LocalizationChanged;

        /// <summary>
        /// Call this function after instantiation to initialize the service.
        /// </summary>
        /// <param name="localizationDirectories">Files and / or directories that contain localization files.</param>
        /// <param name="searchPattern">If you work with directories, this filter can be used to select specific localization files (e.g. *.json)</param>
        /// <param name="options">If you work with directories, you can specify the search options (e.g. include subdirectories etc.)</param>
        void AddLocalizations([NotNull] string[] localizationDirectories, string? searchPattern = null, EnumerationOptions? options = null);

        /// <summary>
        /// Adds localization files from an assembly.
        /// </summary>
        /// <param name="assembly">Assembly that contains the localization resources.</param>
        /// <param name="localizationNamespaces">Namespaces that include localization resources.</param>
        /// <param name="searchPattern">Search pattern that finds resources in the assembly.</param>
        void AddLocalizations([NotNull] Assembly assembly, [NotNull] string[] localizationNamespaces, string? searchPattern = null);
        /// <summary>
        /// Gets or sets the current language.
        /// </summary>
        ILocalizationHeader? CurrentLocalization { get; set; }
        /// <summary>
        /// Contains meta-data of all found translation files.
        /// </summary>
        ReadOnlyCollection<ILocalizationHeader> AvailableLocalizations { get; }

    }
}