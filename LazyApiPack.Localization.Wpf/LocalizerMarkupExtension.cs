using System;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

namespace LazyApiPack.Localization.Wpf
{
    /// <summary>
    /// Provides basic functionality to support xaml localization.
    /// </summary>
    public abstract class LocalizerMarkupExtension : MarkupExtension
    {
        internal static ILocalizationService? LocalizationService { get; private set; }

        public static void Initialize(ILocalizationService service)
        {
            LocalizationService = service;
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
            var target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
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
                    var obj = _targetObject as DependencyObject;
                    var prop = _targetProperty as DependencyProperty;

                    var updateAction = () => obj?.SetValue(prop, value);

                    if (obj?.CheckAccess() == true)
                        updateAction();
                    else
                        obj?.Dispatcher.Invoke(updateAction);
                }
                else // _targetProperty is PropertyInfo
                {
                    var prop = _targetProperty as PropertyInfo;
                    prop?.SetValue(_targetObject, value, null);
                }
            }
        }

        protected abstract object ProvideValueInternal(IServiceProvider serviceProvider);
    }
}
