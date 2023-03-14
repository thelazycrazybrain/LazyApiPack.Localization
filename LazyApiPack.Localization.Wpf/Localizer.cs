using System;

namespace LazyApiPack.Localization.Wpf
{
    /// <summary>
    /// Provides a markup extension, that supports xaml localization.
    /// </summary>
    public sealed class Localizer : LocalizerMarkupExtension
    {
        private readonly string? _group;
        private readonly string? _id;
        /// <summary>
        /// Creates an instance of a localizer
        /// </summary>
        /// <param name="groupAndId">Group and Id separated by "." "\" or "/"</param>
        /// <exception cref="ArgumentException">Is thrown when the Group.Id segments are not properly provided.</exception>
        public Localizer(string groupAndId)
        {

            var segments = groupAndId?.Split(new[] { '.', '\\', '/' },
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (segments?.Length == 2)
            {
                _group = segments[0];
                _id = segments[1];
            }
            else
            {
                throw new ArgumentException("Use the parameter in format Group.Id", nameof(groupAndId));
            }

            if (LocalizationService != null)
            {
                LocalizationService.LocalizationChanged += LocalizationService_LocalizationChanged;
            }
        }

        private void LocalizationService_LocalizationChanged(object sender, LocalizationChangedEventArgs e)
        {
            UpdateValue(GetValue());
        }

        protected override object ProvideValueInternal(IServiceProvider serviceProvider)
        {
            return GetValue() ?? String.Empty;

        }

        private string DefaultValue => _group + "." + _id;
        private string GetValue()
        {
            if (LocalizationService == null || _group == null || _id == null)
            {
                return DefaultValue;
            }
            else
            {
                return LocalizationService.GetTranslation(_group, _id) ?? DefaultValue;
            }
        }
    }
}
