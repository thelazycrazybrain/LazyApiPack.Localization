using System.Transactions;

namespace LazyApiPack.Localization
{
    /// <summary>
    /// Contains metadata that describes a localization file
    /// </summary>
    public interface ILocalizationHeader
    {
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
        /// Four letter language code (e.g. de-DE, en-US)
        /// </summary>
        string LanguageCodeIetf { get; set; }
        /// <summary>
        /// Two letter language code (e.g. de, en)
        /// </summary>
        string LanguageCodeIso639_1 { get; set; }
        /// <summary>
        /// Indicates, that the language is written right-to-left (e.g. Hebrew or Arabic)
        /// </summary>
        bool IsRightToLeft { get; set; }
    }
}