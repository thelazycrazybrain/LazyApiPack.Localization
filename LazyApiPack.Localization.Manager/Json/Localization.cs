using LazyApiPack.Collections.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace LazyApiPack.Localization.Json
{

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    /// <inheritdoc/>
    public class LocalizationFile : LocalizationHeader, ILocalization
    {
        /// <summary>
        /// Translations in format: Group - [][[Key -> Translation]] (e.g. Messages -> [][[ "SayYes", "Yes!"], ["SayNo"], "No!"])
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> Translations { get; set; } = new Dictionary<string, Dictionary<string, string>>();

        /// <inheritdoc />
        public string? GetTranslation([NotNull] string group, [NotNull] string id)
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
        ///<inheritdoc />
        public bool SetTranslation([NotNull] string group, [NotNull] string id, string? value)
        {
            if (Translations == null) return false;
            Translations.Upsert(group, new Dictionary<string, string>());
            Translations[group].Upsert(id, value);
            return true;
        }
    }


#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}