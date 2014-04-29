

namespace Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.MainBoundedContext.IoC
{

    using Microsoft.Practices.Unity;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.DTOAdapters;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.Services;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOAdapters;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.BankingModule.Services;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Logging;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.NetFramework.Logging;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.NetFramework.Validator;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Validator;
    using Microsoft.Samples.NLayerApp.Infrastructure.Data.MainBoundedContext.BankingModule.Repositories;
    using Microsoft.Samples.NLayerApp.Infrastructure.Data.MainBoundedContext.ERPModule.Repositories;
    using Microsoft.Samples.NLayerApp.Infrastructure.Data.MainBoundedContext.UnitOfWork;
    

    /// <summary>
    /// DI container accesor
    /// </summary>
    public static class Container
    {
        #region Properties

        static  IUnityContainer _currentContainer;

        /// <summary>
        /// Get the current configured container
        /// </summary>
        /// <returns>Configured container</returns>
        public static IUnityContainer Current
        {
            get
            {
                return _currentContainer;
            }
        }

        #endregion

        #region Constructor
        
        static Container()
        {
            ConfigureContainer();

            ConfigureFactories();
        }

        #endregion

        #region Methods

        static void ConfigureContainer()
        {
            /*
             * Add here the code configuration or the call to configure the container 
             * using the application configuration file
             */

            _currentContainer = new UnityContainer();
            
            
            //-> Unit of Work and repositories
            _currentContainer.RegisterType<IMainBCUnitOfWork, MainBCUnitOfWork>(new PerResolveLifetimeManager());

            _currentContainer.RegisterType<IBankAccountRepository, BankAccountRepository>();
            _currentContainer.RegisterType<ICountryRepository, CountryRepository>();
            _currentContainer.RegisterType<ICustomerRepository, CustomerRepository>();
            _currentContainer.RegisterType<IOrderRepository,OrderRepository>();
            _currentContainer.RegisterType<IProductRepository, ProductRepository>();

            //-> Adapters
            _currentContainer.RegisterType<ITypeAdapter, TypeAdapter>();
            _currentContainer.RegisterType<RegisterTypesMap, ERPModuleRegisterTypesMap>("erpmodule");
            _currentContainer.RegisterType<RegisterTypesMap, BankingModuleRegisterTypesMap>("bankingmodule");

            //-> Domain Services
            _currentContainer.RegisterType<IBankTransferService, BankTransferService>();

            //-> Application services
            _currentContainer.RegisterType<ISalesAppService, SalesAppService>();
            _currentContainer.RegisterType<ICustomerAppService, CustomerAppService>();
            _currentContainer.RegisterType<IBankAppService, BankAppService>();

        }


        static void ConfigureFactories()
        {
            LogFactory.SetCurrent(new TraceSourceLogFactory());
            EntityValidatorFactory.SetCurrent(new DataAnnotationsEntityValidatorFactory());
        }

        #endregion
    }
}
