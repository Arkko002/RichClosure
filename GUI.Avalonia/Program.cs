using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using PacketSniffer.Packets;
using PacketSniffer.Services;
using ReactiveUI;
using richClosure.Avalonia.ViewModels;
using richClosure.Avalonia.Views;
using Splat;
using Splat.Autofac;

namespace richClosure.Avalonia
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args)
        {
            // builder.RegisterType<ObservableCollection<IPacket>>()
            //     .SingleInstance();
            //
            // //TODO Hide PacketQueue and SnifferThreads, remove library config from DI 
            // builder.RegisterType<PacketSnifferService>()
            //     .WithParameters(new List<Parameter>()
            //     {
            //         new ResolvedParameter(
            //             (info, context) => info.ParameterType == typeof(ObservableCollection<IPacket>),
            //             (info, context) => context.Resolve(typeof(ObservableCollection<IPacket>))),
            //
            //         new TypedParameter(typeof(PacketQueue), new PacketQueue()),
            //         
            //         new TypedParameter(typeof(SnifferThreads), new SnifferThreads())
            //     });
            //
            // builder.RegisterType<PacketCollectionViewModel>()
            //     .WithParameter(new ResolvedParameter((info, context) => info.ParameterType == typeof(ObservableCollection<IPacket>),
            //         (info, context) => context.Resolve(typeof(ObservableCollection<IPacket>))))
            //     .SingleInstance();
            //
            // builder.RegisterType<PacketSnifferViewModel>()
            //     .WithParameters(new List<Parameter>
            //     {
            //         new ResolvedParameter(
            //             (info, context) => info.ParameterType == typeof(PacketSnifferService),
            //             (info, context) => context.Resolve(typeof(PacketSnifferService))),
            //     }) 
            //     .SingleInstance();
            //
            // builder.RegisterType<PacketFilterViewModel>()
            //     .WithParameter(new ResolvedParameter(
            //         ((info, context) => info.ParameterType == typeof(PacketCollectionViewModel)),
            //         ((info, context) => context.Resolve(typeof(PacketCollectionViewModel)))))
            //     .SingleInstance();
            //
            // builder.RegisterType<MainWindowViewModel>()
            //     .WithParameters(new List<Parameter>()
            //     {
            //         new ResolvedParameter(((info, context) => info.ParameterType == typeof(PacketCollectionViewModel)),
            //             ((info, context) => context.Resolve(typeof(PacketCollectionViewModel)))),
            //
            //         new ResolvedParameter(((info, context) => info.ParameterType == typeof(PacketFilterViewModel)),
            //             (info, context) => context.Resolve(typeof(PacketFilterViewModel))),
            //
            //         new ResolvedParameter(((info, context) => info.ParameterType == typeof(PacketSnifferViewModel)),
            //             (info, context) => context.Resolve(typeof(PacketSnifferViewModel))),
            //     })
            //     .SingleInstance();

            // TODO Clean up DI
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();
                
            var autofacResolver = builder.UseAutofacDependencyResolver();
            Locator.SetLocator(autofacResolver);
            
            Locator.CurrentMutable.InitializeSplat();
            Locator.CurrentMutable.InitializeReactiveUI();
            
            var container = builder.Build();
            autofacResolver.SetLifetimeScope(container);
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

        } 

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
    }
}