using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using ProductQuoteApp.Persistence;
using ProductQuoteApp.Logging;
using ProductQuoteApp.Services;
using Autofac.Core;
using System.Reflection;
using System.Web.Http;
using Autofac.Integration.WebApi;

namespace ProductQuoteApp.App_Start
{
    public static class DependenciesConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            
            //NUEVO - WEB API
            builder.RegisterApiControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<LogRecordRepository>().As<ILogRecordRepository>();
            builder.RegisterType<DefaultLogger>().As<ILogger>().SingleInstance();
            //builder.RegisterType<Logger>().As<ILogger>().SingleInstance();

            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>));

            builder.RegisterType<ProviderRepository>().As<IProviderRepository>();
            builder.RegisterType<PackagingRepository>().As<IPackagingRepository>();
            builder.RegisterType<GeographicAreaRepository>().As<IGeographicAreaRepository>();
            builder.RegisterType<TransportTypeRepository>().As<ITransportTypeRepository>();
            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>();
            builder.RegisterType<CurrencyTypeRepository>().As<ICurrencyTypeRepository>();
            builder.RegisterType<ProductRepository>().As<IProductRepository>();
            builder.RegisterType<CreditRatingRepository>().As<ICreditRatingRepository>();
            builder.RegisterType<ProductQuoteRepository>().As<IProductQuoteRepository>();            
            builder.RegisterType<PaymentDeadlineRepository>().As<IPaymentDeadlineRepository>();
            builder.RegisterType<EmailAccountRepository>().As<IEmailAccountRepository>();

            builder.RegisterType<PdfService>().As<IPdfService>();
            //builder.RegisterType<ProductQuoteService>().As<IProductQuoteService>();
            builder.RegisterType<ProductQuoteService>().As<IProductQuoteService>();

            builder.RegisterType<EmailManager>().As<IEmailManager>();
            builder.RegisterType<WorkflowMessageService>().As<IWorkflowMessageService>();
            //builder.RegisterType<WorkflowMessageService>().As<IWorkflowMessageService>().WithParameter(ResolvedParameter.ForNamed<IEmailManager>("")).InstancePerLifetimeScope();
            builder.RegisterType<CustomerOrderRepository>().As<ICustomerOrderRepository>();
            builder.RegisterType<AdminUserRepository>().As<IAdminUserRepository>();
            builder.RegisterType<ApplicationUserRepository>().As<IApplicationUserRepository>();

            builder.RegisterType<ProductDocumentRepository>().As<IProductDocumentRepository>();
            builder.RegisterType<DocumentFileService>().As<IDocumentFileService>();
            builder.RegisterType<ProductDocumentService>().As<IProductDocumentService>();

            builder.RegisterType<GeographicAreaTransportTypeRepository>().As<IGeographicAreaTransportTypeRepository>();

            builder.RegisterType<SaleModalityRepository>().As<ISaleModalityRepository>();
            builder.RegisterType<SaleModalityProductRepository>().As<ISaleModalityProductRepository>();
            //builder.RegisterType<ProductSaleModalityRepository>().As<IProductSaleModalityRepository>();

            builder.RegisterType<SaleModalityCreditRatingRepository>().As<ISaleModalityCreditRatingRepository>();
            builder.RegisterType<CreditRatingPaymentDeadlineRepository>().As<ICreditRatingPaymentDeadlineRepository>();
            builder.RegisterType<GlobalVariableRepository>().As<IGlobalVariableRepository>();

            builder.RegisterType<ProductService>().As<IProductService>();
            builder.RegisterType<RofexRepository>().As<IRofexRepository>();
            builder.RegisterType<ExchangeTypeRepository>().As<IExchangeTypeRepository>();

            builder.RegisterType<FreightTypeRepository>().As<IFreightTypeRepository>();

            builder.RegisterType<StockTimeRepository>().As<IStockTimeRepository>();
            builder.RegisterType<DeliveryAmountRepository>().As<IDeliveryAmountRepository>();
            builder.RegisterType<SaleModalityCreditRatingRepository>().As<ISaleModalityCreditRatingRepository>();
            builder.RegisterType<SaleModalityDeliveryAmountRepository>().As<ISaleModalityDeliveryAmountRepository>();
            builder.RegisterType<ShipmentTrackingRepository>().As<IShipmentTrackingRepository>();

            builder.RegisterType<TestModelRepository>().As<ITestModelRepository>();
            builder.RegisterType<CustomerProductRepository>().As<ICustomerProductRepository>();

            builder.RegisterType<CustomerOrderService>().As<ICustomerOrderService>();

            builder.RegisterType<SellerCompanyRepository>().As<ISellerCompanyRepository>();

            builder.RegisterType<SaleModalityProductMarginRepository>().As<ISaleModalityProductMarginRepository>();
            builder.RegisterType<SaleModalityCustomerMarginRepository>().As<ISaleModalityCustomerMarginRepository>();

            builder.RegisterType<CustomerService>().As<ICustomerService>();
            builder.RegisterType<TransportServices>().As<ITransportServices>();
            builder.RegisterType<MarginServices>().As<IMarginServices>();

            builder.RegisterType<WayOfExceptionRepository>().As<IWayOfExceptionRepository>();
            builder.RegisterType<WayOfExceptionServices>().As<IWayOfExceptionServices>();

            builder.RegisterType<DueDateReasonRepository>().As<IDueDateReasonRepository>();

            builder.RegisterType<RofexService>().As<IRofexService>();

            builder.RegisterType<ContactRepository>().As<IContactRepository>();
            builder.RegisterType<ContactTypeRepository>().As<IContactTypeRepository>();

            builder.RegisterType<SalesChannelRepository>().As<ISalesChannelRepository>();
            builder.RegisterType<SalesChannelUserRepository>().As<ISalesChannelUserRepository>();

            var container = builder.Build();

            //NUEVO - WEB API
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            //Eliminacion de referencias circulares (cuando se devuelven objetos que tienen otros adentro)
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}