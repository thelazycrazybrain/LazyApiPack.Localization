namespace LazyApiPack.Localization
{
    /// <summary>
    /// Contains metadata that describes a localization file
    /// </summary>
    public interface ILocalizationHeader
    {
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
    }
}