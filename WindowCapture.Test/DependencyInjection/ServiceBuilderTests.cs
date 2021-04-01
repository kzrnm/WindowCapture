using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using Xunit;

namespace Kzrnm.WindowCapture.DependencyInjection
{
    public class ServiceBuilderTests
    {
        public static TheoryData EnumerateViewModels()
        {
            var theoryData = new TheoryData<Type>();
            foreach (var t in typeof(ServiceBuilder).Assembly.GetTypes())
                if (t.Namespace.EndsWith(".ViewModels"))
                    theoryData.Add(t);
            return theoryData;
        }
        [Theory]
        [MemberData(nameof(EnumerateViewModels))]
        public void ViewModels(Type type)
        {
            var ioc = new Ioc();
            ioc.ConfigureServices(ServiceBuilder.Default.BuildServiceProvider());
            ioc.GetService(type).Should().BeOfType(type);
        }
    }
}
