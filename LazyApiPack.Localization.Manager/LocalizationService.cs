using LazyApiPack.Localization.Json;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using LazyApiPack.Collections.Extensions;
using System.Linq;
using System.IO;
using System.Threading.Tasks.Sources;
using System.Text.Json.Serialization;

namespace LazyApiPack.Localization.Manager
{
    public class LocalizationService : ILocalizationService
    {
        /// <inheritdoc/>
        public event LocalizationChangedEventHandler? LocalizationChanged;

        /// <summary>
        /// Contains the current translations for the selected localization (Module, Group, Id, (Value, Priority)
        /// </summary>
        private Dictionary<string, Dictionary<string, Dictionary<string, LocalizationEntry>>> _currentDictionary = new();

        /// <summary>
        /// Contains the loaded headers with their respective module name.
        /// </summary>
        private Dictionary<string, List<ILocalizationHeader>> _modules = new Dictionary<string, List<ILocalizationHeader>>();

        ILocalizationHeader? _currentLocalization;
        /// <inheritdoc/>
        public ILocalizationHeader? CurrentLocalization
        {
            get => _currentLocalization;
            set
            {
                var previous = _currentLocalization;
                _currentLocalization = value;
                ChangeLocalization(previous, value);

            }
        }

        private List<ILocalizationHeader> _availableLocalizations = new List<ILocalizationHeader>();
        /// <inheritdoc/>
        public IEnumerable<ILocalizationHeader> AvailableLocalizations => _availableLocalizations;

        /// <inheritdoc/>
        public void AddLocalizations([DisallowNull] IEnumerable<string> localizationFiles)
        {
            var localizationHeaders = LoadHeadersFromFile(localizationFiles);
            var modules = localizationHeaders.GroupBy(g => g.ModuleId);
            foreach (var module in modules)
            {
                if (!_modules.ContainsKey(module.Key))
                {
                    _modules.Add(module.Key, new List<ILocalizationHeader>());
                }

                foreach (var language in module)
                {
                    if (!_modules[module.Key].Any(l => l.LanguageCode == language.LanguageCode))
                    {
                        _modules[module.Key].Add(language);
                    }
                    else if (_modules[module.Key].Any(l => l.LanguageCode == language.LanguageCode && l.Priority < language.Priority))
                    {
                        _modules[module.Key].Add(language);
                    }
                }
            }
            UpdateAvailableLocalizations();
            ChangeLocalization(CurrentLocalization, CurrentLocalization);
        }

        /// <inheritdoc/>
        public void AddLocalizations([DisallowNull] string localizationFile)
        {
            AddLocalizations(new[] { localizationFile });
        }

        /// <inheritdoc/>
        public void RemoveModule(string moduleId)
        {
            if (_modules.ContainsKey(moduleId))
            {
                _modules.Remove(moduleId);
            }
            UpdateAvailableLocalizations();
            ChangeLocalization(CurrentLocalization, CurrentLocalization);
        }

        /// <inheritdoc/>
        public string? GetTranslation(string group, string id)
        {
            if (_currentDictionary.Any(d => d.Value.ContainsKey(group)))
            {
                var selectedGroup = _currentDictionary.First(d => d.Value.ContainsKey(group)).Value[group];
                if (selectedGroup.ContainsKey(id))
                {
                    return selectedGroup[id].Value;
                }
            }


            return null;
        }

        /// <summary>
        /// Updates the list of available localizations
        /// </summary>
        private void UpdateAvailableLocalizations()
        {
            var languageHeaders = _modules.SelectMany(l => l.Value).Distinct(new LocalizationHeaderComparer());
            _availableLocalizations = languageHeaders.ToList();
        }

        /// <summary>
        /// Loads the headers from the localization files.
        /// </summary>
        /// <param name="localizationFiles">List of localization files</param>
        /// <returns>List of the localization headers without translations.</returns>
        private IEnumerable<LocalizationHeader> LoadHeadersFromFile(IEnumerable<string> localizationFiles)
        {
            foreach (var file in localizationFiles)
            {
                var header = JsonSerializer.Deserialize<LocalizationHeader>(File.ReadAllText(file)) ?? throw new NullReferenceException($"Deserialization of the file '{file}' resulted in NULL.");
                header.File = file;
                yield return header;
            }
        }

