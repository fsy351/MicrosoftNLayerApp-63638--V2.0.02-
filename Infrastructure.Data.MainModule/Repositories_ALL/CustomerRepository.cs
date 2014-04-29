
namespace Microsoft.Samples.NLayerApp.Infrastructure.Data.MainModule.Repositories
{
    using Microsoft.Samples.NLayerApp.Domain.MainModule.Aggregates.CustomerAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Data.Core;
    using Microsoft.Samples.NLayerApp.Infrastructure.Data.MainModule.UnitOfWork;


    /// <summary>
    /// The customer repository implementation
    /// </summary>
    public class CustomerRepository
        : Repository<Customer>, ICustomerRepository
    {
        #region Constructor

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="unitOfWork">Associated unit of work</param>
        public CustomerRepository(IMainModuleUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        #endregion
    }
}
