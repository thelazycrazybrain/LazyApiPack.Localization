# About this project
This project provides interfaces, to use the Localization Manager in MVVM Service patterns.

# Localization Interface
Supports functions `GetTranslation` and `SetTranslation`
- `GetTranslation(group, id)` Gets the localization from the Group with the Id (See file structure).
- `SetTranslation(group, id, value)` Sets or updates the localization in the specified Group with the Id. 

# Localization File Structure
The localization file is structured as following:
-  a Header `ILocalizationHeader` that provides Metadata such as Localized Language Name (E.g. עברית) or the Language Ietf / Language ISO 639-1 codes.
- the localization `ILocalization`, that is grouped into `Group.Id`.
  - Group: e.g. Captions or Messages
  - Id: The unique id of a translation e.g. `FileNotFoundMessage` or `MainWindowTitle`

