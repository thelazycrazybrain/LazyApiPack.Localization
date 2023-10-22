using LazyApiPack.Localization.Manager;
//using NUnit.Framework.Internal;
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

        const string ACC_MOD_DE = "Localization.AccountingModule.de.json";
        const string ACC_MOD_EN = "Localization.AccountingModule.en.json";
        const string TIM_MOD_DE = "Localization.TimeRecordingModule.de.json";
        const string TIM_MOD_DE_OVERRIDE = "Localization.TimeRecordingModule.New.de.json";
        const string TIM_MOD_NO = "Localization.TimeRecordingModule.no.json";
        [Test]
        public void TestLanguageLoadingAndSwitching()
        {
            var pathes = new[] {
                Path.Combine(_lPath, ACC_MOD_DE),
                Path.Combine(_lPath, ACC_MOD_EN),
                Path.Combine(_lPath, TIM_MOD_DE)
            };

            _localizationService.AddLocalizations(pathes);
            Assert.IsTrue(_localizationService.AvailableLocalizations.Any(l => l.LanguageCode == "de"), "Localization DE is not available.");
            Assert.IsTrue(_localizationService.AvailableLocalizations.Any(l => l.LanguageCode == "en"), "Localization EN is not available.");

            _localizationService.AddLocalizations(new[] { Path.Combine(_lPath, TIM_MOD_NO) });

            Assert.IsTrue(_localizationService.AvailableLocalizations.Any(l => l.LanguageCode == "no"), "Localization NO is not available.");
            Assert.IsTrue(_localizationService.AvailableLocalizations.Any(l => l.LanguageCode == "de"), "Localization DE is not available.");
            Assert.IsTrue(_localizationService.AvailableLocalizations.Any(l => l.LanguageCode == "en"), "Localization EN is not available.");

            _localizationService.CurrentLocalization = _localizationService.AvailableLocalizations.First(l => l.LanguageCode == "en");

            Assert.IsTrue(_localizationService.GetTranslation("CaptionsAcc", "SaveFileDialogCaption") == "Save accounting file",
                "Localization of CaptionsAcc.SaveDialogCaption did not get the expected result");

            Assert.IsTrue(_localizationService.GetTranslation("CaptionsTim", "SaveFileDialogCaption") == "Zeiterfassungsdatei speichern",
                "The translation for Time Recording did not fall back to german as expected.");

            // TODO: Embedded resource is not working in test framework.
            _localizationService.AddLocalizations(new[] { Path.Combine(_lPath, TIM_MOD_DE_OVERRIDE) });

            _localizationService.CurrentLocalization = _localizationService.AvailableLocalizations.First(l => l.LanguageCode == "de");
            Assert.IsTrue(_localizationService.GetTranslation("CaptionsTim", "SaveFileDialogCaption") == "Zeiterfassungsdatei speichern (\u00DCberschrieben)",
                "The translation for Time Recording did not fall back to german and uses the overridden value as expected.");

            Assert.IsTrue(_localizationService.GetTranslation("CaptionsTim", "OpenFileDialogCaption") == "Zeiterfassungsdatei \u00F6ffnen",
                "The translation for Time Recording CaptionsTim.OpenFileDialogCaption was not correctly in the dictionary as expected");

            _localizationService.RemoveModule("TimeRecordingModule");

            Assert.IsFalse(_localizationService.GetTranslation("CaptionsTim", "OpenFileDialogCaption") == "Zeiterfassungsdatei \u00F6ffnen",
                "The translation for Time Recording CaptionsTim.OpenFileDialogCaption was not correctly in the dictionary as expected");

            _localizationService.AddLocalizations(new[] { Path.Combine(_lPath, TIM_MOD_DE) });

            Assert.IsTrue(_localizationService.GetTranslation("CaptionsTim", "OpenFileDialogCaption") == "Zeiterfassungsdatei \u00F6ffnen",
                "The translation for Time Recording CaptionsTim.OpenFileDialogCaption was not correctly in the dictionary as expected");


            Assert.Pass();
        }

    }
}