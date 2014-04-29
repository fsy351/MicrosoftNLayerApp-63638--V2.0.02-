
namespace Microsoft.Samples.NLayerApp.Infrastructure.Data.MainBoundedContext.ERPModule.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Data.MainBoundedContext.UnitOfWork;
    using Microsoft.Samples.NLayerApp.Infrastructure.Data.Seedwork;


    /// <summary>
    /// The customer repository implementation
    /// </summary>
    public class CustomerRepository
        : Repository<Customer>, ICustomerRepository
    {
        #region Members

        IMainBCUnitOfWork _currentUnitOfWork;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="unitOfWork">Associated unit of work</param>
        public CustomerRepository(IMainBCUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _currentUnitOfWork = unitOfWork;
        }

        #endregion

        #region ICustomerRepository Members

        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg.ICustomerRepository"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override Customer Get(Guid id)
        {
            if (id != Guid.Empty)
            {
                var set = _currentUnitOfWork.CreateSet<Customer>();

                return set.Include(c => c.Picture)
                          .Where(c => c.Id == id)
                          .Select(c => c)
                          .SingleOrDefault();
            }
            else
                return null;
        }

        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg.ICustomerRepository"/>
        /// </summary>
        /// <param name="pageIndex"><see cref="Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg.ICustomerRepository"/></param>
        /// <param name="pageCount"><see cref="Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg.ICustomerRepository"/></param>
        /// <returns><see cref="Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg.ICustomerRepository"/></returns>
        public IEnumerable<Customer> GetEnabled(int pageIndex, int pageCount)
        {
            return _currentUnitOfWork.Customers
                                     .Where(c=>c.IsEnabled == true)
                                     .OrderBy(c => c.FullName)
                                     .Skip(pageIndex * pageCount)
                                     .Take(pageCount);
        }

        #endregion
    }
}
