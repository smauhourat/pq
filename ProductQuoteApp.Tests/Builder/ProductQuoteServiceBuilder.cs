using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductQuoteApp.Controllers;
using ProductQuoteApp.Persistence;
using ProductQuoteApp.Logging;
using ProductQuoteApp.Services;


namespace ProductQuoteApp.Tests
{
    public class ProductQuoteServiceBuilder : IDisposable
    {
        private readonly ILogger _logger = null;
        private readonly IProductRepository _productRepository = null;
        private readonly ISaleModalityRepository _saleModalityRepository = null;
        private readonly IGeographicAreaRepository _geographicAreaRepository = null;
        private readonly ISaleModalityProductRepository _saleModalityProductRepository = null;
        private readonly IGeographicAreaTransportTypeRepository _geographicAreaTransportTypeRepository = null;
        private readonly ICreditRatingPaymentDeadlineRepository _creditRatingPaymentDeadlineRepository = null;
        private readonly IExchangeTypeRepository _exchangeTypeRepository = null;
        private readonly IStockTimeRepository _stockTimeRepository = null;
        private readonly IDeliveryAmountRepository _deliveryAmountRepository = null;

        private readonly IProductQuoteRepository _productQuoteRepository = null;
        private readonly IEmailManager _emailManager = null;
        private readonly IEmailAccountRepository _emailAccountRepository = null;
        private readonly IAdminUserRepository _adminUserRepository = null;
        private readonly IApplicationUserRepository _applicationUserRepository = null;
        private readonly IWorkflowMessageService _workflowMessageService = null;
        private readonly IPdfService _pdfService = null;
        private readonly ISaleModalityCreditRatingRepository _saleModalityCreditRatingRepository = null;
        private readonly IGlobalVariableRepository _globalVariableRepository = null;
        private readonly ICustomerOrderRepository _customerOrderRepository = null;
        private readonly ICustomerRepository _customerRepository = null;
        private readonly ITransportTypeRepository _transportTypeRepository = null;
        private readonly IPackagingRepository _packagingRepository = null;
        private readonly IRofexRepository _rofexRepository = null;
        private readonly IPaymentDeadlineRepository _paymentDeadlineRepository = null;
        private readonly IShipmentTrackingRepository _shipmentTrackingRepository = null;
        private readonly ICustomerProductRepository _customerProductRepository = null;
        private readonly ILogRecordRepository _logRecordRepository = null;
        private readonly ICustomerOrderService _customerOrderService = null;
        private readonly ITransportServices _transportService = null;
        private readonly IMarginServices _marginServices = null;
        private readonly IWayOfExceptionRepository _wayOfExceptionRepository = null;
        private readonly IWayOfExceptionServices _wayOfExceptionServices = null;
        private readonly ISalesChannelUserRepository _salesChannelUserRepository = null;

        public ProductQuoteServiceBuilder()
        {
            _logger = new Logger();
            _productRepository = new ProductRepository(_logger, new ProductDocumentRepository(_logger));
            _saleModalityRepository = new SaleModalityRepository(_logger);
            _geographicAreaRepository = new GeographicAreaRepository(_logger);
            _saleModalityProductRepository = new SaleModalityProductRepository(_logger);
            _geographicAreaTransportTypeRepository = new GeographicAreaTransportTypeRepository(_logger);
            _creditRatingPaymentDeadlineRepository = new CreditRatingPaymentDeadlineRepository(_logger);
            _exchangeTypeRepository = new ExchangeTypeRepository(_logger);
            _stockTimeRepository = new StockTimeRepository(_logger);
            _deliveryAmountRepository = new DeliveryAmountRepository(_logger);

            _productQuoteRepository = new ProductQuoteRepository(_logger);
            _emailManager = new EmailManager();
            _emailAccountRepository = new EmailAccountRepository(_logger);
            _adminUserRepository = new AdminUserRepository();
            _applicationUserRepository = new ApplicationUserRepository();
            _workflowMessageService = new WorkflowMessageService(_emailManager, _emailAccountRepository, _adminUserRepository, _applicationUserRepository);
            _pdfService = new PdfService();
            _saleModalityCreditRatingRepository = new SaleModalityCreditRatingRepository(_logger);
            _globalVariableRepository = new GlobalVariableRepository(_logger);
            _customerOrderRepository = new CustomerOrderRepository(_logger);
            _customerRepository = new CustomerRepository(_logger);
            _transportTypeRepository = new TransportTypeRepository(_logger);
            _packagingRepository = new PackagingRepository(_logger);
            _rofexRepository = new RofexRepository(_logger);
            _paymentDeadlineRepository = new PaymentDeadlineRepository(_logger);
            _shipmentTrackingRepository = new ShipmentTrackingRepository(_logger);
            _customerProductRepository = new CustomerProductRepository(_logger);
            _logRecordRepository = new LogRecordRepository();
            _customerOrderService = new CustomerOrderService(_customerOrderRepository, _workflowMessageService);
            _transportService = new TransportServices(_geographicAreaTransportTypeRepository);
            _marginServices = new MarginServices();
            _wayOfExceptionRepository = new WayOfExceptionRepository(_logger);
            _wayOfExceptionServices = new WayOfExceptionServices(_wayOfExceptionRepository);
            _salesChannelUserRepository = new SalesChannelUserRepository(_logger);
        }

