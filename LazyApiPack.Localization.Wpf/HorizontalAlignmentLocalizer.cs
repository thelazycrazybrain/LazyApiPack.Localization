using System.Windows;

namespace LazyApiPack.Localization.Wpf
{
    public sealed class HorizontalAlignmentLocalizer : RightToLeftLocalizerMarkupExtension
    {
        public HorizontalAlignmentLocalizer() : base()
        {

        }
        public HorizontalAlignmentLocalizer(bool invert) : base(invert)
        {

        }
        protected override object ProvideValueInternal(bool isLeft)
        {
            return isLeft ? HorizontalAlignment.Left : HorizontalAlignment.Right;
        }

      
    }

}
