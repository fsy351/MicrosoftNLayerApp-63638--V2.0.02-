

namespace Microsoft.Samples.NLayerApp.Infrastructure.Data.MainModule.Repositories
{
    using Microsoft.Samples.NLayerApp.Domain.MainModule.Aggregates.CountryAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Data.Core;
    using Microsoft.Samples.NLayerApp.Infrastructure.Data.MainModule.UnitOfWork;

    /// <summary>
    /// The country repository implementation
    /// </summary>
    public class CountryRepository
        :Repository<Country>,ICountryRepository
    {
        #region Constructor

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="unitOfWork">Associated unit of work</param>
        public CountryRepository(IMainModuleUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        #endregion
    }
}
