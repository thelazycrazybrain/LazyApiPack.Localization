using System;
using System.Transactions;

namespace LazyApiPack.Localization
{
    /// <summary>
    /// Contains metadata that describes a localization file
    /// </summary>
    public interface ILocalizationHeader
    {
        /// <summary>
        /// If the selected language is not supported by the module, this language file is used instead.
        /// </summary>
        bool IsDefault { get; set; }
        /// <summary>
        /// Specifies the module id.
        /// </summary>
        string ModuleId { get; set; }
        /// <summary>
        /// Used for localization merging. If the value is higher than other files, the other files override values from this file.
        /// </summary>
        int Priority { get; set; }
        /// <summary>
        /// Name of the language in the language of this localization file (e.g. Deutsch (Deutschland))
        /// </summary>
        string LocalizedLanguageName { get; set; }
        /// <summary>
        /// Name of the language in the application default language (e.g. German (Germany))
        /// </summary>
        string DefaultLanguageName { get; set; }
        /// <summary>
        /// Language code (e.g. de-DE, en-US, no)
        /// </summary>
        string LanguageCode { get; set; }
        /// <summary>
        /// Indicates, that the language is written right-to-left (e.g. Hebrew or Arabic)
        /// </summary>
        bool IsRightToLeft { get; set; }

    }
}