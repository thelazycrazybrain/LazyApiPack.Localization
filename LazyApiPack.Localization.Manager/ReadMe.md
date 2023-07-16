# About this project
This library provides a way to localize applications.
It uses the JSON format to store the localizations.
Please refer to the LazyApiPack.Localization library documentation to learn, how to use the localization functionality and the file format.

# How to use this library
This library can be uses as a service in an MVVM pattern, since it uses the interface from the `LazyApiPack.Localization` library.

## LocalizationService
This service implements a Read-Only service for localizations
To create an instance, just use DependencyInjection with the `LazyApiPack.Localization` library or create the instance with 
```cs
new LocalizationService();
```
after the instantiation, you need to initialize the service (AddLocalizations).

### Function AddLocalizations (Files)
This method is used to initialize the localization resources.
To add localizations, use the "AddLocalizations" function and pass it a list of file pathes to the localization files.
To use a localization, set the "CurrentLocalization" property to a localization found in AvailableLocalizations eg.
`_localizationService.CurrentLocalization = _localizationService.AvailableLocalizations.First(l => l.LanguageCode == "en");`

## Use localization
To use a localized value, use `_localizationService.GetLocalization("Group", "Id")`
If you need information about the localization attributes like "IsRightToLeft", use the `CurrentLocalization.IsRightToLeft` attribute.

## Remove localization
If you unload a module, you should remove the localizations with the module name
`_localizationManager.RemoveLocalization("MyModule");`
Note, that this also removes overridden translations with the same module name.
Important: Make sure, that the currently selected language is still in one of the dictionaries. If not, all localizations revert to the default localization files.
```csharp
if (!_localizationService.AvailableLocalizations.Any(l => _localizationService.CurrentLocalization?.LanguageCode == l.LanguageCode)`) 
{
	_localizationService.CurrentLocalization = 
		_localizationService.AvailableLocalizations.FirstOrDefault(l => l.LanguageCode == "en") ?? 
			_localizationService.AvailableLocalizations.FirstOrDefault();

}
```
