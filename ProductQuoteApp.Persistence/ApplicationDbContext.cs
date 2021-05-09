using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ProductQuoteApp.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Packaging> Packagings { get; set; }
        public DbSet<GeographicArea> GeographicAreas { get; set; }
        public DbSet<TransportType> TransportTypes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CurrencyType> CurrencyTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CreditRating> CreditRatings { get; set; }
        public DbSet<ProductQuote> ProductQuotes { get; set; }
        public DbSet<PaymentDeadline> PaymentDeadlines { get; set; }
        public DbSet<EmailAccount> EmailAccounts { get; set; }
        public DbSet<CustomerOrderStatus> CustomerOrderStatus { get; set; }
        public DbSet<CustomerOrder> CustomerOrders { get; set; }
        public DbSet<ProductDocument> ProductDocuments { get; set; }
        public DbSet<GeographicAreaTransportType> GeographicAreaTransportTypes { get; set; }
        public DbSet<SaleModality> SaleModalitys { get; set; }
        public DbSet<SaleModalityProduct> SaleModalityProducts { get; set; }

        public DbSet<SaleModalityCreditRating> SaleModalityCreditRatings { get; set; }

        public DbSet<CreditRatingPaymentDeadline> CreditRatingPaymentDeadlines { get; set; }
        public DbSet<GlobalVariable> GlobalVariables { get; set; }

        public DbSet<Rofex> Rofexs { get; set; }
        public DbSet<ExchangeType> ExchangeTypes { get; set; }
        public DbSet<FreightType> FreightTypes { get; set; }
        public DbSet<StockTime> StockTimes { get; set; }
        public DbSet<DeliveryAmount> DeliveryAmounts { get; set; }
        public DbSet<SaleModalityGeographicArea> SaleModalityGeographicAreas { get; set; }
        public DbSet<SaleModalityDeliveryAmount> SaleModalityDeliveryAmounts { get; set; }
        public DbSet<SaleModalityStockTime> SaleModalityStockTimes { get; set; }
        public DbSet<SaleModalityExchangeType> SaleModalityExchangeTypes { get; set; }
        public DbSet<ShipmentTracking> ShipmentTrackings { get; set; }
        public DbSet<TestModel> TestModels { get; set; }
        public DbSet<LogRecord> LogRecords { get; set; }
        public DbSet<CustomerProduct> CustomerProducts { get; set; }
        public DbSet<SellerCompany> SellerCompanys { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactType> ContactTypes { get; set; }

        public DbSet<SaleModalityProductMargin> SaleModalityProductMargins { get; set; }
        public DbSet<SaleModalityCustomerMargin> SaleModalityCustomerMargins { get; set; }

        public DbSet<WayOfException> WayOfExceptions { get; set; }

        public DbSet<DueDateReason> DueDateReasons { get; set; }

        public DbSet<IIBBTreatment> IIBBTreatments { get; set; }

        public DbSet<ReasonsForClosure> ReasonsForClosures { get; set; }

        public DbSet<SalesChannel> SalesChannels { get; set; }
        public DbSet<SalesChannelUser> SalesChannelUsers { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            //Para que no tire error por no existir las tablas __MigrationHistory y EdmMetadata 
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<CustomerOrder>()
                .HasKey(c => c.ProductQuoteID);

            modelBuilder.Entity<ProductQuote>()
                .HasRequired(m => m.CustomerOrder)
                .WithRequiredPrincipal(p => p.ProductQuote);

            modelBuilder.Entity<ShipmentTracking>()
                .HasKey(c => c.ProductQuoteID);

            modelBuilder.Entity<ProductQuote>()
                        .HasOptional(s => s.ShipmentTracking)
                        .WithRequired(ad => ad.ProductQuote);


            //modelBuilder.Entity<ProductQuote>().HasOptional(m => m.CustomerOrder).WithRequired(c => c.ProductQuote);
            //modelBuilder.Entity<CustomerOrder>().HasKey(e => e.CustomerOrderID);



            base.OnModelCreating(modelBuilder);

            //https://www.itworld.com/article/2909612/working-with-decimal-precision-in-net-with-mssql-server-and-entity-framework.html
            // Precision attribute for decimals
            Precision.ConfigureModelBuilder(modelBuilder);
        }

    }
}