        public IProductQuoteService Build()
        {
            IProductQuoteService productQuoteService = new ProductQuoteService(_logger,
                _productQuoteRepository,
                _productRepository,
                _workflowMessageService,
                _pdfService,
                _saleModalityProductRepository,
                _saleModalityCreditRatingRepository,
                _globalVariableRepository,
                _customerOrderRepository,
                _customerRepository,
                _transportTypeRepository,
                _geographicAreaTransportTypeRepository,
                _packagingRepository,
                _rofexRepository,
                _paymentDeadlineRepository,
                _shipmentTrackingRepository,
                _customerOrderService,
                _transportService,
                _marginServices,
                _wayOfExceptionServices,
                _salesChannelUserRepository);

            return productQuoteService;

        }

        public IProductRepository ProductRepositoryCreate()
        {
            return _productRepository;
        }

        public ICustomerRepository CustomerRepositoryCreate()
        {
            return _customerRepository;
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
                if (_productQuoteRepository != null)
                {
                    _productQuoteRepository.Dispose();
                }
                if (_productRepository != null)
                {
                    _productRepository.Dispose();
                }
                if (_workflowMessageService != null)
                {
                    _workflowMessageService.Dispose();
                }
                if (_pdfService != null)
                {
                    _pdfService.Dispose();
                }
                if (_saleModalityProductRepository != null)
                {
                    _saleModalityProductRepository.Dispose();
                }
                if (_saleModalityCreditRatingRepository != null)
                {
                    _saleModalityCreditRatingRepository.Dispose();
                }
                if (_globalVariableRepository != null)
                {
                    _globalVariableRepository.Dispose();
                }
                if (_customerOrderRepository != null)
                {
                    _customerOrderRepository.Dispose();
                }
                if (_customerRepository != null)
                {
                    _customerRepository.Dispose();
                }
                if (_transportTypeRepository != null)
                {
                    _transportTypeRepository.Dispose();
                }
                if (_geographicAreaTransportTypeRepository != null)
                {
                    _geographicAreaTransportTypeRepository.Dispose();
                }
                if (_packagingRepository != null)
                {
                    _packagingRepository.Dispose();
                }

                if (_rofexRepository != null)
                {
                    _rofexRepository.Dispose();
                }
                if (_paymentDeadlineRepository != null)
                {
                    _paymentDeadlineRepository.Dispose();
                }
                if (_shipmentTrackingRepository != null)
                {
                    _shipmentTrackingRepository.Dispose();
                }
                if (_customerProductRepository != null)
                {
                    _customerProductRepository.Dispose();
                }
                if (_customerOrderService != null)
                {
                    _customerOrderService.Dispose();
                }
                if (_transportService != null)
                {
                    _transportService.Dispose();
                }
                if (_marginServices != null)
                {
                    _marginServices.Dispose();
                }
                if (_wayOfExceptionRepository != null)
                {
                    _wayOfExceptionRepository.Dispose();
                }
                if (_wayOfExceptionServices != null)
                {
                    _wayOfExceptionServices.Dispose();
                }
            }
        }
    }
}
