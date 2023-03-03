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

namespace LazyApiPack.Localization.Wpf.Example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ILocalizationService Service = new LocalizationService();
        public MainWindow()
        {
            DataContext = this;
            // Bootstrap
           
            LocalizerMarkupExtension.Initialize(Service);
            var root = IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var lPath = IO.Path.Combine(root, "Localizations");


            Service.Initialize(new[] { lPath }, "*.json");
            Service.CurrentLocalization = Service.AvailableLocalizations["de-DE"];


            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(Service.CurrentLocalization?.LanguageCodeIetf == "de-DE")
            {
                Service.CurrentLocalization = Service.AvailableLocalizations["en-UK"];
            }else
            {
                Service.CurrentLocalization = Service.AvailableLocalizations["de-DE"];
            }
        }
    }
}
