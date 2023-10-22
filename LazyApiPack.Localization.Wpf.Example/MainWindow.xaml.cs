using LazyApiPack.Localization.Manager;
using IO = System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.IO;

namespace LazyApiPack.Localization.Wpf.Example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly ILocalizationService Service = new LocalizationService();
        public MainWindow()
        {
            DataContext = this;
            // Bootstrap
            var root = IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var lPath = IO.Path.Combine(root, "Localizations");
            Service.AddLocalizations(Directory.GetFiles(lPath, "*.json"));
            Service.AddLocalizations(typeof(MainWindow).Assembly, "LazyApiPack.Localization.Wpf.Example.Localizations.he.json");

            LocalizerMarkupExtension.Initialize(Service);



            NextLocalization();
            Service.CurrentLocalization = Service.AvailableLocalizations.First(l => l.LanguageCode == "de");

            InitializeComponent();
        }



        int l = 0;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NextLocalization();

        }
        void NextLocalization()
        {
            if (l >= Service.AvailableLocalizations.Count())
            {
                l = 0;
            }
            Service.CurrentLocalization = Service.AvailableLocalizations.ElementAt(l);
            l++;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

         

        }
    }
}
