using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Autofac;
using Autofac.Core;
using PacketSniffer.Packet_Sniffing;
using PacketSniffer.Packets;
using richClosure.ViewModels;
using richClosure.Views;
using richClosure.Window_Services;

namespace richClosure
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IContainer _container;

        //TODO!!! Make all dependencies injectable, remove creation of classes from constructors and provide them through DI
        protected override void OnStartup(StartupEventArgs e)
        {
            // TODO Clean up DI
            var builder = new ContainerBuilder();

            builder.RegisterType<ObservableCollection<IPacket>>()
                .SingleInstance();

            //TODO Hide PacketQueue and SnifferThreads, remove library config from DI 
            builder.RegisterType<PacketSniffer.Packet_Sniffing.PacketSnifferService>()
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
                        (info, context) => info.ParameterType == typeof(PacketSniffer.Packet_Sniffing.PacketSnifferService),
                        (info, context) => context.Resolve(typeof(PacketSniffer.Packet_Sniffing.PacketSnifferService))),
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
