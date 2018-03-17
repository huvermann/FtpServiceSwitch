
using Prism.Unity;
using System.Windows;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using FtpServiceSwitch.Views;
using FtpServiceSwitch.Services;
using Prism.Events;

namespace FtpServiceSwitch
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            ModuleCatalog catalog = (ModuleCatalog)ModuleCatalog;
        }

        protected override void ConfigureContainer()
        {
            Container.RegisterType<IEventAggregator, EventAggregator>();
            Container.RegisterType<IDienstSwitchService, DienstSwitchService>(new ContainerControlledLifetimeManager(), new InjectionConstructor(new EventAggregator()));
            base.ConfigureContainer();
        }
    }
}