using LazyApiPack.Localization.Manager;
using System.Reflection;

namespace LazyApiPack.Localization.Tests
{
    public class Tests
    {
        ILocalizationService _localizationService;
        string _lPath;
        [SetUp]
        public void Setup()
        {
            _localizationService = new LocalizationService();
            var root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _lPath = Path.Combine(root, "Localizations");
        }

        [Test]
        public void TestLanguageLoadingAndSwitching()
        {

            _localizationService.AddLocalizations(new[] { _lPath }, "*.json", new EnumerationOptions() { RecurseSubdirectories = true });
            Assert.IsTrue(_localizationService.AvailableLocalizations.Any(l => l.LanguageCodeIetf == "de-DE"));
            Assert.IsTrue(_localizationService.AvailableLocalizations.Any(l => l.LanguageCodeIetf =="en-UK"));

            _localizationService.CurrentLocalization = _localizationService.AvailableLocalizations.First(l => l.LanguageCodeIetf == "en-UK");
            Assert.NotNull(_localizationService.CurrentLocalization);
            Assert.That(_localizationService.CurrentLocalization.LanguageCodeIetf == "en-UK");
            Assert.That(_localizationService.GetTranslation("Messages", "SayYes"), Is.EqualTo("Yes!"));


            _localizationService.CurrentLocalization = _localizationService.AvailableLocalizations.First(l => l.LanguageCodeIetf == "de-DE");
            Assert.NotNull(_localizationService.CurrentLocalization);
            Assert.That(_localizationService.CurrentLocalization.LanguageCodeIetf == "de-DE");
            Assert.That(_localizationService.GetTranslation("Messages", "SayYes"), Is.EqualTo("Jaaaaaaaa (override)!")); // Merged!

            Assert.That(_localizationService.GetTranslation("Alerts", "WarnLowPressure"), Is.EqualTo("Achtung, geringer druck!"));
            Assert.Pass();
        }

        //[Test]
        //public void TestAddTranslationToFile()
        //{
        //    var deClone = Path.Combine(_lPath, "de-DE_modified.json");
        //    if (File.Exists(deClone))
        //    {
        //        File.Delete(deClone);
        //    }

        //    File.Copy(Path.Combine(_lPath, "de-DE.json"), deClone);
        //    _localizationService.Initialize(new[] { deClone }, "*.json", new EnumerationOptions() { RecurseSubdirectories = true });


        //    _localizationService.CurrentLocalization = _localizationService.AvailableLocalizations["de-DE"];
        //    _localizationService.SetTranslation("MyNewGroup", "MyNewMessage", "Test");
        //    _localizationService.SetTranslation("MyNewGroup", "MyNewMessage2", "Test2");

        //    Assert.That(_localizationService.GetTranslation("Captions", "MainTitle"), Is.EqualTo("Meine Anwendung"));

        //    _localizationService.SetTranslation("Captions", "MainTitle", "Meine Neue Anwendung");
        //    Assert.That(_localizationService.GetTranslation("Captions", "MainTitle"), Is.EqualTo("Meine Neue Anwendung")));



        //    File.Delete(deClone);


        //}
    }
}