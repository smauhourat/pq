using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface IDueDateReasonRepository: IDisposable 
    {
        List<DueDateReason> DueDateReasons();
        IEnumerable<DueDateReason> FindDueDateReasonsFilter(string filter, string sortBy);
        Task<List<DueDateReason>> FindDueDateReasonsAsync();
        Task<DueDateReason> FindDueDateReasonsByIDAsync(int dueDateReasonID);
        Task CreateAsync(DueDateReason dueDateReasonToAdd);
        Task DeleteAsync(int dueDateReasonID);
        Task UpdateAsync(DueDateReason dueDateReasonToSave);
    }
}
