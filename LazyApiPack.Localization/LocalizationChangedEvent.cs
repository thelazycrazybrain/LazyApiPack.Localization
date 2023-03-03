namespace LazyApiPack.Localization
{
    public delegate void LocalizationChangedEventHandler(object sender, LocalizationChangedEventArgs e);
    public class LocalizationChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Contains the old language header.
        /// </summary>
        public ILocalizationHeader? OldLocalization { get; private set; }
        /// <summary>
        /// Contains the new language header.
        /// </summary>
        public ILocalizationHeader? NewLocalization { get; private set; }

        public LocalizationChangedEventArgs(ILocalizationHeader? oldLocalization, ILocalizationHeader? newLocalization)
        {
            OldLocalization = oldLocalization;
            NewLocalization = newLocalization;
        }
    }
}