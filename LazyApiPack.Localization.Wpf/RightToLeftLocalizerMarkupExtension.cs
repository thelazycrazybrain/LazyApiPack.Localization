using System;

namespace LazyApiPack.Localization.Wpf
{
    /// <summary>
    /// Provides a markup extension that supports Right-To-Left languages.
    /// </summary>
    public abstract class RightToLeftLocalizerMarkupExtension : LocalizerMarkupExtension
    {
        private readonly bool _invert;
        /// <summary>
        /// Creates an instance of the RightToLeftLocalizer.
        /// </summary>
        protected RightToLeftLocalizerMarkupExtension()
        {

            if (LocalizationService != null)
            {
                LocalizationService.LocalizationChanged += LocalizationService_LocalizationChanged;
            }
        }

        /// <summary>
        /// Creates an instance of the RightToLeftLocalizer that supports RTL inversion.
        /// </summary>
        /// <param name="invert">If true and the language is RTL, the outcome is HorizontalTextAlignment.Left.</param>
        protected RightToLeftLocalizerMarkupExtension(bool invert) : this()
        {
            _invert = invert;
        }

        private void LocalizationService_LocalizationChanged(object sender, LocalizationChangedEventArgs e)
        {
            UpdateValue(ProvideValueInternal((bool)GetValue()));
        }

        private object GetValue()
        {
            bool isLeft;
            if (LocalizerMarkupExtension.LocalizationService?.CurrentLocalization != null)
            {
                isLeft = !LocalizerMarkupExtension.LocalizationService.CurrentLocalization.IsRightToLeft;
            }
            else
            {
                isLeft = true;
            }

            if (_invert)
            {
                isLeft = !isLeft;
            }
            
            return isLeft;
        }
        protected override object ProvideValueInternal(IServiceProvider serviceProvider)
        {
            return ProvideValueInternal((bool)GetValue());
        }

        protected abstract object ProvideValueInternal(bool isLeft);
    }

}
