using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface ICreditRatingRepository : IDisposable
    {
        List<CreditRating> CreditRatings();
        Task<List<CreditRating>> CreditRatingsAsync();

        Task<List<CreditRating>> FindCreditRatingsAsync();
        Task<CreditRating> FindCreditRatingByIDAsync(int creditRatingID);
        Task CreateAsync(CreditRating creditRatingToAdd);
        Task DeleteAsync(int creditRatingID);
        Task UpdateAsync(CreditRating creditRatingToSave);
    }
}
