
namespace Microsoft.Samples.NLayerApp.Infrastructure.Data.MainModule.Repositories
{
    using Microsoft.Samples.NLayerApp.Domain.MainModule.Aggregates.BankAccountAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Data.Core;
    using Microsoft.Samples.NLayerApp.Infrastructure.Data.MainModule.UnitOfWork;


    /// <summary>
    /// The bank account repository implementation
    /// </summary>
    public class BankAccountRepository
        :Repository<BankAccount>,IBankAccountRepository
    {
        #region Constructor

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="unitOfWork">Associated unit of work</param>
        public BankAccountRepository(IMainModuleUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        #endregion
    }
}
