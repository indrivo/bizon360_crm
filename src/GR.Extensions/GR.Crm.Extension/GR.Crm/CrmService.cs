using System.Collections.Generic;
using System.Threading.Tasks;
using GR.Core.Helpers;
using GR.Core.Helpers.Responses;
using GR.Crm.Abstractions;
using GR.Crm.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace GR.Crm
{
    public class CrmService : ICrmService
    {
        #region Injectable

        /// <summary>
        /// Inject context
        /// </summary>
        private readonly ICrmContext _context;

        #endregion

        public CrmService(ICrmContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all currencies
        /// </summary>
        /// <returns></returns>
        public async Task<ResultModel<IEnumerable<Currency>>> GetAllCurrenciesAsync()
        {
            var data = await _context.Currencies
                .AsNoTracking()
                .ToListAsync();
            return new SuccessResultModel<IEnumerable<Currency>>(data);
        }
    }
}
