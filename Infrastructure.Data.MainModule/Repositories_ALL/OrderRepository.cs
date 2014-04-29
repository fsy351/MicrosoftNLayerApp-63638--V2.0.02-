

namespace Microsoft.Samples.NLayerApp.Infrastructure.Data.MainModule.Repositories
{
    using Microsoft.Samples.NLayerApp.Domain.MainModule.Aggregates.OrderAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Data.Core;
    using Microsoft.Samples.NLayerApp.Infrastructure.Data.MainModule.UnitOfWork;

    /// <summary>
    /// The order repository implementation
    /// </summary>
    public class OrderRepository
        :Repository<Order>,IOrderRepository
    {
        #region Constructor

        /// <summary>
        /// Create a new instance of this repository
        /// </summary>
        /// <param name="unitOfWork">Associated unit of work</param>
        public OrderRepository(IMainModuleUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        #endregion
    }
}
