using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductQuoteApp.Persistence;

namespace ProductQuoteApp.Services
{
    public class ProductService : IProductService
    {
        private IProductRepository productRepository = null;
        private ISaleModalityProductRepository saleModalityProductRepository = null;
        private IProductDocumentService productDocumentService = null;
        private ICustomerProductRepository customerProductRepository = null;
        private ISaleModalityProductMarginRepository saleModalityProductMarginRepository = null;

        public ProductService(
            IProductRepository productRepo, 
            ISaleModalityProductRepository saleModalityProductRepo, 
            IProductDocumentService productDocumentServ, 
            ICustomerProductRepository customerProductRepo, 
            ISaleModalityProductMarginRepository saleModalityProductMarginRepo)
        {
            productRepository = productRepo;
            saleModalityProductRepository = saleModalityProductRepo;
            productDocumentService = productDocumentServ;
            customerProductRepository = customerProductRepo;
            saleModalityProductMarginRepository = saleModalityProductMarginRepo;
        }

        public async Task CreateAsync(Product productToAdd)
        {
            await productRepository.CreateAsync(productToAdd);
        }

        private async Task CreateProductCost(int productID, int saleModalityID, decimal? productCost)
        {
            if (productCost == null)
                return;

            SaleModalityProduct saleModalityProduct = new SaleModalityProduct
            {
                ProductID = productID,
                SaleModalityID = saleModalityID,
                ProductCost = (decimal)productCost
            };
            await saleModalityProductRepository.CreateAsync(saleModalityProduct);
        }

        //
        private async Task CreateProductMargin(int productID, int saleModalityID, decimal? minimumMarginPercentage, decimal? minimumMarginUSD)
        {
            if ((minimumMarginPercentage == null) && (minimumMarginUSD == null))
                return;

            SaleModalityProductMargin saleModalityProductMargin = new SaleModalityProductMargin
            {
                ProductID = productID,
                SaleModalityID = saleModalityID,
                MinimumMarginPercentage = minimumMarginPercentage.HasValue ? (decimal)minimumMarginPercentage : minimumMarginPercentage,
                MinimumMarginUSD = minimumMarginUSD.HasValue ? (decimal)minimumMarginUSD : minimumMarginUSD
            };
            await saleModalityProductMarginRepository.CreateAsync(saleModalityProductMargin);
        }

        public async Task CreateWithCostAsync(Product productToAdd, decimal? productCost_DL, decimal? productCost_DLP, decimal? productCost_ISL, decimal? productCost_IND, Boolean addToAllCustomer)
        {
            await productRepository.CreateAsync(productToAdd);

            saleModalityProductRepository.DeleteByProduct(productToAdd.ProductID);

            await CreateProductCost(productToAdd.ProductID, 1, productCost_DL);
            await CreateProductCost(productToAdd.ProductID, 2, productCost_DLP);
            await CreateProductCost(productToAdd.ProductID, 3, productCost_ISL);
            await CreateProductCost(productToAdd.ProductID, 4, productCost_IND);

            if (addToAllCustomer)
                customerProductRepository.AddAllCustomersToProduct(productToAdd.ProductID);
        }

        public async Task CreateCompleteAsync(Product productToAdd, decimal? productCost_DL, decimal? productCost_DLP, decimal? productCost_ISL, decimal? productCost_IND, Boolean addToAllCustomer, decimal? minimumMarginPercentage_DL, decimal? minimumMarginPercentage_DLP, decimal? minimumMarginPercentage_ISL, decimal? minimumMarginPercentage_IND, decimal? minimumMarginUSD_DL, decimal? minimumMarginUSD_DLP, decimal? minimumMarginUSD_ISL, decimal? minimumMarginUSD_IND)
        {
            //Creamos el Producto
            await productRepository.CreateAsync(productToAdd);

            //Creamos los precios asociados por Modalidades
            saleModalityProductRepository.DeleteByProduct(productToAdd.ProductID);

            await CreateProductCost(productToAdd.ProductID, 1, productCost_DL);
            await CreateProductCost(productToAdd.ProductID, 2, productCost_DLP);
            await CreateProductCost(productToAdd.ProductID, 3, productCost_ISL);
            await CreateProductCost(productToAdd.ProductID, 4, productCost_IND);

            //Para TODOS los Clientes
            customerProductRepository.AddAllCustomersToProduct(productToAdd.ProductID);

            //Si se aplica el Producto a todos los Clientes, lo agregamos
            //if (addToAllCustomer)
            //    customerProductRepository.AddAllCustomersToProduct(productToAdd.ProductID);
            //else
            //    customerProductRepository.AddAllSpotCustomerToProduct(productToAdd.ProductID);

            //Creamos los margenes asociados por Modalidades
            saleModalityProductMarginRepository.DeleteByProduct(productToAdd.ProductID);

            await CreateProductMargin(productToAdd.ProductID, (int)EnumSaleModality.Local, minimumMarginPercentage_DL, minimumMarginUSD_DL);
            await CreateProductMargin(productToAdd.ProductID, (int)EnumSaleModality.LocalProgramada, minimumMarginPercentage_DLP, minimumMarginUSD_DLP);
            await CreateProductMargin(productToAdd.ProductID, (int)EnumSaleModality.IndentSL, minimumMarginPercentage_ISL, minimumMarginUSD_ISL);
            await CreateProductMargin(productToAdd.ProductID, (int)EnumSaleModality.Indent, minimumMarginPercentage_IND, minimumMarginUSD_IND);
        }

