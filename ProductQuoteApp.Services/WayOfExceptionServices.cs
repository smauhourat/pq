using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductQuoteApp.Persistence;

namespace ProductQuoteApp.Services
{

    public class WayOfExceptionServices : IWayOfExceptionServices
    {
        private IWayOfExceptionRepository wayOfExceptionRepository = null;

        public WayOfExceptionServices(IWayOfExceptionRepository wayOfExceptionRepo)
        {
            wayOfExceptionRepository = wayOfExceptionRepo;
        }

        public WayOfException HasWayOfExceptionProductQuote(ProductQuote productQuote)
        {
            WayOfException ret = null;
            var listWoE = wayOfExceptionRepository.FindWayOfExceptions();

            foreach (WayOfException woe in listWoE)
            {
                if ((woe.CustomerID == productQuote.CustomerID) && (woe.ProductID == productQuote.ProductID))
                {
                    productQuote.IsSaleModalityFindParam = ExceptionParamType.ParametroVariable;
                    if (woe.IsSaleModalitySearchParam)
                    {
                        if (woe.SaleModalityID == productQuote.SaleModalityID)
                        {
                            productQuote.IsSaleModalityFindParam = ExceptionParamType.ParametroEncontrado;
                        }
                        else
                        {
                            productQuote.IsSaleModalityFindParam = ExceptionParamType.ParametroNoEncontrado;
                            continue;
                        }
                    }

                    productQuote.IsGeographicAreaFindParam = ExceptionParamType.ParametroVariable;
                    if (woe.IsGeographicAreaSearchParam)
                    {
                        if (woe.GeographicAreaID == productQuote.GeographicAreaID)
                        {
                            productQuote.IsGeographicAreaFindParam = ExceptionParamType.ParametroEncontrado;
                        }
                        else
                        {
                            productQuote.IsGeographicAreaFindParam = ExceptionParamType.ParametroNoEncontrado;
                            continue;
                        }
                    }

                    productQuote.IsPaymentDeadlineFindParam = ExceptionParamType.ParametroVariable;
                    if (woe.IsPaymentDeadlineSearchParam)
                    {
                        if (woe.PaymentDeadlineID == productQuote.PaymentDeadlineID)
                        {
                            productQuote.IsPaymentDeadlineFindParam = ExceptionParamType.ParametroEncontrado;
                        }
                        else
                        {
                            productQuote.IsPaymentDeadlineFindParam = ExceptionParamType.ParametroNoEncontrado;
                            continue;
                        }
                    }

                    productQuote.IsQuantityOpenPurchaseOrderFindParam = ExceptionParamType.ParametroVariable;
                    if (woe.IsQuantityOpenPurchaseOrderSearchParam)
                    {
                        if (woe.QuantityOpenPurchaseOrder == productQuote.QuantityOpenPurchaseOrder)
                        {
                            productQuote.IsQuantityOpenPurchaseOrderFindParam = ExceptionParamType.ParametroEncontrado;
                        }
                        else
                        {
                            productQuote.IsQuantityOpenPurchaseOrderFindParam = ExceptionParamType.ParametroNoEncontrado;
                            continue;
                        }
                    }

                    productQuote.IsDeliveryAmountFindParam = ExceptionParamType.ParametroVariable;
                    if (woe.IsDeliveryAmountSearchParam)
                    {
                        if (woe.DeliveryAmount == productQuote.DeliveryAmount)
                        {
                            productQuote.IsDeliveryAmountFindParam = ExceptionParamType.ParametroEncontrado;
                        }
                        else
                        {
                            productQuote.IsDeliveryAmountFindParam = ExceptionParamType.ParametroNoEncontrado;
                            continue;
                        }
                    }

                    productQuote.IsMaximumMonthsStockFindParam = ExceptionParamType.ParametroVariable;
                    if (woe.IsMaximumMonthsStockSearchParam)
                    {
                        if (woe.MaximumMonthsStock == productQuote.MaximumMonthsStock)
                        {
                            productQuote.IsMaximumMonthsStockFindParam = ExceptionParamType.ParametroEncontrado;
                        }
                        else
                        {
                            productQuote.IsMaximumMonthsStockFindParam = ExceptionParamType.ParametroNoEncontrado;
                            continue;
                        }
                    }

                    productQuote.IsMinimumQuantityDeliveryFindParam = ExceptionParamType.ParametroVariable;
                    if (woe.IsMinimumQuantityDeliverySearchParam)
                    {
                        if (woe.MinimumQuantityDelivery == productQuote.MinimumQuantityDelivery)
                        {
                            productQuote.IsMinimumQuantityDeliveryFindParam = ExceptionParamType.ParametroEncontrado;
                        }
                        else
                        {
                            productQuote.IsMinimumQuantityDeliveryFindParam = ExceptionParamType.ParametroNoEncontrado;
                            continue;
                        }
                    }

                    productQuote.IsExchangeTypeFindParam = ExceptionParamType.ParametroVariable;
                    if (woe.IsExchangeTypeSearchParam)
                    {
                        if (woe.ExchangeTypeID == productQuote.ExchangeTypeID)
                        {
                            productQuote.IsExchangeTypeFindParam = ExceptionParamType.ParametroEncontrado;
                        }
                        else
                        {
                            productQuote.IsExchangeTypeFindParam = ExceptionParamType.ParametroNoEncontrado;
                            continue;
                        }
                    }

                    ret = woe;
                    break;
                }
            }

            if (ret == null)
            {
                productQuote.IsSaleModalityFindParam = ExceptionParamType.Indefinido;
                productQuote.IsGeographicAreaFindParam = ExceptionParamType.Indefinido;
                productQuote.IsPaymentDeadlineFindParam = ExceptionParamType.Indefinido;
                productQuote.IsQuantityOpenPurchaseOrderFindParam = ExceptionParamType.Indefinido;
                productQuote.IsDeliveryAmountFindParam = ExceptionParamType.Indefinido;
                productQuote.IsMaximumMonthsStockFindParam = ExceptionParamType.Indefinido;
                productQuote.IsExchangeTypeFindParam = ExceptionParamType.Indefinido;
                productQuote.IsMinimumQuantityDeliveryFindParam = ExceptionParamType.Indefinido;
            }

            return ret;

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && wayOfExceptionRepository != null)
            {
                wayOfExceptionRepository.Dispose();
                wayOfExceptionRepository = null;
            }
        }
    }
}
