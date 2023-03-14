using System.Windows.Automation.Text;

namespace LazyApiPack.Localization.Wpf
{

    public sealed class HorizontalTextAlignmentLocalizer : RightToLeftLocalizerMarkupExtension
    {
        public HorizontalTextAlignmentLocalizer() : base()
        {

        }
        public HorizontalTextAlignmentLocalizer(bool invert) : base(invert)
        {

        }
        protected override object ProvideValueInternal(bool isLeft)
        {
           return isLeft ? HorizontalTextAlignment.Left : HorizontalTextAlignment.Right;
        }
    }

}
