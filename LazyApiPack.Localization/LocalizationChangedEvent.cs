namespace LazyApiPack.Localization
{  /// <summary>
   /// Is used to signal that the localization has changed.
   /// </summary>
   /// <param name="sender">The localization service instance which changed the localization</param>
   /// <param name="e">The information which localization has changed.</param>
    public delegate void LocalizationChangedEventHandler(object sender, LocalizationChangedEventArgs e);

    /// <summary>
    /// Details about the localization change.
    /// </summary>
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