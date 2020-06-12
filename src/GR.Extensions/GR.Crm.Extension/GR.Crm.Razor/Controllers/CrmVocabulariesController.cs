using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GR.Core.BaseControllers;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Abstractions;
using GR.Crm.Abstractions.ViewModels.JobPositionViewModels;
using GR.Crm.Abstractions.ViewModels.ProductTypeViewModels;
using GR.Crm.Abstractions.ViewModels.ServiceTypeViewModels;
using GR.Crm.Abstractions.ViewModels.SolutionTypeViewModels;
using GR.Crm.Abstractions.ViewModels.SourceViewModels;
using GR.Crm.Abstractions.ViewModels.TechnologyTypeViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GR.Crm.Razor.Controllers
{
    public class CrmVocabulariesController : BaseGearController
    {
        #region Injectable

        /// <summary>
        /// contact service
        /// </summary>
        private readonly IVocabulariesService _crmVocabulariesService;

        #endregion

        public IActionResult JobPositions()
        {
            return View();
        }
        public IActionResult Sources()
        {
            return View();
        }
        public IActionResult SolutionTypes()
        {
            return View();
        }
        public IActionResult ProductTypes()
        {
            return View();
        }
        public IActionResult ServiceTypes()
        {
            return View();
        }
        public IActionResult TechnologyTypes()
        {
            return View();
        }

        public CrmVocabulariesController(IVocabulariesService crmVocabulariesService)
        {
            _crmVocabulariesService = crmVocabulariesService;
        }


        #region JopPosition



        /// <summary>
        /// Get all paginated jop position
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<PagedResult<JobPositionViewModel>>))]
        public async Task<JsonResult> GetAllPaginatedJobPositions(PageRequest request)
            => await JsonAsync(_crmVocabulariesService.GetAllPaginatedJobPositionsAsync(request));

        /// <summary>
        /// Get all jop position
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<JobPositionViewModel>>))]
        public async Task<JsonResult> GetAllJobPositions()
            => await JsonAsync(_crmVocabulariesService.GetAllJobPositionsAsync());


        /// <summary>
        /// Get job position by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<JobPositionViewModel>))]
        public async Task<JsonResult> GetJobPositionById([Required] Guid id)
            => await JsonAsync(_crmVocabulariesService.GetJobPositionByIdAsync(id));



        /// <summary>
        /// Add new job position
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> AddNewJobPosition([Required] JobPositionViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_crmVocabulariesService.AddNewJobPositionAsync(model));
        }

        /// <summary>
        /// Update job position
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdateJobPosition([Required] JobPositionViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_crmVocabulariesService.UpdateJobPositionAsync(model));
        }


        /// <summary>
        /// Delete job position by id
        /// </summary>
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DeleteJobPositionById([Required] Guid id)
            => await JsonAsync(_crmVocabulariesService.DeleteJobPositionByIdAsync(id));

        /// <summary>
        /// Activate job position 
        /// </summary>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> ActivateJobPositionById([Required] Guid id)
            => await JsonAsync(_crmVocabulariesService.ActivateJobPositionByIdAsync(id));

        /// <summary>
        /// Disable job position 
        /// </summary>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DisableJobPositionById([Required] Guid id)
            => await JsonAsync(_crmVocabulariesService.DisableJobPositionByIdAsync(id));

        #endregion

        #region Source

        /// <summary>
        /// Get all paginated sources
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<PagedResult<GetSourceViewModel>>))]
        public async Task<JsonResult> GetAllPaginatedSources(PageRequest request)
            => await JsonAsync(_crmVocabulariesService.GetAllPaginatedSourcesAsync(request));

        /// <summary>
        /// Get all sources
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<GetSourceViewModel>>))]
        public async Task<JsonResult> GetAllSourcesAsync(bool includeDeleted = false)
            => await JsonAsync(_crmVocabulariesService.GetAllSourcesAsync(includeDeleted));


        /// <summary>
        /// Get source by id
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<GetSourceViewModel>))]
        public async Task<JsonResult> GetSourceById([Required] Guid sourceId)
            => await JsonAsync(_crmVocabulariesService.GetSourceByIdAsync(sourceId));



        /// <summary>
        /// Add new source
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> AddSource([Required] SourceViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_crmVocabulariesService.AddSourceAsync(model));
        }

        /// <summary>
        /// Update source
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdateSource([Required] SourceViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_crmVocabulariesService.UpdateSourceAsync(model));
        }


        /// <summary>
        /// Delete source by id
        /// </summary>
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DeleteSource([Required] Guid sourceId)
            => await JsonAsync(_crmVocabulariesService.DeleteSourceAsync(sourceId));

        /// <summary>
        /// Activate source 
        /// </summary>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> ActivateSource([Required] Guid sourceId)
            => await JsonAsync(_crmVocabulariesService.ActivateSourceAsync(sourceId));

        /// <summary>
        /// Disable source
        /// </summary>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DisableSource([Required] Guid sourceId)
            => await JsonAsync(_crmVocabulariesService.DisableSourceAsync(sourceId));


        #endregion

        #region SolutionType

        /// <summary>
        /// Get all paginated solution type
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<PagedResult<GetSolutionTypeViewModel>>))]
        public async Task<JsonResult> GetAllPaginatedSolutionType(PageRequest request)
            => await JsonAsync(_crmVocabulariesService.GetAllPaginatedSolutionTypeAsync(request));

        /// <summary>
        /// Get all solution type
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<GetSolutionTypeViewModel>>))]
        public async Task<JsonResult> GetAllSolutionType(bool includeDeleted = false)
            => await JsonAsync(_crmVocabulariesService.GetAllSolutionTypeAsync(includeDeleted));


        /// <summary>
        /// Get solution type by id
        /// </summary>
        /// <param name="solutionTypeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<GetSolutionTypeViewModel>))]
        public async Task<JsonResult> GetSolutionTypeById([Required] Guid solutionTypeId)
            => await JsonAsync(_crmVocabulariesService.GetSolutionTypeByIdAsync(solutionTypeId));



        /// <summary>
        /// Add new solution type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> AddSolutionType([Required] SolutionTypeViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_crmVocabulariesService.AddSolutionTypeAsync(model));
        }

        /// <summary>
        /// Update solution type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdateSolutionType([Required] SolutionTypeViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_crmVocabulariesService.UpdateSolutionTypeAsync(model));
        }


        /// <summary>
        /// Delete solution type by id
        /// </summary>
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DeleteSolutionType([Required] Guid solutionTypeId)
            => await JsonAsync(_crmVocabulariesService.DeleteSolutionTypeAsync(solutionTypeId));

        /// <summary>
        /// Activate solution type 
        /// </summary>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> ActivateSolutionType([Required] Guid solutionTypeId)
            => await JsonAsync(_crmVocabulariesService.ActivateSolutionTypeAsync(solutionTypeId));

        /// <summary>
        /// Disable solution type
        /// </summary>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DisableSolutionType([Required] Guid solutionTypeId)
            => await JsonAsync(_crmVocabulariesService.DisableSolutionTypeAsync(solutionTypeId));


        #endregion

        #region TechnologyType

        /// <summary>
        /// Get all paginated technology type
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<PagedResult<GetTechnologyTypeViewModel>>))]
        public async Task<JsonResult> GetAllPaginatedTechnologyType(PageRequest request)
            => await JsonAsync(_crmVocabulariesService.GetAllPaginatedTechnologyTypeAsync(request));

        /// <summary>
        /// Get all technology type
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<GetTechnologyTypeViewModel>>))]
        public async Task<JsonResult> GetAllTechnologyType(bool includeDeleted = false)
            => await JsonAsync(_crmVocabulariesService.GetAllTechnologyTypeAsync(includeDeleted));


        /// <summary>
        /// Get technology type by id
        /// </summary>
        /// <param name="technologyTypeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<GetTechnologyTypeViewModel>))]
        public async Task<JsonResult> GetTechnologyTypeById([Required] Guid technologyTypeId)
            => await JsonAsync(_crmVocabulariesService.GetTechnologyTypeByIdAsync(technologyTypeId));



        /// <summary>
        /// Add new technology type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> AddTechnologyType([Required] TechnologyTypeViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_crmVocabulariesService.AddTechnologyTypeAsync(model));
        }

        /// <summary>
        /// Update technology type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdateTechnologyType([Required] TechnologyTypeViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_crmVocabulariesService.UpdateTechnologyTypeAsync(model));
        }


        /// <summary>
        /// Delete technology by id
        /// </summary>
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DeleteTechnologyType([Required] Guid technologyTypeId)
            => await JsonAsync(_crmVocabulariesService.DeleteTechnologyTypeAsync(technologyTypeId));

        /// <summary>
        /// Activate technology type 
        /// </summary>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> ActivateTechnologyType([Required] Guid technologyTypeId)
            => await JsonAsync(_crmVocabulariesService.ActivateTechnologyTypeAsync(technologyTypeId));

        /// <summary>
        /// Disable technology type
        /// </summary>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DisableTechnologyType([Required] Guid technologyTypeId)
            => await JsonAsync(_crmVocabulariesService.DisableTechnologyTypeAsync(technologyTypeId));


        #endregion

        #region ProductType

        /// <summary>
        /// Get all paginated product  type
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<PagedResult<GetProductTypeViewModel>>))]
        public async Task<JsonResult> GetAllPaginatedProductType(PageRequest request)
            => await JsonAsync(_crmVocabulariesService.GetAllPaginatedProductTypeAsync(request));

        /// <summary>
        /// Get all product type
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<GetProductTypeViewModel>>))]
        public async Task<JsonResult> GetAllProductType(bool includeDeleted = false)
            => await JsonAsync(_crmVocabulariesService.GetAllProductTypeAsync(includeDeleted));


        /// <summary>
        /// Get product type by id
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<GetProductTypeViewModel>))]
        public async Task<JsonResult> GetProductTypeById([Required] Guid productTypeId)
            => await JsonAsync(_crmVocabulariesService.GetProductTypeByIdAsync(productTypeId));



        /// <summary>
        /// Add new product type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> AddProductType([Required] ProductTypeViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_crmVocabulariesService.AddProductTypeAsync(model));
        }

        /// <summary>
        /// Update product type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdateProductType([Required] ProductTypeViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_crmVocabulariesService.UpdateProductTypeAsync(model));
        }


        /// <summary>
        /// Delete product by id
        /// </summary>
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DeleteProductType([Required] Guid productTypeId)
            => await JsonAsync(_crmVocabulariesService.DeleteProductTypeAsync(productTypeId));

        /// <summary>
        /// Activate product type 
        /// </summary>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> ActivateProductType([Required] Guid productTypeId)
            => await JsonAsync(_crmVocabulariesService.ActivateProductTypeAsync(productTypeId));

        /// <summary>
        /// Disable product type
        /// </summary>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DisableProductType([Required] Guid productTypeId)
            => await JsonAsync(_crmVocabulariesService.DisableProductTypeAsync(productTypeId));


        #endregion

        #region ServiceType

        /// <summary>
        /// Get all paginated service type
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<PagedResult<GetServiceTypeViewModel>>))]
        public async Task<JsonResult> GetAllPaginatedServiceType(PageRequest request)
            => await JsonAsync(_crmVocabulariesService.GetAllPaginatedServiceTypeAsync(request));

        /// <summary>
        /// Get all service type
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<GetServiceTypeViewModel>>))]
        public async Task<JsonResult> GetAllServiceType(bool includeDeleted = false)
            => await JsonAsync(_crmVocabulariesService.GetAllServiceTypeAsync(includeDeleted));


        /// <summary>
        /// Get service type by id
        /// </summary>
        /// <param name="serviceTypeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<GetServiceTypeViewModel>))]
        public async Task<JsonResult> GetServiceTypeByIdA([Required] Guid serviceTypeId)
            => await JsonAsync(_crmVocabulariesService.GetServiceTypeByIdAsync(serviceTypeId));



        /// <summary>
        /// Add new service type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> AddServiceType([Required] ServiceTypeViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_crmVocabulariesService.AddServiceTypeAsync(model));
        }

        /// <summary>
        /// Update service type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdateServiceType([Required] ServiceTypeViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_crmVocabulariesService.UpdateServiceTypeAsync(model));
        }


        /// <summary>
        /// Delete service by id
        /// </summary>
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DeleteServiceType([Required] Guid serviceTypeId)
            => await JsonAsync(_crmVocabulariesService.DeleteServiceTypeAsync(serviceTypeId));

        /// <summary>
        /// Activate service type 
        /// </summary>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> ActivateServiceType([Required] Guid serviceTypeId)
            => await JsonAsync(_crmVocabulariesService.ActivateServiceTypeAsync(serviceTypeId));

        /// <summary>
        /// Disable service type
        /// </summary>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DisableServiceType([Required] Guid serviceTypeId)
            => await JsonAsync(_crmVocabulariesService.DisableServiceTypeAsync(serviceTypeId));


        #endregion

    }
}
