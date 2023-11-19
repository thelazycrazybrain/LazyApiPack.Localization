using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace LazyApiPack.Localization.Json
{
    // Localization Json Format v1.0
    /// <inheritdoc/>
    public class LocalizationFile : LocalizationHeader
    {
        /// <summary>
        /// Translations in format: Group - [][[Key -> Translation]] (e.g. Messages -> [][[ "SayYes", "Yes!"], ["SayNo"], "No!"])
        /// </summary>
        [JsonPropertyOrder(9999)]
        public Dictionary<string, Dictionary<string, string>> Translations { get; set; } = new Dictionary<string, Dictionary<string, string>>();

        /// <inheritdoc />
        public string? GetTranslation([DisallowNull] string group, [DisallowNull] string id)
        {
            if (Translations == null)
            {
                return null;
            }

            if (!Translations.ContainsKey(group) || !Translations[group].ContainsKey(id))
            {
                return null;
            }

            return Translations[group][id];

        }

    }

}