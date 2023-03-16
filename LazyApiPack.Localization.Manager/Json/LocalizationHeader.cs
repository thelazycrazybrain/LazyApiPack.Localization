using System.Reflection;
using System.Text.Json.Serialization;

namespace LazyApiPack.Localization.Json
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    /// <inheritdoc/>
    public class LocalizationHeader : ILocalizationHeader
    {
        /// <summary>
        /// Assembly that contains the embedded localization resource (For internal use only!).
        /// </summary>
        [JsonIgnore]
        public Assembly? Assembly { get; set; }
        /// <summary>
        /// Path to the localization file (For internal use only!).
        /// </summary>
        [JsonIgnore]
        public string Path { get; set; }
        /// <inheritdoc/>
        public int Priority { get; set; }
        /// <inheritdoc/>
        public string LocalizedLanguageName { get; set; }
        /// <inheritdoc/>
        public string DefaultLanguageName { get; set; }
        /// <inheritdoc/>
        public string LanguageCodeIetf { get; set; }
        /// <inheritdoc/>
        public string LanguageCodeIso639_1 { get; set; }
        /// <inheritdoc/>
        public bool IsRightToLeft { get; set; }

    }


#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}