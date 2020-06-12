using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Leads.Abstractions.ViewModels.ProductsViewModels;

namespace GR.Crm.Leads.Abstractions
{
    public interface IProductService
    {

        /// <summary>
        /// Add product 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddProductAsync(AddProductViewModel model);

        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateProductAsync(AddProductViewModel model);

        /// <summary>
        /// Get product by id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<ResultModel<GetProductViewModel>> GetProductByIdAsync(Guid? productId);

        /// <summary>
        /// Get product by bank account
        /// </summary>
        /// <param name="bankAccount"></param>
        /// <returns></returns>
        Task<ResultModel<GetProductViewModel>> GetProductByBankAccountAsync(string bankAccount);

        /// <summary>
        /// Get all product
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetProductViewModel>>> GetAllProductsAsync(bool includeDeleted);


        /// <summary>
        /// Get all paginated product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetProductViewModel>>> GetAllPaginatedProductsAsync(PageRequest request);

        /// <summary>
        /// Disable product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<ResultModel> DisableProductAsync(Guid? productId);

        /// <summary>
        /// Activate product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateProductAsync(Guid? productId);

        /// <summary>
        /// Delete product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteProductAsync(Guid? productId);
    }
}