        /// <summary>
        /// Selects the headers for the specific language
        /// </summary>
        /// <param name="languageCode">Language code that is also represented in the AvailableLocalizations.</param>
        /// <returns>A List of localization headers for the selected localization or their respective default localization.</returns>
        private IEnumerable<ILocalizationHeader> GetHeaders(string languageCode)
        {
            foreach (var module in _modules)
            {
                if (module.Value.Any(v => v.LanguageCode == languageCode))
                {
                    foreach (var content in module.Value.Where(l => l.LanguageCode == languageCode))
                    {
                        yield return content;
                    }
                }
                else if (module.Value.Any(v => v.IsDefault))
                {
                    foreach (var content in module.Value.Where(l => l.IsDefault))
                    {
                        yield return content;
                    }
                }

            }
        }

        /// <summary>
        /// Updates the current localization
        /// </summary>
        /// <param name="oldLocalization">Header of the old localization.</param>
        /// <param name="newLocalization">Header of the new localization.</param>
        private void ChangeLocalization(ILocalizationHeader? oldLocalization, ILocalizationHeader? newLocalization)
        {
            if (newLocalization == null || !AvailableLocalizations.Any(h => h.LanguageCode == newLocalization.LanguageCode))
            {
                return; // Language not set or is not available anymore.
            }

            _currentDictionary.Clear();
            foreach (var header in GetHeaders(newLocalization.LanguageCode))
            {
                if (!_currentDictionary.ContainsKey(header.ModuleId))
                {
                    _currentDictionary.Add(header.ModuleId, new());
                }

                var dictionary = JsonSerializer.Deserialize<LocalizationFile>(File.ReadAllText(((LocalizationHeader)header).File));
                foreach (var group in dictionary.Translations)
                {
                    if (!_currentDictionary[header.ModuleId].ContainsKey(group.Key))
                    {
                        _currentDictionary[header.ModuleId].Add(group.Key, new());
                    }
                    foreach (var id in group.Value)
                    {
                        if (!_currentDictionary[header.ModuleId][group.Key].ContainsKey(id.Key))
                        {
                            _currentDictionary[header.ModuleId][group.Key].Add(id.Key, new LocalizationEntry(id.Value, dictionary.Priority));
                        }
                        else if (_currentDictionary[header.ModuleId][group.Key][id.Key].Priority < dictionary.Priority)
                        {
                            _currentDictionary[header.ModuleId][group.Key][id.Key] = new LocalizationEntry(id.Value, dictionary.Priority);
                        }

                    }
                }
            }

            OnLocalizationChanged(oldLocalization, newLocalization);

        }

        /// <summary>
        /// Raises the LocalizationChanged event if the event is used.
        /// </summary>
        /// <param name="oldLocalization">Header of the old localization.</param>
        /// <param name="newLocalization">Header of the new localization.</param>
        protected virtual void OnLocalizationChanged(ILocalizationHeader? oldLocalization, ILocalizationHeader? newLocalization)
        {
            LocalizationChanged?.Invoke(this, new LocalizationChangedEventArgs(oldLocalization, newLocalization));
        }
    }

    /// <summary>
    /// Compares localizations by language code
    /// </summary>
    internal class LocalizationHeaderComparer : IEqualityComparer<ILocalizationHeader>
    {
        public bool Equals(ILocalizationHeader? x, ILocalizationHeader? y)
        {
            return x?.LanguageCode == y?.LanguageCode;
        }

        public int GetHashCode([DisallowNull] ILocalizationHeader obj)
        {
            return obj.LanguageCode.GetHashCode();
        }
    }

    /// <summary>
    /// Represents the localization with the content and the priority
    /// </summary>
    internal struct LocalizationEntry
    {
        public LocalizationEntry(string? value, int priority)
        {
            Value = value;
            Priority = priority;
        }

        public string? Value { get; internal set; }
        public int Priority { get; internal set; }
    }
}