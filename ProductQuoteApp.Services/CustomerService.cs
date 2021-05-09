using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductQuoteApp.Persistence;

namespace ProductQuoteApp.Services
{
    public class CustomerService : ICustomerService
    {
        private ICustomerRepository customerRepository = null;
        private ISaleModalityCustomerMarginRepository saleModalityCustomerMarginRepository = null;
        private ICustomerProductRepository customerProductRepository = null;

        public CustomerService(ICustomerRepository customerRepo, ISaleModalityCustomerMarginRepository saleModalityCustomerMarginRepo, ICustomerProductRepository customerProductRepo)
        {
            customerRepository = customerRepo;
            saleModalityCustomerMarginRepository = saleModalityCustomerMarginRepo;
            customerProductRepository = customerProductRepo;
        }

        public async Task CreateAsync(Customer customerToAdd)
        {
            await customerRepository.CreateAsync(customerToAdd);
        }

        private async Task CreateCustomerMargin(int customerID, int saleModalityID, decimal? minimumMarginPercentage, decimal? minimumMarginUSD)
        {
            if ((minimumMarginPercentage == null) && (minimumMarginUSD == null))
                return;

            SaleModalityCustomerMargin saleModalityCustomerMargin = new SaleModalityCustomerMargin
            {
                CustomerID = customerID,
                SaleModalityID = saleModalityID,
                MinimumMarginPercentage = minimumMarginPercentage.HasValue ? (decimal)minimumMarginPercentage : minimumMarginPercentage,
                MinimumMarginUSD = minimumMarginUSD.HasValue ? (decimal)minimumMarginUSD: minimumMarginUSD
            };
            await saleModalityCustomerMarginRepository.CreateAsync(saleModalityCustomerMargin);
        }

        public async Task CreateCompleteAsync(Customer customerToAdd, decimal? minimumMarginPercentage_DL, decimal? minimumMarginPercentage_DLP, decimal? minimumMarginPercentage_ISL, decimal? minimumMarginPercentage_IND, decimal? minimumMarginUSD_DL, decimal? minimumMarginUSD_DLP, decimal? minimumMarginUSD_ISL, decimal? minimumMarginUSD_IND)
        {
            //Creamos el Cliente
            await customerRepository.CreateAsync(customerToAdd);

            //Creamos los margenes asociados por Modalidades
            saleModalityCustomerMarginRepository.DeleteByCustomer(customerToAdd.CustomerID);

            await CreateCustomerMargin(customerToAdd.CustomerID, (int)EnumSaleModality.Local, minimumMarginPercentage_DL, minimumMarginUSD_DL);
            await CreateCustomerMargin(customerToAdd.CustomerID, (int)EnumSaleModality.LocalProgramada, minimumMarginPercentage_DLP, minimumMarginUSD_DLP);
            await CreateCustomerMargin(customerToAdd.CustomerID, (int)EnumSaleModality.IndentSL, minimumMarginPercentage_ISL, minimumMarginUSD_ISL);
            await CreateCustomerMargin(customerToAdd.CustomerID, (int)EnumSaleModality.Indent, minimumMarginPercentage_IND, minimumMarginUSD_IND);

            //if (customerToAdd.IsSpot) //Para TODOS
                customerProductRepository.AddAllProductsToCustomer(customerToAdd.CustomerID, true);
        }

        public async Task DeleteAsync(int customerID)
        {
            await customerRepository.DeleteAsync(customerID);
        }

        public async Task DeleteCascadeAsync(int customerID)
        {
            await customerRepository.DeleteCascadeAsync(customerID);
        }

        public IEnumerable<Customer> FindCustomers()
        {
            return customerRepository.FindCustomers();
        }

        public async Task<IEnumerable<Customer>> FindCustomersAsync()
        {
            return await customerRepository.FindCustomersAsync();
        }

        public Customer FindCustomersByID(int customerID)
        {
            return customerRepository.FindCustomersByID(customerID);
        }

        public async Task<Customer> FindCustomersByIDAsync(int customerID)
        {
            return await customerRepository.FindCustomersByIDAsync(customerID);
        }

        public async Task UpdateAsync(Customer customerToSave)
        {
            await customerRepository.UpdateAsync(customerToSave);
        }

        public async Task UpdateCompleteAsync(Customer customerToSave, decimal? minimumMarginPercentage_DL, decimal? minimumMarginPercentage_DLP, decimal? minimumMarginPercentage_ISL, decimal? minimumMarginPercentage_IND, decimal? minimumMarginUSD_DL, decimal? minimumMarginUSD_DLP, decimal? minimumMarginUSD_ISL, decimal? minimumMarginUSD_IND)
        {
            //Eliminamos los Margenes por Modalidad
            saleModalityCustomerMarginRepository.DeleteByCustomer(customerToSave.CustomerID);

            //Actualizamos el Cliente
            await customerRepository.UpdateAsync(customerToSave);

            //Actualizamos los Margenes por Modalidad
            await CreateCustomerMargin(customerToSave.CustomerID, (int)EnumSaleModality.Local, minimumMarginPercentage_DL, minimumMarginUSD_DL);
            await CreateCustomerMargin(customerToSave.CustomerID, (int)EnumSaleModality.LocalProgramada, minimumMarginPercentage_DLP, minimumMarginUSD_DLP);
            await CreateCustomerMargin(customerToSave.CustomerID, (int)EnumSaleModality.IndentSL, minimumMarginPercentage_ISL, minimumMarginUSD_ISL);
            await CreateCustomerMargin(customerToSave.CustomerID, (int)EnumSaleModality.Indent, minimumMarginPercentage_IND, minimumMarginUSD_IND);

            //if (customerToSave.IsSpot) //Para TODOS
                customerProductRepository.AddAllProductsToCustomer(customerToSave.CustomerID, true);

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free managed resources
                if (customerRepository != null)
                {
                    customerRepository.Dispose();
                    customerRepository = null;
                }                
                if (saleModalityCustomerMarginRepository != null)
                {
                    saleModalityCustomerMarginRepository.Dispose();
                    saleModalityCustomerMarginRepository = null;
                }
                if (customerProductRepository != null)
                {
                    customerProductRepository.Dispose();
                    customerProductRepository = null;
                }
            }
        }
    }
}
