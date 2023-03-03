namespace LazyApiPack.Localization
{
    /// <summary>
    /// Provides functionality to get a translation from a localization file.
    /// </summary>
    public interface ILocalization
    {
        /// <summary>
        /// Gets the translated text for the group and id
        /// </summary>
        /// <param name="group">E.g. "Captions"</param>
        /// <param name="id">E.g. "WelcomeMessageCaption"</param>
        /// <returns>The translated text or null.</returns>
        string? GetTranslation(string group, string id);
        /// <summary>
        /// Adds a translated text to ghe group with the id
        /// </summary>
        /// <param name="group">E.g. "Messages"</param>
        /// <param name="id">E.g. "WelcomeMessageText"</param>
        /// <param name="value"> The translated text or null.</param>
        /// <returns>True, if the translation was successfully added or false, if not.</returns>
        bool SetTranslation(string group, string id, string? value);

    }
}