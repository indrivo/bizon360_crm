using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GR.Core.BaseControllers;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.ViewModels.ProductsViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GR.Crm.Leads.Razor.Controllers
{
    public class ProductController : BaseGearController
    {

        #region Injectable

        /// <summary>
        /// Inject service
        /// </summary>
        private readonly IProductService _productService;

        #endregion

        public IActionResult Index()
        {
            return View();
        }

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<GetProductViewModel>>))]
        public async Task<JsonResult> GetAllProducts(bool includeDeleted = false) 
            => await JsonAsync(_productService.GetAllProductsAsync(includeDeleted));

        /// <summary>
        /// Get all paginated products
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<PagedResult<GetProductViewModel>>))]
        public async Task<JsonResult> GetAllPaginatedProduct(PageRequest request)
            => await JsonAsync(_productService.GetAllPaginatedProductsAsync(request));



        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<GetProductViewModel>))]
        public async Task<JsonResult> GetProductById([Required] Guid productId)
            => await JsonAsync(_productService.GetProductByIdAsync(productId));


        /// <summary>
        /// Add product
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> AddProduct([Required] AddProductViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_productService.AddProductAsync(model));
        }


        /// <summary>
        /// Update product
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdateProduct([Required] AddProductViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_productService.UpdateProductAsync(model));
        }



        /// <summary>
        /// Disable product
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> DisableProduct([Required] Guid productId)
            => await JsonAsync(_productService.DisableProductAsync(productId));

        /// <summary>
        /// Activate product
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> ActivateProduct([Required] Guid productId)
            => await JsonAsync(_productService.ActivateProductAsync(productId));

        /// <summary>
        /// Activate product
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> DeleteProduct([Required] Guid productId)
            => await JsonAsync(_productService.DeleteProductAsync(productId));
    }
}
