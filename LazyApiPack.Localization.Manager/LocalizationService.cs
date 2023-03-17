using LazyApiPack.Localization.Json;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Formats.Asn1;
using System.Security.Cryptography;
using System.Text.Json;
using LazyApiPack.Collections.Extensions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace LazyApiPack.Localization.Manager {

    /// <inheritdoc/>
    public class LocalizationService : ILocalizationService {
        /// <inheritdoc/>
        public event LocalizationChangedEventHandler? LocalizationChanged;
        /// <summary>
        /// Contains meta-information about all found localization files.
        /// </summary>
        private readonly List<ILocalizationHeader> _availableLocalizations
            = new List<ILocalizationHeader>();
        /// <inheritdoc/>
        public ReadOnlyCollection<ILocalizationHeader> AvailableLocalizations => _availableLocalizations.AsReadOnly();


        /// <summary>
        /// Contains the localizations from the current localization file.
        /// </summary>
        private ILocalization? _currentLocalizationDictionary;
        /// <summary>
        /// Creates an instance of the LocalizationService
        /// </summary>
        public LocalizationService() { }

        /// <inheritdoc/>
        public void AddLocalizations([NotNull] string[] localizationDirectories, string? searchPattern = null, EnumerationOptions? options = null) {
            searchPattern = searchPattern ?? string.Empty;
            options = options ?? new EnumerationOptions();
            var folders = localizationDirectories.Where(d => Directory.Exists(d));
            var files = localizationDirectories.Where(d => File.Exists(d));


            var localizationFiles = folders.SelectMany(d => Directory.GetFiles(d, searchPattern, options)).ToList();
            localizationFiles.AddRange(files);

            AddLocalizationsIntern(localizationFiles, null, (path) => File.OpenRead(path));

        }

        public void AddLocalizations([NotNull] Assembly assembly, [NotNull] string[] localizationNamespaces, string? searchPattern = null) {
            if (localizationNamespaces == null || localizationNamespaces.Length == 0) return;

            var resources = assembly.GetManifestResourceNames().Where(m => localizationNamespaces.Any(n => m.StartsWith(n)));

            // If file filter was used:
            if (!string.IsNullOrWhiteSpace(searchPattern)) {
                // Convert pattern to regex
                searchPattern = $".*{searchPattern.Replace(".", @"\.").Replace('?', '.').Replace("*", "(.*)")}$";
                var rx = new Regex(searchPattern);
                resources = resources.Where(m => rx.Match(m).Success);
            }

            AddLocalizationsIntern(resources.ToList(), assembly, (path) => assembly.GetManifestResourceStream(path));

        }
        /// <summary>
        /// Adds localizations independent from the source
        /// </summary>
        /// <param name="pathes">The path for the streamLoader</param>
        /// <param name="sourceAssembly">If the resource is from an assembly, specify the assembly here.</param>
        /// <param name="streamLoader">Function that resolves the path and returns a stream containing the localization json file.</param>
        /// <exception cref="NullReferenceException">If the JsonSerializer returns no object for the specified file.</exception>
        /// <exception cref="ArgumentNullException">If the streamLoader function was not specified.</exception>
        private void AddLocalizationsIntern(List<string> pathes, Assembly? sourceAssembly, Func<string, Stream>? streamLoader)
        {
            if (streamLoader == null) throw new ArgumentNullException(nameof(streamLoader));
            foreach (var path in pathes)
            {
                using (var fHandle = streamLoader(path))
                {
                    var localization = JsonSerializer.Deserialize<LocalizationHeader>(fHandle)
                        ?? throw new NullReferenceException("The deserialized localization file returned null.");
                    localization.Path = path;
                    localization.Assembly = sourceAssembly;

                    _availableLocalizations.Add(localization);
                }
            }
            ReloadLocalization();
        }

        /// <summary>
        /// Reloads the localization in case the dictionaries have changed or a service has added additional resources.
        /// </summary>
        private void ReloadLocalization()
        {
            if (CurrentLocalization != null)
            {
                var temp = CurrentLocalization;
                CurrentLocalization = null;
                CurrentLocalization = temp;
            }
        }

        protected ILocalizationHeader? _currentLocalization;
        /// <inheritdoc />
        public ILocalizationHeader? CurrentLocalization {
            get {
                return _currentLocalization;
            }
            set {
                if (value == _currentLocalization) return;
                LoadLocalization((LocalizationHeader?)value);
            }
        }


        protected void LoadLocalization(LocalizationHeader? header) {
            var old = _currentLocalization;


            if (header != null) {
                var mergedHeaders = _availableLocalizations.Where(
                        l => l.LanguageCodeIetf == header.LanguageCodeIetf ||
                        (
                            //header.LanguageCodeIetf == null &&
                            //l.LanguageCodeIetf == null &&
                            l.LanguageCodeIso639_1 == header.LanguageCodeIso639_1
                        ));

                List<LocalizationFile> mergedDictionaries = new();

                var target = new LocalizationFile();

                foreach (var file in mergedHeaders
                    .OfType<LocalizationHeader>()
                    .OrderByDescending(o => o.Priority)
                    .Select(o => new { Path = o.Path, Assembly = o.Assembly })) {
                    
                    
                    if (file.Assembly == null && !File.Exists(file.Path)) {
                        throw new FileNotFoundException($"File {file.Path} does not exist anymore.");
                    }
                    LocalizationFile? source = null;

                    if (file.Assembly == null) {
                       source = JsonSerializer.Deserialize<LocalizationFile>(File.ReadAllText(file.Path))
                            ?? throw new NullReferenceException("The deserialized localization file returned null.");

                    } else {
                        source = JsonSerializer.Deserialize<LocalizationFile>(file.Assembly.GetManifestResourceStream(file.Path) ??
                            throw new InvalidOperationException($"Resource {file.Path} returned an empty stream."));
                    }

                    if (source == null) throw new InvalidOperationException($"Resource {file.Path} returned an empty resource.");
                    target.DefaultLanguageName = source.DefaultLanguageName;
                    target.LocalizedLanguageName = source.LocalizedLanguageName;
                    target.IsRightToLeft = source.IsRightToLeft;
                    target.LanguageCodeIso639_1 = source.LanguageCodeIso639_1;
                    foreach (var group in source.Translations) {
                        if (!target.Translations.ContainsKey(group.Key)) {
                            target.Translations.Upsert(group.Key, group.Value);
                        }

                        foreach (var id in group.Value) {
                            target.Translations[group.Key].Upsert(id.Key, id.Value);
                        }
                    }

                }
                _currentLocalizationDictionary = target;
            } else {
                _currentLocalizationDictionary = null;
            }


            _currentLocalization = header;
            LocalizationChanged?.Invoke(this, new LocalizationChangedEventArgs(old, header));
        }

        /// <inheritdoc/>
        public string? GetTranslation(string group, string id) {
            return _currentLocalizationDictionary?.GetTranslation(group, id);
        }
        /// <inheritdoc/>
        public bool SetTranslation(string group, string id, string? value) {
            return _currentLocalizationDictionary?.SetTranslation(group, id, value) ?? false;
        }


    }
}