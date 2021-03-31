using Kzrnm.WindowCapture.Images;
using Kzrnm.WindowCapture.ViewModels;
using Kzrnm.WindowCapture.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Generic;

namespace Kzrnm.WindowCapture.DependencyInjection
{
    public class ServiceBuilder : IServiceCollection
    {
        private ServiceBuilder() {
            services.AddSingleton<ImageProvider>();
            services.AddSingleton<IClipboardManager, ClipboardManager>();
            
            services.AddTransient<WindowCapturerViewModel>();
            services.AddTransient<ImageListViewModel>();
            services.AddTransient<ImageSettingsViewModel>();
            services.AddTransient<ImagePreviewWindowViewModel>();
        }
        public static ServiceBuilder Default { get; } = new();
        private readonly ServiceCollection services = new();

        public int Count => this.services.Count;
        public bool IsReadOnly => this.services.IsReadOnly;
        ServiceDescriptor IList<ServiceDescriptor>.this[int index] { get => ((IList<ServiceDescriptor>)this.services)[index]; set => ((IList<ServiceDescriptor>)this.services)[index] = value; }
        int IList<ServiceDescriptor>.IndexOf(ServiceDescriptor item)
            => this.services.IndexOf(item);
        void IList<ServiceDescriptor>.Insert(int index, ServiceDescriptor item)
            => this.services.Insert(index, item);
        void IList<ServiceDescriptor>.RemoveAt(int index)
            => this.services.RemoveAt(index);

        void ICollection<ServiceDescriptor>.Add(ServiceDescriptor item)
            => ((ICollection<ServiceDescriptor>)this.services).Add(item);
        void ICollection<ServiceDescriptor>.Clear()
            => this.services.Clear();
        bool ICollection<ServiceDescriptor>.Contains(ServiceDescriptor item)
           => this.services.Contains(item);

        void ICollection<ServiceDescriptor>.CopyTo(ServiceDescriptor[] array, int arrayIndex)
            => this.services.CopyTo(array, arrayIndex);

        bool ICollection<ServiceDescriptor>.Remove(ServiceDescriptor item)
            => this.services.Remove(item);

        IEnumerator<ServiceDescriptor> IEnumerable<ServiceDescriptor>.GetEnumerator()
            => ((IEnumerable<ServiceDescriptor>)this.services).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
            => ((IEnumerable)this.services).GetEnumerator();
    }
}
