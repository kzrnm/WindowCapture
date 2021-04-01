using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.Windows;
using Xunit;

#pragma warning disable CS0659
namespace Kzrnm.WindowCapture.DependencyInjection
{
    public class IocBehaviorTests
    {
        class DefinedType { public override bool Equals(object obj) => obj is DefinedType; }
        class UndefinedType { public override bool Equals(object obj) => obj is UndefinedType; }
        public IocBehaviorTests()
        {
            var ioc = new Ioc();
            ioc.ConfigureServices(
                new ServiceCollection()
                .AddSingleton<DefinedType>()
                .BuildServiceProvider());
            IocBehavior.Ioc = ioc;
        }

        [UIFact]
        public void FrameworkElementAutoViewModel()
        {
            var obj = new FrameworkElement();
            IocBehavior.SetAutoViewModel(obj, typeof(DefinedType));
            IocBehavior.GetAutoViewModel(obj).Should().Be(typeof(DefinedType));
            obj.DataContext.Should().Be(new DefinedType());
            IocBehavior.SetAutoViewModel(obj, typeof(UndefinedType));
            IocBehavior.GetAutoViewModel(obj).Should().Be(typeof(UndefinedType));
            obj.DataContext.Should().BeNull();
        }

        [UIFact]
        public void DependencyObjectAutoViewModel()
        {
            var obj = new DependencyObject();
            IocBehavior.SetAutoViewModel(obj, typeof(DefinedType));
            IocBehavior.GetAutoViewModel(obj).Should().Be(typeof(DefinedType));
            IocBehavior.SetAutoViewModel(obj, typeof(UndefinedType));
            IocBehavior.GetAutoViewModel(obj).Should().Be(typeof(UndefinedType));
        }
    }
}
