

namespace Microsoft.Samples.NLayerApp.Infrastructure.Data.MainModule.Repositories
{
    using Microsoft.Samples.NLayerApp.Domain.MainModule.Aggregates.ProductAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Data.Core;
using Microsoft.Samples.NLayerApp.Infrastructure.Data.MainModule.UnitOfWork;

    /// <summary>
    /// Product repository implementation
    /// </summary>
    public class ProductRepository
        :Repository<Product>,IProductRepository
    {
        #region Constructor

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="unitOfWork">Associated unit of work</param>
        public ProductRepository(IMainModuleUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        #endregion
    }
}
