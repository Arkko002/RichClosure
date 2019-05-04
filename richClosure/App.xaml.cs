using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using Autofac.Core;
using richClosure.ViewModels;

namespace richClosure
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IContainer _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            //TODO Clean up DI
            var builder = new ContainerBuilder();

            builder.RegisterType<ObservableCollection<IPacket>>()
                .SingleInstance();

            builder.RegisterType<PacketCollectionViewModel>()
                .WithParameters(new List<Parameter>()
                {
                    new ResolvedParameter((info, context) => info.ParameterType == typeof(ObservableCollection<IPacket>),
                        (info, context) => context.Resolve(typeof(ObservableCollection<IPacket>))),

                })
                .SingleInstance();

            builder.RegisterType<MainWindowViewModel>()
                .WithParameters(new List<Parameter>()
                {
                    new ResolvedParameter(((info, context) => info.ParameterType == typeof(PacketCollectionViewModel)),
                        ((info, context) => context.Resolve(typeof(PacketCollectionViewModel)))),
                    new TypedParameter(typeof(IWindowManager), new WindowManager(new WindowFactory()))                  
                })
                .SingleInstance();

            _container = builder.Build();

            var window = new MainWindow(_container.Resolve<MainWindowViewModel>());
            window.Show();
        }
    }
}
