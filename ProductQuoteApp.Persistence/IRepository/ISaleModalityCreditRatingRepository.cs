using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface ISaleModalityCreditRatingRepository: IDisposable
    {
        SaleModalityCreditRating FindSaleModalityCreditRatingByID(int saleModalityID, int customerID);
        Task<List<SaleModalityCreditRating>> FindSaleModalityCreditRatingBySaleModalityAndCustomerAsync(int saleModalityID, int customerID);

        Task<List<SaleModalityCreditRating>> FindSaleModalityCreditRatingsBySaleModalityAsync(int saleModalityID);

        List<CreditRating> FindCreditRatingAvailableBySaleModality(int saleModalityID);

        void Create(SaleModalityCreditRating saleModalityCreditRatingToAdd);
        Task CreateAsync(SaleModalityCreditRating saleModalityCreditRatingToAdd);
        void Delete(int saleModalityCreditRatingID);
        Task DeleteAsync(int saleModalityCreditRatingID);
        void Update(SaleModalityCreditRating saleModalityCreditRatingToSave);
        Task UpdateAsync(SaleModalityCreditRating saleModalityCreditRatingToSave);
    }
}
