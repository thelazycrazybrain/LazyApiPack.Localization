# About this project
This library provides functionality to support localization for your applications.

Example to use this library:
```xaml
<Window  xmlns:loc="clr-namespace:LazyApiPack.Localization.Wpf;assembly=LazyApiPack.Localization.Wpf">
 <Button Content="{loc:Localizer Captions.MainTitle}" 
		 HorizontalContentAlignment="{loc:HorizontalAlignmentLocalizer}" />
</Window>
```

In the example shown above, you import the `Localization.Wpf` to get access to the `HorizontalAlignmentLocalizer`, `HorizontalTextAlignmentLocalizer`, both used for right-to-left text support
and the Localizer itself to get the localized text via `Group.Id`.

To get the localized text for group "Captions" and Id "MainTitle", use the markup extension `{loc:Localizer Captions.MainTitle}`.
