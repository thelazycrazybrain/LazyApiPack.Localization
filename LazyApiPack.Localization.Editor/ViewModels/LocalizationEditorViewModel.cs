using LazyApiPack.Localization.Wpf.Editor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Localization.Wpf.Editor.ViewModels
{
    internal class LocalizationEditorViewModel : ViewModelBase
    {
        protected readonly ILocalizationEditorService _editorService;
        protected readonly ILocalizationService _localizationService;
        public LocalizationEditorViewModel(ILocalizationEditorService editorService, ILocalizationService localizationService)
        {
            _editorService = editorService;
            _localizationService = localizationService;
            Localizations= new ObservableCollection<SelectableLocalizationItem>(
                new[] {
                    new SelectableLocalizationItem(),
                    new SelectableLocalizationItem(),
                    new SelectableLocalizationItem(),
                    new SelectableLocalizationItem(),
                    new SelectableLocalizationItem(),
                    new SelectableLocalizationItem() 
                });

        }

        public LocalizationMatrix? Matrix { get; private set; }

        public string? Title { get => _localizationService.GetTranslation("Captions", "LocalizationEditorTitle"); }

        public ObservableCollection<SelectableLocalizationItem> Localizations { get; private set; }
    }

    internal class LocalizationMatrix
    {
        // File (Language)
        //    Group
        //       Id
    }
}

