using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Avalonia;
using PacketSniffer.Packets;
using PacketSniffer.Services;
using ReactiveUI;
using richClosure.Avalonia.Services.Window_Services;
using richClosure.Avalonia.ViewModels;
using richClosure.Avalonia.Views;
using Splat;

namespace richClosure.Avalonia
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());
        }
        
        private IContainer _container;

        //TODO!!! Make all dependencies injectable, remove creation of classes from constructors and provide them through DI
        protected 
        {
            // TODO Clean up DI
            var builder = new ContainerBuilder();

            builder.RegisterType<ObservableCollection<IPacket>>()
                .SingleInstance();

            //TODO Hide PacketQueue and SnifferThreads, remove library config from DI 
            builder.RegisterType<PacketSnifferService>()
                .WithParameters(new List<Parameter>()
                {
                    new ResolvedParameter(
                        (info, context) => info.ParameterType == typeof(ObservableCollection<IPacket>),
                        (info, context) => context.Resolve(typeof(ObservableCollection<IPacket>))),

                    new TypedParameter(typeof(PacketQueue), new PacketQueue()),
                    
                    new TypedParameter(typeof(SnifferThreads), new SnifferThreads())
                });

            builder.RegisterType<PacketCollectionViewModel>()
                .WithParameter(new ResolvedParameter((info, context) => info.ParameterType == typeof(ObservableCollection<IPacket>),
                    (info, context) => context.Resolve(typeof(ObservableCollection<IPacket>))))
                .SingleInstance();

            builder.RegisterType<PacketSnifferViewModel>()
                .WithParameters(new List<Parameter>
                {
                    new ResolvedParameter(
                        (info, context) => info.ParameterType == typeof(PacketSnifferService),
                        (info, context) => context.Resolve(typeof(PacketSnifferService))),
                }) 
                .SingleInstance();

            builder.RegisterType<PacketFilterViewModel>()
                .WithParameter(new ResolvedParameter(
                    ((info, context) => info.ParameterType == typeof(PacketCollectionViewModel)),
                    ((info, context) => context.Resolve(typeof(PacketCollectionViewModel)))))
                .SingleInstance();

            builder.RegisterType<MainWindowViewModel>()
                .WithParameters(new List<Parameter>()
                {
                    new ResolvedParameter(((info, context) => info.ParameterType == typeof(PacketCollectionViewModel)),
                        ((info, context) => context.Resolve(typeof(PacketCollectionViewModel)))),

                    new ResolvedParameter(((info, context) => info.ParameterType == typeof(PacketFilterViewModel)),
                        (info, context) => context.Resolve(typeof(PacketFilterViewModel))),

                    new ResolvedParameter(((info, context) => info.ParameterType == typeof(PacketSnifferViewModel)),
                        (info, context) => context.Resolve(typeof(PacketSnifferViewModel))),

                    new TypedParameter(typeof(IWindowManager), new WindowManager(new WindowFactory()))                  
                })
                .SingleInstance();

            _container = builder.Build();

            var window = new MainWindow(_container.Resolve<MainWindowViewModel>());
            window.Show();
        }
    }
}
