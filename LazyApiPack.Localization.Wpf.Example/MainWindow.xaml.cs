using LazyApiPack.Localization.Manager;
using System;
using System.Collections.Generic;
using IO = System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Policy;

namespace LazyApiPack.Localization.Wpf.Example {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public static ILocalizationService Service = new LocalizationService();
        public MainWindow() {
            DataContext = this;
            // Bootstrap

            LocalizerMarkupExtension.Initialize(Service);
            var root = IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var lPath = IO.Path.Combine(root, "Localizations");


            Service.AddLocalizations(new[] { lPath }, "*.json");
            Service.AddLocalizations(Assembly.GetExecutingAssembly(), new[] { "LazyApiPack.Localization.Wpf.Example.Localizations" }, "Localization.*.json");
            NextLocalization();
            Service.CurrentLocalization = Service.AvailableLocalizations.First(l => l.LanguageCodeIetf == "de-DE");


            InitializeComponent();
        }

        int l = 0;
        private void Button_Click(object sender, RoutedEventArgs e) {
            NextLocalization();

        }
        void NextLocalization() {
            if (l >= Service.AvailableLocalizations.Count) {
                l = 0;
            }
            Service.CurrentLocalization = Service.AvailableLocalizations[l];
            l++;
        }
    }
}
