using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.DTOs
{
    /// <summary>
    /// This is the data transfer object for
    /// BankActivity entitiy.The name
    /// of properties for this type
    /// is based on conventions of many mappers
    /// to simplificate the mapping process.
    /// </summary>
    public class BankActivityDTO
    {
        /// <summary>
        /// The activity date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The activity amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The activity description
        /// </summary>
        public string ActivityDescription { get; set; }

    }
}
