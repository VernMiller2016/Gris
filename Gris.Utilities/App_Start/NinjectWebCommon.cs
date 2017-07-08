[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Gris.Utilities.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Gris.Utilities.App_Start.NinjectWebCommon), "Stop")]

namespace Gris.Utilities.App_Start
{
    using Application.Core.Interfaces;
    using Application.Core.Services;
    using Domain.Core.Models;
    using Infrastructure.Core;
    using Infrastructure.Core.DAL;
    using Infrastructure.Core.Interfaces;
    using Infrastructure.Core.Repositories;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using System;
    using System.Web;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        public static object CurrentDbContext
        {
            get
            {
                if (currentKernal == null)
                {
                    return null;
                }
                return currentKernal.Get(typeof(ApplicationDbContext));
            }
        }

        private static StandardKernel currentKernal = null;

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                currentKernal = kernel;
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ApplicationDbContext>().ToSelf().InRequestScope();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();

            kernel.Bind<IExportingService>().To<ExcelExportingService>();

            kernel.Bind<IServerRepository>().To<ServerRepository>();
            kernel.Bind<IServerService>().To<ServerService>();

            kernel.Bind<IPaySourceRepository>().To<PaySourceRepository>();
            kernel.Bind<IPaySourceService>().To<PaySourceService>();

            kernel.Bind<IProgramRepository>().To<ProgramRepository>();
            kernel.Bind<IProgramService>().To<ProgramService>();

            kernel.Bind<IServerTimeEntryRepository>().To<ServerTimeEntryRepository>();
            kernel.Bind<IServerTimeEntryService>().To<ServerTimeEntryService>();

            kernel.Bind<IRepository<ServerAvailableHour>>().To<EFRepository<ServerAvailableHour>>();
            kernel.Bind<IServerAvailableHourService>().To<ServerAvailableHourService>();

            kernel.Bind<IRepository<Element>>().To<EFRepository<Element>>();
            kernel.Bind<IElementService>().To<ElementService>();

            kernel.Bind<IServerSalaryReportRepository>().To<ServerSalaryReportRepository>();
            kernel.Bind<IServerSalaryReportService>().To<ServerMonthlyReportService>();
        }
    }
}