        public async Task CreateCopyAsync(int productID)
        {
            var newProduct = await productRepository.CreateCopyAsync(productID);
            var product = await productRepository.FindProductsByIDAsync(productID);

            //Costos
            foreach (var item in product.SaleModalityProducts)
            {
                await CreateProductCost(newProduct.ProductID, item.SaleModalityID, item.ProductCost);
            }

            //Margenes
            foreach (var item in product.SaleModalityProductMargins)
            {
                await CreateProductMargin(newProduct.ProductID, item.SaleModalityID, item.MinimumMarginPercentage, item.MinimumMarginUSD);
            }

            //Para TODOS los Clientes
            customerProductRepository.AddAllCustomersToProduct(newProduct.ProductID);
        }

        public async Task UpdateAsync(Product productToSave)
        {
            await productRepository.UpdateAsync(productToSave);
        }

        public async Task UpdateWithCostAsync(Product productToSave, decimal? productCost_DL, decimal? productCost_DLP, decimal? productCost_ISL, decimal? productCost_IND)
        {
            //Eliminamos los Precios por Modalidad
            saleModalityProductRepository.DeleteByProduct(productToSave.ProductID);

            //Actualizamos el Producto
            await productRepository.UpdateAsync(productToSave);

            //Actualizamos los Precios por Modalidad
            await CreateProductCost(productToSave.ProductID, 1, productCost_DL);
            await CreateProductCost(productToSave.ProductID, 2, productCost_DLP);
            await CreateProductCost(productToSave.ProductID, 3, productCost_ISL);
            await CreateProductCost(productToSave.ProductID, 4, productCost_IND);
        }

        public async Task UpdateCompleteAsync(Product productToSave, decimal? productCost_DL, decimal? productCost_DLP, decimal? productCost_ISL, decimal? productCost_IND, decimal? minimumMarginPercentage_DL, decimal? minimumMarginPercentage_DLP, decimal? minimumMarginPercentage_ISL, decimal? minimumMarginPercentage_IND, decimal? minimumMarginUSD_DL, decimal? minimumMarginUSD_DLP, decimal? minimumMarginUSD_ISL, decimal? minimumMarginUSD_IND)
        {
            //Eliminamos los Precios por Modalidad
            saleModalityProductRepository.DeleteByProduct(productToSave.ProductID);

            //Eliminamos los Margenes por Modalidad
            saleModalityProductMarginRepository.DeleteByProduct(productToSave.ProductID);

            //Actualizamos el Producto
            await productRepository.UpdateAsync(productToSave);

            //Actualizamos los Precios por Modalidad
            await CreateProductCost(productToSave.ProductID, 1, productCost_DL);
            await CreateProductCost(productToSave.ProductID, 2, productCost_DLP);
            await CreateProductCost(productToSave.ProductID, 3, productCost_ISL);
            await CreateProductCost(productToSave.ProductID, 4, productCost_IND);
            
            //Actualizamos los Margenes por Modalidad
            await CreateProductMargin(productToSave.ProductID, (int)EnumSaleModality.Local, minimumMarginPercentage_DL, minimumMarginUSD_DL);
            await CreateProductMargin(productToSave.ProductID, (int)EnumSaleModality.LocalProgramada, minimumMarginPercentage_DLP, minimumMarginUSD_DLP);
            await CreateProductMargin(productToSave.ProductID, (int)EnumSaleModality.IndentSL, minimumMarginPercentage_ISL, minimumMarginUSD_ISL);
            await CreateProductMargin(productToSave.ProductID, (int)EnumSaleModality.Indent, minimumMarginPercentage_IND, minimumMarginUSD_IND);
        }

        public async Task DeleteAsync(int productID)
        {
            //Eliminamos los pdf relacionados al producto
            productDocumentService.DeleteByProductIDAsync(productID);

            //Eliminamos la relacion de precios con las Modalidades
            saleModalityProductRepository.DeleteByProduct(productID);

            //Eliminamos la relacion de margenes con las Modalidades
            saleModalityProductMarginRepository.DeleteByProduct(productID);

            //Eliminamos el Producto
            await productRepository.DeleteAsync(productID);
        }

        public async Task<IEnumerable<Product>> FindProductsAsync()
        {
            return await productRepository.FindProductsAsync();
        }

        public IEnumerable<Product> FindProducts()
        {
            return productRepository.FindProducts();
        }

        public Product FindProductsByID(int productID)
        {
            return productRepository.FindProductsByID(productID);
        }

        public async Task<Product> FindProductsByIDAsync(int productID)
        {
            return await productRepository.FindProductsByIDAsync(productID);
        }

        public List<Product> Products()
        {
            return productRepository.Products();
        }

        public List<Product> ProductsActive()
        {
            return productRepository.ProductsActive();
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
                if (productRepository != null)
                {
                    productRepository.Dispose();
                    productRepository = null;
                }
                if (saleModalityProductRepository != null)
                {
                    saleModalityProductRepository.Dispose();
                    saleModalityProductRepository = null;
                }
                if (productDocumentService != null)
                {
                    productDocumentService.Dispose();
                    productDocumentService = null;
                }
                if (customerProductRepository != null)
                {
                    customerProductRepository.Dispose();
                    customerProductRepository = null;
                }
                if (saleModalityProductMarginRepository != null)
                {
                    saleModalityProductMarginRepository.Dispose();
                    saleModalityProductMarginRepository = null;
                }
            }
        }
    }
}
