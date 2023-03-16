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

**localizationDirectories**:
A list of directories and files that contain your translation files.

**searchPattern**: If you pass directories with `localizationDirectories`, you can pass a file filter (e.g. `*.json`)

**options**: You can specify, how to deal with directories (e.g. find files in subdirectories etc.)

### Function AddLocalizations (Embedded resources)
To load localization files from an embedded resource, use the overloaded function.

**Assembly^^: Specifies the assembly where the resources are located

**localizationNamespaces**: Specifies the namespace (and subnamespaces) where the resources are located.

**searchPattern**: If specified, only the files that match the searchPattern (e.g. Localization.*.json) are loaded.

