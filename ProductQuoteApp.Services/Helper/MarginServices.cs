using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Services
{

    public enum MarginTypes
    {
        [Description("Indefinido")]
        MarginUndefined = 1,
        [Description("Ingreso Manual")]
        MarginInput = 2,
        [Description("Modalidad de Venta")]
        MarginSaleModality = 3,
        [Description("Cliente")]
        MarginCustomer = 4,
        [Description("Producto")]
        MarginProduct = 5
    }

    public class MininumMarginSale
    {
        public decimal MinimumMarginPercentage { get; set; }
        public decimal MinimumMarginUSD { get; set; }
        public string MinimumMarginSource { get; set; }
        public MarginTypes MininumMarginSourceUSD { get; set; }
        public MarginTypes MininumMarginSourcePercentage { get; set; }

        public MininumMarginSale(decimal? minMarginPercentage, decimal? minMarginUSD, MarginTypes mininumMarginSourcePercentage, MarginTypes mininumMarginSourceUSD)
        {
            MinimumMarginPercentage = (decimal)minMarginPercentage;
            MinimumMarginUSD = (decimal)(minMarginUSD != null ? minMarginUSD : 0);
            MininumMarginSourceUSD = mininumMarginSourceUSD;
            MininumMarginSourcePercentage = mininumMarginSourcePercentage;
        }
    }

    public class MaxMinimunMarginSale
    {
        public decimal? MarginValue { get; set; }
        public MarginTypes MarginSource { get; set; }

        public MaxMinimunMarginSale(decimal? marginValue, MarginTypes marginSource)
        {
            MarginValue = marginValue;
            MarginSource = marginSource;
        }
    }

    public class MarginServices : IMarginServices
    {
        private MaxMinimunMarginSale GetMaxMarginPercentage(ProductQuote productQuoteToAdd, Product product, Customer customer, SaleModalityCreditRating saleModalityCreditRating)
        {
            MaxMinimunMarginSale result = null;

            if ((productQuoteToAdd.MargenInput.HasValue) && (productQuoteToAdd.MargenInput >= 0))
            {
                result = new MaxMinimunMarginSale(productQuoteToAdd.MargenInput, MarginTypes.MarginInput);
                return result;
            }

            //Producto y Cliente son NULL
            if ((product.MinimumMarginPercentage == null) && (customer.MinimumMarginPercentage == null))
            {
                if (saleModalityCreditRating != null)
                {
                    result = new MaxMinimunMarginSale(saleModalityCreditRating.MinimumMarginPercentage, MarginTypes.MarginSaleModality);
                }
                else
                {
                    result = new MaxMinimunMarginSale(0, MarginTypes.MarginUndefined);
                }

                return result;
            }

            //
            if ((product.MinimumMarginPercentage.HasValue) && (product.MinimumMarginPercentage >= (customer.MinimumMarginPercentage.HasValue ? (decimal)customer.MinimumMarginPercentage : 0)))
            {
                result = new MaxMinimunMarginSale(product.MinimumMarginPercentage, MarginTypes.MarginProduct);
                return result;
            }

            //
            if ((customer.MinimumMarginPercentage.HasValue) && (customer.MinimumMarginPercentage >= (product.MinimumMarginPercentage.HasValue ? (decimal)product.MinimumMarginPercentage : 0)))
            {
                result = new MaxMinimunMarginSale(customer.MinimumMarginPercentage, MarginTypes.MarginCustomer);
                return result;
            }

            result = new MaxMinimunMarginSale(saleModalityCreditRating.MinimumMarginPercentage, MarginTypes.MarginSaleModality);

            return result;
        }

        private MaxMinimunMarginSale GetMaxMarginUSD(ProductQuote productQuoteToAdd, Product product, Customer customer, SaleModalityCreditRating saleModalityCreditRating)
        {
            MaxMinimunMarginSale result = null;

            //Si viene el margen input anulo el margen USD
            if ((productQuoteToAdd.MargenInput.HasValue) && (productQuoteToAdd.MargenInput >= 0))
            {
                result = new MaxMinimunMarginSale(0, MarginTypes.MarginInput);
                return result;
            }

            if ((product.MinimumMarginUSD == null) && (customer.MinimumMarginUSD == null))
            {
                if (saleModalityCreditRating != null)
                {
                    result = new MaxMinimunMarginSale(saleModalityCreditRating.MinimumMarginUSD, MarginTypes.MarginSaleModality);
                }
                else
                {
                    result = new MaxMinimunMarginSale(0, MarginTypes.MarginUndefined);
                }

                return result;
            }

            if ((product.MinimumMarginUSD.HasValue) && (product.MinimumMarginUSD >= (customer.MinimumMarginUSD.HasValue ? (decimal)customer.MinimumMarginUSD : 0)))
            {
                result = new MaxMinimunMarginSale(product.MinimumMarginUSD, MarginTypes.MarginProduct);
                return result;
            }

            if ((customer.MinimumMarginUSD.HasValue) && (customer.MinimumMarginUSD >= (product.MinimumMarginUSD.HasValue ? (decimal)product.MinimumMarginUSD : 0)))
            {
                result = new MaxMinimunMarginSale(customer.MinimumMarginUSD, MarginTypes.MarginCustomer);
                return result;
            }

            result = new MaxMinimunMarginSale(saleModalityCreditRating.MinimumMarginUSD, MarginTypes.MarginSaleModality);

            return result;
        }

        public MininumMarginSale GetMargenNetoPorcentual(ProductQuote productQuoteToAdd, Product product, Customer customer, SaleModalityCreditRating saleModalityCreditRating)
        {
            MaxMinimunMarginSale maxMarginPercentage = GetMaxMarginPercentage(productQuoteToAdd, product, customer, saleModalityCreditRating);
            MaxMinimunMarginSale maxMarginUSD = GetMaxMarginUSD(productQuoteToAdd, product, customer, saleModalityCreditRating);

            MininumMarginSale result = new MininumMarginSale(maxMarginPercentage.MarginValue, maxMarginUSD.MarginValue, maxMarginPercentage.MarginSource, maxMarginUSD.MarginSource);

            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
