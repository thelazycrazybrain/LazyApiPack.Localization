using System.Text.Json.Serialization;

namespace LazyApiPack.Localization.Json
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    /// <inheritdoc/>
    public class LocalizationHeader : ILocalizationHeader
    {
        [JsonIgnore]
        public string FilePath { get; set; }
        /// <inheritdoc/>

        public string LocalizedLanguageName { get; set; }

        /// <inheritdoc/>
        public string DefaultLanguageName { get; set; }
        /// <inheritdoc/>
        public string LanguageCodeIetf { get; set; }
        /// <inheritdoc/>
        public string LanguageCodeIso639_1 { get; set; }
    }


#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}