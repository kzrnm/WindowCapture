using Kzrnm.WindowCapture.Images;
using Kzrnm.WindowCapture.Windows;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System.Windows.Controls;

namespace Kzrnm.WindowCapture
{
    public class WindowCaptureModule<T> : IModule where T : ContentControl
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(T));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            if (!containerRegistry.IsRegistered<ImageProvider>())
                containerRegistry.RegisterSingleton<ImageProvider>();
            if (!containerRegistry.IsRegistered<IClipboardManager>())
                containerRegistry.RegisterSingleton<IClipboardManager, ClipboardManager>();
        }
    }
}
