using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ProductQuoteApp.Persistence;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Services
{
    public class CustomerOrderService : ICustomerOrderService
    {
        private ICustomerOrderRepository customerOrderRepository = null;
        private IWorkflowMessageService workflowMessageService = null;

        public CustomerOrderService(ICustomerOrderRepository customerOrderRepo, IWorkflowMessageService workflowMessageServ) //, IPdfService pdfServ)
        {
            customerOrderRepository = customerOrderRepo;
            workflowMessageService = workflowMessageServ;
        }

        public async Task CreateAsync(CustomerOrder customerOrderToAdd)
        {
            await customerOrderRepository.CreateAsync(customerOrderToAdd);
        }

        public async Task DeleteAsync(int customerOrderID)
        {
            CustomerOrder customerOrder = await customerOrderRepository.FindCustomerOrdersByIDAsync(customerOrderID);

            if (customerOrder == null)
                return;

            await customerOrderRepository.DeleteAsync(customerOrderID);
        }

        public void Approve(int customerOrderID)
        {
            customerOrderRepository.Approve(customerOrderID);
        }

        public void Reject(int customerOrderID)
        {
            customerOrderRepository.Reject(customerOrderID);
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
                if (customerOrderRepository != null)
                {
                    customerOrderRepository.Dispose();
                    customerOrderRepository = null;
                }
                if (workflowMessageService != null)
                {
                    workflowMessageService.Dispose();
                    workflowMessageService = null;
                }
            }
        }
    }
}
