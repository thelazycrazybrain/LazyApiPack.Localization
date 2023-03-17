using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Localization.Wpf.Editor.Models
{
    internal class SelectableLocalizationItem
    {
        public SelectableLocalizationItem()
        {
            LanguageIetf= "de-DE";
            LanguageIso = "de";
        }
        public bool? IsChecked { get; set; }
        public string LanguageIetf { get; set; }
        public string LanguageIso { get; set; }
        public string LanguageFriendlyName { get => $"{LanguageIso} ({LanguageIetf})"; }
    }
}
