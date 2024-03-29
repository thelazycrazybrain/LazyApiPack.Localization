﻿using System.Text.Json.Serialization;

namespace LazyApiPack.Localization.Json
{
    /// <inheritdoc/>
    public class LocalizationHeader : ILocalizationHeader
    {
        /// <inheritdoc/>
        [JsonPropertyOrder(0)]
        public string ModuleId { get; set; }
        /// <inheritdoc/>
        [JsonPropertyOrder(1)]
        public string LanguageCode { get; set; }
        /// <inheritdoc/>
        [JsonPropertyOrder(2)]
        public bool IsDefault { get; set; }
        /// <summary>
        /// Path to the localization file (For internal use only!).
        /// </summary>
        [JsonIgnore]
        internal string File { get; set; }
        [JsonIgnore]
        internal bool FileIsContent { get; set; }
        /// <inheritdoc/>
        [JsonPropertyOrder(3)]
        public int Priority { get; set; }
        /// <inheritdoc/>
        [JsonPropertyOrder(4)]
        public string DefaultLanguageName { get; set; }
        /// <inheritdoc/>
        [JsonPropertyOrder(5)]
        public string LocalizedLanguageName { get; set; }

        /// <inheritdoc/>
        [JsonPropertyOrder(6)]
        public bool IsRightToLeft { get; set; }


    }

}