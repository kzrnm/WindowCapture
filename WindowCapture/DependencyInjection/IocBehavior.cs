using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.ComponentModel;
using System.Windows;

namespace Kzrnm.WindowCapture.DependencyInjection
{
    public static class IocBehavior
    {
        public static Type GetAutoViewModel(DependencyObject obj) => (Type)obj.GetValue(AutoViewModelProperty);
        public static void SetAutoViewModel(DependencyObject obj, Type value) => obj.SetValue(AutoViewModelProperty, value);
        public static readonly DependencyProperty AutoViewModelProperty =
            DependencyProperty.RegisterAttached(
                "AutoViewModel",
                typeof(Type),
                typeof(IocBehavior),
                new FrameworkPropertyMetadata(null, 
                    FrameworkPropertyMetadataOptions.NotDataBindable, 
                    AutoViewModelChanged));

        private static void AutoViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(d))
                return;
            if (d is not FrameworkElement elm) return;
            if (e.NewValue is Type type)
            {
                elm.DataContext = Ioc.Default.GetService(type);
            }
            else elm.DataContext = null;
        }
    }
}
