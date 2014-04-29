//===================================================================================
// Microsoft Developer and Platform Evangelism
//=================================================================================== 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// This code is released under the terms of the MS-LPL license, 
// http://microsoftnlayerapp.codeplex.com/license
//===================================================================================

namespace Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Microsoft.Samples.NLayerApp.Domain.Seedwork;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.Resources;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
    
    /// <summary>
    /// Order aggregate root-entity
    /// </summary>
    public class Order
        :Entity,IValidatableObject
    {
        #region Members

        HashSet<OrderLine> _Lines;

        #endregion

        #region Properties

        /// <summary>
        /// Get or set the Order Date
        /// </summary>
        public DateTime OrderDate { get; set; }
       
        /// <summary>
        /// Get or set order delivery date
        /// </summary>
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// True if this order is delivered
        /// </summary>
        public bool IsDelivered { get; set; }


        /// <summary>
        /// Associated customer identifier to this Order
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Get the related customer
        /// </summary>
        public virtual Customer Customer { get; private set; }
            
        /// <summary>
        /// Get or set the shipping information
        /// </summary>
        public virtual ShippingInfo ShippingInformation { get; set; }

        /// <summary>
        /// Get or set related order lines
        /// </summary>
        public virtual ICollection<OrderLine> OrderLines 
        {
            get
            {
                if (_Lines == null)
                    _Lines = new HashSet<OrderLine>();

                return _Lines;
            }
            set
            {
                _Lines = new HashSet<OrderLine>(value);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create a new order line
        /// </summary>
        /// <param name="productId">The product identifier</param>
        /// <param name="amount">the number of items</param>
        /// <param name="discount">the selected discount for this line</param>
        /// <returns>The new order line</returns>
        public OrderLine CreateOrderLine(Guid productId,int amount,decimal unitPrice,decimal discount)
        {
            //check precondition
            if ( amount <= 0
               ||
                productId == Guid.Empty)
            {
                throw new ArgumentException(Messages.exception_InvalidDataForOrderLine);
            }

            //check discount values
            if (discount < 0)
                discount = 0;


            if (discount > 100)
                discount = 100;

           //create and return order line

            return new OrderLine()
            {
                Id = IdentityGenerator.NewSequentialGuid(),
                OrderId = this.Id,
                ProductId = productId,
                Amount = amount,
                Discount = discount,
                UnitPrice = unitPrice
            };
            
        }
        /// <summary>
        /// Create a new order line
        /// </summary>
        /// <param name="product">The associated product</param>
        /// <param name="amount">the number of items</param>
        /// <param name="discount">the selected discount for this line</param>
        /// <returns>The new order line</returns>
        public OrderLine CreateOrderLine(Product product, int amount, decimal unitPrice, decimal discount)
        {
            //check precondition
            if (amount <= 0
               ||
                product.IsTransient())
            {
                throw new ArgumentException(Messages.exception_InvalidDataForOrderLine);
            }

            //check discount values
            if (discount < 0)
                discount = 0;


            if (discount > 100)
                discount = 100;

            //create and return order line

            var line =  new OrderLine()
            {
                OrderId = this.Id,
                Amount = amount,
                Discount = discount,
                UnitPrice = unitPrice
            };

            line.SetProduct(product);

            return line;
        }

        /// <summary>
        /// Add new order-line (Order Details) into this order
        /// </summary>
        /// <param name="line">The new order-line to add</param>
        public void AddOrderLine(OrderLine line)
        {
            if (line == null)
                throw new ArgumentNullException("line");

            //Fix relation
            line.OrderId = this.Id;

            this.OrderLines.Add(line);
        }

        /// <summary>
        /// Link a customer to this order line
        /// </summary>
        /// <param name="customer">The customer to relate</param>
        public void SetCustomer(Customer customer)
        {
            if (customer == null
                ||
                customer.IsTransient())
            {
                throw new ArgumentException(Messages.exception_CannotAssociateTransientOrNullCustomer);
            }
            this.Customer = customer;
            this.CustomerId = customer.Id;
        }

        /// <summary>
        /// Set this order as delivered. This method
        /// changes the delivered date and sets a new state for this order
        /// </summary>
        public void SetOrderAsDelivered()
        {
            this.DeliveryDate = DateTime.UtcNow;
            this.IsDelivered = true;
        }
        /// <summary>
        /// Get the total of the order
        /// </summary>
        /// <returns>The total of the order</returns>
        public decimal GetOrderTotal()
        {
            decimal total = 0M;

            if (OrderLines != null //use OrderLines for lazy loading
                &&
                OrderLines.Any())
            {
                total = OrderLines.Aggregate(total, (t, l) => t += l.TotalLine);
            }

            return total;
        }

        /// <summary>
        /// Check if the total order is less than the Max Credit
        /// </summary>
        /// <returns>True if total order is less thatn the max customer credit, else false</returns>
        public bool IsCreditValidForOrder()
        {
            //Check if amout of order is valid for the customer credit

            decimal customerCredit = this.Customer.CreditLimit;

            if (this.GetOrderTotal() > customerCredit)
                return false;

            //TODO: This is a parametrizable value, you can 
            //set this value in configuration or other system

            decimal maxTotalOrder = 1000000M;

            //Check if total order exceeds  limits 
            if (this.GetOrderTotal() > maxTotalOrder)
                return false;

            return true;
        }

        #endregion

        #region IValidatableObject Members

        /// <summary>
        /// <see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/>
        /// </summary>
        /// <param name="validationContext"><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></param>
        /// <returns><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();


            if ( CustomerId == Guid.Empty)
                validationResults.Add(new ValidationResult(Messages.validation_OrderCustomerIdCannotBenull,
                                                            new string[]{"CustomerId"}));

            return validationResults;
        }

        #endregion
    }
}
