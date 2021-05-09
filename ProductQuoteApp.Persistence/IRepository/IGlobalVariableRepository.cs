using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface IGlobalVariableRepository: IDisposable 
    {
        Task<GlobalVariable> FindGlobalVariablesAsync();
        GlobalVariable FindGlobalVariables();
        Task UpdateAsync(GlobalVariable globalVariableToSave);
        void Update(GlobalVariable globalVariableToSave);
    }
}
