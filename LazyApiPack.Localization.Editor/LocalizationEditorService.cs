using LazyApiPack.Localization.Editor;
using LazyApiPack.Localization.Editor.Windows;
using LazyApiPack.Localization.Wpf.Editor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Localization.Wpf.Editor
{
    public class LocalizationEditorService : ILocalizationEditorService
    {
        protected readonly ILocalizationService _localizationService;
        public LocalizationEditorService(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
            localizationService.AddLocalizations(Assembly.GetExecutingAssembly(), 
                                                 new[] { "LazyApiPack.Localization.Wpf.Editor.Localizations" }, "Localization.*.json");
        }

        LocalizationEditorWindow? _currentWindow;
        public void ShowEditor()
        {
            if (_currentWindow == null)
            {
                _currentWindow = new LocalizationEditorWindow();
                _currentWindow.Closed += (s, e) => _currentWindow = null;
                _currentWindow.DataContext = new LocalizationEditorViewModel(this, _localizationService);
                _currentWindow.Show();
            }
            else
            {
                if (_currentWindow.WindowState == System.Windows.WindowState.Minimized)
                {
                    _currentWindow.WindowState = System.Windows.WindowState.Normal;
                }
                _currentWindow.Focus();
            }
        }

    }
}
