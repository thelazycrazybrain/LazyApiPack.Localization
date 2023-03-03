using LazyApiPack.Localization.Json;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Formats.Asn1;
using System.Security.Cryptography;
using System.Text.Json;

namespace LazyApiPack.Localization.Manager
{
    public class LocalizationService : ILocalizationService
    {
        public event LocalizationChangedEventHandler? LocalizationChanged;
        private readonly Dictionary<string, ILocalizationHeader> _availableLocalizations
            = new Dictionary<string, ILocalizationHeader>();

        private ILocalization? _currentLocalizationDictionary;
        /// <summary>
        /// Creates an instance of the LocalizationService
        /// </summary>
        public LocalizationService()
        {

        }

        public void Initialize([NotNull] string[] localizationDirectories, string? searchPattern = null, EnumerationOptions? options = null)
        {
            _availableLocalizations.Clear();
            _currentLocalization = null;
            _currentLocalizationDictionary = null;
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
                    _availableLocalizations.Add(localization.LanguageCodeIetf, localization);
                }
            }
        }

        public ReadOnlyDictionary<string, ILocalizationHeader> AvailableLocalizations
            => new ReadOnlyDictionary<string, ILocalizationHeader>(_availableLocalizations);

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
                var file = header.FilePath;
                if (!File.Exists(file))
                {
                    throw new FileNotFoundException($"File {file} does not exist anymore.");
                }

                _currentLocalizationDictionary = JsonSerializer.Deserialize<LocalizationFile>(File.ReadAllText(file))
                    ?? throw new NullReferenceException("The deserialized localization file returned null.");
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