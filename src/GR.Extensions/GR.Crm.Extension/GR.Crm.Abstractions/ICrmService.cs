using System.Collections.Generic;
using System.Threading.Tasks;
using GR.Core.Helpers;
using GR.Crm.Abstractions.Models;

namespace GR.Crm.Abstractions
{
    public interface ICrmService
    {
        /// <summary>
        /// Get all currencies
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<Currency>>> GetAllCurrenciesAsync();
    }
}