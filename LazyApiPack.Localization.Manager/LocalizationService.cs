using LazyApiPack.Localization.Json;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Formats.Asn1;
using System.Security.Cryptography;
using System.Text.Json;
using LazyApiPack.Collections.Extensions;

namespace LazyApiPack.Localization.Manager
{

    /// <inheritdoc/>
    public class LocalizationService : ILocalizationService
    {
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
        public void AddLocalizations([NotNull] string[] localizationDirectories, string? searchPattern = null, EnumerationOptions? options = null)
        {
            //_availableLocalizations.Clear();
            //_currentLocalization = null;
            //_currentLocalizationDictionary = null;
            searchPattern = searchPattern ?? string.Empty;
            options = options ?? new EnumerationOptions();
            var folders = localizationDirectories.Where(d => Directory.Exists(d));
            var files = localizationDirectories.Where(d => File.Exists(d));


            var localizationFiles = folders.SelectMany(d => Directory.GetFiles(d, searchPattern, options)).ToList();
            localizationFiles.AddRange(files);

            foreach (var file in localizationFiles)
            {
                using (var fHandle = File.OpenRead(file))
                {
                    var localization = JsonSerializer.Deserialize<LocalizationHeader>(fHandle)
                        ?? throw new NullReferenceException("The deserialized localization file returned null.");
                    localization.FilePath = file;
                    _availableLocalizations.Add(localization);
                }
            }
        }



        protected ILocalizationHeader? _currentLocalization;
        /// <inheritdoc />
        public ILocalizationHeader? CurrentLocalization
        {
            get
            {
                return _currentLocalization;
            }
            set
            {
                LoadLocalization((LocalizationHeader?)value);
            }
        }


        protected void LoadLocalization(LocalizationHeader? header)
        {
            var old = _currentLocalization;

            
            if (header != null)
            {
                var mergedHeaders = _availableLocalizations.Where(
                        l => l.LanguageCodeIetf == header.LanguageCodeIetf || 
                        ( 
                            header.LanguageCodeIetf == null && 
                            l.LanguageCodeIetf == null &&
                            l.LanguageCodeIso639_1 == header.LanguageCodeIso639_1
                        ));

                List<LocalizationFile> mergedDictionaries = new();

                var target = new LocalizationFile();

                foreach (var file in mergedHeaders
                    .OfType<LocalizationHeader>()
                    .OrderByDescending(o => o.Priority)
                    .Select(o => o.FilePath))
                {
                    if (!File.Exists(file))
                    {
                        throw new FileNotFoundException($"File {file} does not exist anymore.");
                    }

                    var source =  JsonSerializer.Deserialize<LocalizationFile>(File.ReadAllText(file))
                        ?? throw new NullReferenceException("The deserialized localization file returned null.");

                    target.DefaultLanguageName = source.DefaultLanguageName;
                    target.LocalizedLanguageName = source.LocalizedLanguageName;
                    target.IsRightToLeft = source.IsRightToLeft;
                    target.LanguageCodeIso639_1 = source.LanguageCodeIso639_1;
                    foreach (var group in source.Translations)
                    {
                        if (!target.Translations.ContainsKey(group.Key))
                        {
                            target.Translations.Upsert(group.Key, group.Value);
                        }

                        foreach (var id in group.Value)
                        {
                            target.Translations[group.Key].Upsert(id.Key, id.Value);
                        }
                    }

                }
                _currentLocalizationDictionary = target;
            }
            else
            {
                _currentLocalizationDictionary = null;
            }


            _currentLocalization = header;
            LocalizationChanged?.Invoke(this, new LocalizationChangedEventArgs(old, header));
        }

        /// <inheritdoc/>
        public string? GetTranslation(string group, string id)
        {
            return _currentLocalizationDictionary?.GetTranslation(group, id);
        }
        /// <inheritdoc/>
        public bool SetTranslation(string group, string id, string? value)
        {
            return _currentLocalizationDictionary?.SetTranslation(group, id, value) ?? false;
        }


    }
}