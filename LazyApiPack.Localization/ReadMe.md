# About this project
This project provides interfaces, to use the Localization Manager in MVVM Service patterns.

# Localization Interface
Supports functions `GetTranslation`
- `GetTranslation(group, id)` Gets the localization from the Group with the Id (See file structure).
- `SetTranslation(group, id, value)` Sets or updates the localization in the specified Group with the Id. 

# Localization File Structure
## Header
The localization file has a header with the following attributes
- ModuleId: Specifies the unique name of the module this localization file supports
- IsDefault: If the module does not support the selected language, this file is used instead (Usually english)
- Priority: You can write localization files for modules that are already translated. If you want to override existing localizations in other modules, you can set a higher priority to use your translations instead
- LocalizedLanguageName: The name of the language in the native language (eg. Norsk (Bokmål))
- DefaultLanguageName: The name of the language in the default language (Usually in english)
- LanguageCode: The code for the localization (de, en, etc.) Note: en-US is a different language than en-UK, so the language files will not be mixed but the default localization file is selected if the module does not explicitly supports en-US
- IsRightToLeft: Indicates, if the language should be written from right to left (eg. Hebrew or Arabic)

## Localizations
The following section "Localization" contains a dictionary of translations used in your program.
The hierarchy is "Group" then "Id"

## Example of a localization file
```json
{
  "ModuleId": "AccountingModule",
  "LanguageCode": "en",
  "IsDefault": true,
  "Priority": 0,
  "DefaultLanguageName": "English",
  "LocalizedLanguageName": "English",
  "IsRightToLeft": false,
  "Translations": {
    "CaptionsAcc": {
      "SaveFileDialogCaption": "Save accounting file",
      "OpenFileDialogCaption": "Open accounting file"
    },
    "MessagesAcc": {
      "SaveFileDialogMessage": "Do you want to save the accounting file?",
      "OpenFileDialogMessage": "Do you want to open the accounting file?"
    }
  }
}
```
