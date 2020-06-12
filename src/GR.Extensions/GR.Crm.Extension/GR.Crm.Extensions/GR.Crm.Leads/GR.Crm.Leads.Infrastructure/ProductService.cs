using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Leads.Abstractions.ViewModels.ProductsViewModels;
using Mapster;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;

namespace GR.Crm.Leads.Infrastructure
{
    public class ProductService: IProductService
    {

        #region Injectable

        /// <summary>
        /// Inject context
        /// </summary>
        private readonly ILeadContext<Lead> _context;

        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;

        #endregion


        public ProductService(ILeadContext<Lead> context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Add product 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddProductAsync(AddProductViewModel model)
        {
            if(model == null)
                return new InvalidParametersResultModel<Guid>();

            var product = _mapper.Map<Product>(model);

            _context.Products.Add(product);
            var result = await _context.PushAsync();

            return new ResultModel<Guid>{IsSuccess = result.IsSuccess, Errors = result.Errors, Result = product.Id};
        }

        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateProductAsync(AddProductViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();


            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.Id.Equals(model.Id));

            if(product == null)
                return new NotFoundResultModel();

            product.Name = model.Name;
            product.Commission = model.Commission;
            product.Price = model.Price;
            product.Sku = model.Sku;
            product.Vat = model.Vat;
            product.BankAccount = model.BankAccount;

            _context.Products.Update(product);
            return await _context.PushAsync();
        }

        /// <summary>
        /// Get product by id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetProductViewModel>> GetProductByIdAsync(Guid? productId)
        {
            if(productId == null)
                return new InvalidParametersResultModel<GetProductViewModel>();

            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.Id.Equals(productId));

            if(product == null)
                return new NotFoundResultModel<GetProductViewModel>();

            return new SuccessResultModel<GetProductViewModel>(_mapper.Map<GetProductViewModel>(product));

        }

        /// <summary>
        /// Get product by bank account
        /// </summary>
        /// <param name="bankAccount"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetProductViewModel>> GetProductByBankAccountAsync(string bankAccount)
        {
            if (string.IsNullOrEmpty(bankAccount))
                return new InvalidParametersResultModel<GetProductViewModel>();

            var product = await _context.Products
                .FirstOrDefaultAsync(x => string.Equals(x.BankAccount.Trim(), bankAccount.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if (product == null)
                return new NotFoundResultModel<GetProductViewModel>();

            return new SuccessResultModel<GetProductViewModel>(_mapper.Map<GetProductViewModel>(product));

        }

        /// <summary>
        /// Get all product
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetProductViewModel>>> GetAllProductsAsync(
            bool includeDeleted = false)
        {
            var products = await _context.Products
                .Where(x => !x.IsDeleted || includeDeleted)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<GetProductViewModel>>(_mapper.Map<IEnumerable<GetProductViewModel>>(products));

        }

        /// <summary>
        /// Get all paginated product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<GetProductViewModel>>> GetAllPaginatedProductsAsync(PageRequest request)
        {
            var pagedResult = await _context.Products
                .Where(x => !x.IsDeleted || request.IncludeDeleted)
                .GetPagedAsync(request);

            var map = pagedResult.Map(_mapper.Map<IEnumerable<GetProductViewModel>>(pagedResult.Result));

            return new SuccessResultModel<PagedResult<GetProductViewModel>>(map);

        }


        /// <summary>
        /// Disable product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisableProductAsync(Guid? productId) =>
            await _context.DisableRecordAsync<Product>(productId);

        /// <summary>
        /// Activate product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivateProductAsync(Guid? productId) =>
            await _context.ActivateRecordAsync<Product>(productId);

        /// <summary>
        /// Delete product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteProductAsync(Guid? productId) =>
            await _context.RemovePermanentRecordAsync<Product>(productId);

    }
}
