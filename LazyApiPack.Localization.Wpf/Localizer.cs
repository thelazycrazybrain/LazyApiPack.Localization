using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

namespace LazyApiPack.Localization.Wpf
{
    public abstract class LocalizerMarkupExtension : MarkupExtension
    {
        protected static ILocalizationService _localizationService;
        public static void Initialize(ILocalizationService service)
        {
            _localizationService = service;
        }

        private object _targetObject;
        private object _targetProperty;

        protected object TargetObject
        {
            get { return _targetObject; }
        }

        protected object TargetProperty
        {
            get { return _targetProperty; }
        }


        public sealed override object ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (target != null)
            {
                _targetObject = target.TargetObject;
                _targetProperty = target.TargetProperty;
            }

            return ProvideValueInternal(serviceProvider);
        }

        protected void UpdateValue(object value)
        {
            if (_targetObject != null)
            {
                if (_targetProperty is DependencyProperty)
                {
                    DependencyObject obj = _targetObject as DependencyObject;
                    DependencyProperty prop = _targetProperty as DependencyProperty;

                    Action updateAction = () => obj.SetValue(prop, value);

                    // Check whether the target object can be accessed from the
                    // current thread, and use Dispatcher.Invoke if it can't

                    if (obj.CheckAccess())
                        updateAction();
                    else
                        obj.Dispatcher.Invoke(updateAction);
                }
                else // _targetProperty is PropertyInfo
                {
                    PropertyInfo prop = _targetProperty as PropertyInfo;
                    prop.SetValue(_targetObject, value, null);
                }
            }
        }

        protected abstract object ProvideValueInternal(IServiceProvider serviceProvider);
    }

    public sealed class Localizer : LocalizerMarkupExtension
    {
        string _group;
        string _id;
        public Localizer(string groupAndId)
        {

            var segments = groupAndId?.Split(new[] { '.', '\\', '/' },
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (segments.Length == 2)
            {
                _group = segments[0];
                _id = segments[1];
            }
            else
            {
                throw new ArgumentException("Use the parameter in format Group.Id", nameof(groupAndId));
            }

            if (_localizationService != null)
            {
                _localizationService.LocalizationChanged +=_localizationService_LocalizationChanged;
            }
        }

        private void _localizationService_LocalizationChanged(object sender, LocalizationChangedEventArgs e)
        {
            UpdateValue(GetValue());
        }

        protected override object ProvideValueInternal(IServiceProvider serviceProvider)
        {
            return GetValue();

        }

        private string GetValue()
        {
            if (_localizationService == null)
            {
                return _group + "." + _id;
            }
            else
            {
#pragma warning disable CS8603 // Possible null reference return.
                return _localizationService.GetTranslation(_group, _id);
#pragma warning restore CS8603 // Possible null reference return.
            }
        }
    }
}
