using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Abstractions;
using GR.Crm.Abstractions.Models;
using GR.Crm.Abstractions.ViewModels.JobPositionViewModels;
using GR.Crm.Abstractions.ViewModels.ProductTypeViewModels;
using GR.Crm.Abstractions.ViewModels.ServiceTypeViewModels;
using GR.Crm.Abstractions.ViewModels.SolutionTypeViewModels;
using GR.Crm.Abstractions.ViewModels.SourceViewModels;
using GR.Crm.Abstractions.ViewModels.TechnologyTypeViewModels;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Guid = System.Guid;

namespace GR.Crm
{
    public class VocabulariesService : IVocabulariesService
    {

        #region Injectable

        /// <summary>
        /// Inject context
        /// </summary>
        private readonly ICrmContext _context;


        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;

        #endregion


        public VocabulariesService(ICrmContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        #region JobPosition


        /// <summary>
        /// Get all paginated jop position
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<JobPositionViewModel>>> GetAllPaginatedJobPositionsAsync(PageRequest request)
        {
            var listJobPositions = await _context.JobPositions
                .Where(x => !x.IsDeleted || request.IncludeDeleted)
                .GetPagedAsync(request);

            var map = listJobPositions.Map(_mapper.Map<IEnumerable<JobPositionViewModel>>(listJobPositions.Result));

            return new SuccessResultModel<PagedResult<JobPositionViewModel>>(map);
        }


        /// <summary>
        /// Get all jop position
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<JobPositionViewModel>>> GetAllJobPositionsAsync()
        {
            var listJobPositions = await _context.JobPositions
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<JobPositionViewModel>>(listJobPositions.Adapt<IEnumerable<JobPositionViewModel>>());
        }

        /// <summary>
        /// Get job position by id
        /// </summary>
        /// <param name="jobPositionId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<JobPositionViewModel>> GetJobPositionByIdAsync(Guid? jobPositionId)
        {
            if (jobPositionId == null)
                return new InvalidParametersResultModel<JobPositionViewModel>();

            var jobPosition = await _context.JobPositions
                .FirstOrDefaultAsync(x => x.Id == jobPositionId);

            if (jobPosition == null)
                return new NotFoundResultModel<JobPositionViewModel>();

            return new SuccessResultModel<JobPositionViewModel>(jobPosition.Adapt<JobPositionViewModel>());
        }

        /// <summary>
        /// Add new job position
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddNewJobPositionAsync(JobPositionViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var newJobPosition = new JobPosition
            {
                Name = model.Name
            };

            await _context.JobPositions.AddAsync(newJobPosition);
            var result = await _context.PushAsync();

            return new ResultModel<Guid>
            { Result = newJobPosition.Id, IsSuccess = result.IsSuccess, Errors = result.Errors };
        }

        /// <summary>
        /// Update job position
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateJobPositionAsync(JobPositionViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var jobPosition = await _context.JobPositions.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (jobPosition == null)
                return new NotFoundResultModel();

            jobPosition.Name = model.Name;

            _context.JobPositions.Update(jobPosition);
            return await _context.PushAsync();
        }

        /// <summary>
        /// Delete job position by id
        /// </summary>
        /// <param name="jobPositionId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteJobPositionByIdAsync(Guid? jobPositionId)
        {
            if (jobPositionId == null)
                return new InvalidParametersResultModel();

            var jobPosition = await _context.JobPositions
                .FirstOrDefaultAsync(x => x.Id == jobPositionId);

            if (jobPosition == null)
                return new NotFoundResultModel();

            _context.JobPositions.Remove(jobPosition);
            return await _context.PushAsync();

        }

        /// <summary>
        /// Activate job position 
        /// </summary>
        /// <param name="jobPositionId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivateJobPositionByIdAsync(Guid? jobPositionId) =>
            await _context.ActivateRecordAsync<JobPosition>(jobPositionId);

        /// <summary>
        /// Disable job position
        /// </summary>
        /// <param name="jobPositionId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisableJobPositionByIdAsync(Guid? jobPositionId) =>
            await _context.DisableRecordAsync<JobPosition>(jobPositionId);

        #endregion


        #region Source

        /// <summary>
        /// Get all sources
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetSourceViewModel>>> GetAllSourcesAsync(
            bool includeDeleted = false)
        {
            var listSources = await _context.Sources
                .Where(x => !x.IsDeleted || includeDeleted).ToListAsync();

            return new SuccessResultModel<IEnumerable<GetSourceViewModel>>(_mapper.Map<IEnumerable<GetSourceViewModel>>(listSources));

        }

        /// <summary>
        /// get all paginated sources
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<GetSourceViewModel>>> GetAllPaginatedSourcesAsync(
            PageRequest request)
        {
            if (request == null)
                return new InvalidParametersResultModel<PagedResult<GetSourceViewModel>>();

            var listSources = await _context.Sources
                .Where(x => !x.IsDeleted || request.IncludeDeleted)
                .GetPagedAsync(request);

            var map = listSources.Map(_mapper.Map<IEnumerable<GetSourceViewModel>>(listSources.Result));
            return new SuccessResultModel<PagedResult<GetSourceViewModel>>(map);
        }

        /// <summary>
        /// get source by id
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetSourceViewModel>> GetSourceByIdAsync(Guid? sourceId)
        {
            if (sourceId == null)
                return new InvalidParametersResultModel<GetSourceViewModel>();

            var source = await _context.Sources.FirstOrDefaultAsync(x => x.Id == sourceId);

            if (source == null)
                return new NotFoundResultModel<GetSourceViewModel>();

            return new SuccessResultModel<GetSourceViewModel>(_mapper.Map<GetSourceViewModel>(source));
        }

        /// <summary>
        /// add source 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddSourceAsync(SourceViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var source = _mapper.Map<Source>(model);

            await _context.Sources.AddAsync(source);
            var result = await _context.PushAsync();

            return result.Map(source.Id);
        }

        /// <summary>
        /// update source 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateSourceAsync(SourceViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var source = await _context.Sources.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (source == null)
                return new NotFoundResultModel();

            source = _mapper.Map<Source>(model);

             _context.Sources.Update(source);
             return await _context.PushAsync();
        }

        /// <summary>
        /// Disable source 
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisableSourceAsync(Guid? sourceId) =>
            await _context.DisableRecordAsync<Source>(sourceId);

        /// <summary>
        /// Activate source
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivateSourceAsync(Guid? sourceId) =>
            await _context.ActivateRecordAsync<Source>(sourceId);

        /// <summary>
        /// Delete source
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteSourceAsync(Guid? sourceId) =>
            await _context.RemovePermanentRecordAsync<Source>(sourceId);

        #endregion


        #region SolutionType

        /// <summary>
        /// Get all solution type
        /// </summary>
        /// <param name="IncludeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetSolutionTypeViewModel>>> GetAllSolutionTypeAsync(
            bool IncludeDeleted = false)
        {
            var solutions = await _context.SolutionTypes
                .Where(x => !x.IsDeleted || IncludeDeleted)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<GetSolutionTypeViewModel>>(_mapper.Map<IEnumerable<GetSolutionTypeViewModel>>(solutions));

        }

        /// <summary>
        /// Get paginated solution type
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<GetSolutionTypeViewModel>>> GetAllPaginatedSolutionTypeAsync(PageRequest request)
        {
            var solutions = await _context.SolutionTypes
                .Where(x => !x.IsDeleted || request.IncludeDeleted)
                .GetPagedAsync(request);

            var map = solutions.Map(_mapper.Map<IEnumerable<GetSolutionTypeViewModel>>(solutions.Result));

            return new SuccessResultModel<PagedResult<GetSolutionTypeViewModel>>(map);

        }

        /// <summary>
        /// Get solution type by id
        /// </summary>
        /// <param name="solutionTypeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetSolutionTypeViewModel>> GetSolutionTypeByIdAsync(Guid? solutionTypeId)
        {
            if (solutionTypeId == null)
                return new InvalidParametersResultModel<GetSolutionTypeViewModel>();

            var solution = await _context.SolutionTypes
                .FirstOrDefaultAsync(x => x.Id == solutionTypeId);

            if (solution == null)
                return new NotFoundResultModel<GetSolutionTypeViewModel>();

            return new SuccessResultModel<GetSolutionTypeViewModel>(_mapper.Map<GetSolutionTypeViewModel>(solution));
        }

        /// <summary>
        /// Add solution type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddSolutionTypeAsync(SolutionTypeViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var solutionType = _mapper.Map<SolutionType>(model);

            await _context.SolutionTypes.AddAsync(solutionType);
            var result = await _context.PushAsync();

            return result.Map(solutionType.Id);
        }

        /// <summary>
        /// Update solution type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateSolutionTypeAsync(SolutionTypeViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var solutionType = await _context.SolutionTypes.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (solutionType == null)
                return new NotFoundResultModel();

            solutionType = _mapper.Map<SolutionType>(model);

             _context.SolutionTypes.Update(solutionType);
            await _context.PushAsync();

            return await _context.PushAsync();
        }

        /// <summary>
        /// Disable solution type
        /// </summary>
        /// <param name="solutionTypeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisableSolutionTypeAsync(Guid? solutionTypeId) =>
            await _context.DisableRecordAsync<SolutionType>(solutionTypeId);

        /// <summary>
        /// Activate solution type
        /// </summary>
        /// <param name="solutionTypeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivateSolutionTypeAsync(Guid? solutionTypeId) =>
            await _context.ActivateRecordAsync<SolutionType>(solutionTypeId);


        /// <summary>
        /// Delete solution type
        /// </summary>
        /// <param name="solutionTypeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteSolutionTypeAsync(Guid? solutionTypeId) =>
            await _context.RemovePermanentRecordAsync<SolutionType>(solutionTypeId);

        #endregion


        #region TechnologyType

        /// <summary>
        /// Get all technology type
        /// </summary>
        /// <param name="IncludeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetTechnologyTypeViewModel>>> GetAllTechnologyTypeAsync(
            bool IncludeDeleted = false)
        {
            var technologyTypes = await _context.TechnologyTypes
                .Where(x => !x.IsDeleted || IncludeDeleted)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<GetTechnologyTypeViewModel>>(_mapper.Map<IEnumerable<GetTechnologyTypeViewModel>>(technologyTypes));

        }

        /// <summary>
        /// Get all paginated technology type
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<GetTechnologyTypeViewModel>>> GetAllPaginatedTechnologyTypeAsync(PageRequest request)
        {
            var technologyTypes = await _context.TechnologyTypes
                .Where(x => !x.IsDeleted || request.IncludeDeleted)
                .GetPagedAsync(request);

            var map = technologyTypes.Map(_mapper.Map<IEnumerable<GetTechnologyTypeViewModel>>(technologyTypes.Result));

            return new SuccessResultModel<PagedResult<GetTechnologyTypeViewModel>>(map);

        }

        /// <summary>
        /// Get technology type by id
        /// </summary>
        /// <param name="solutionTypeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetTechnologyTypeViewModel>> GetTechnologyTypeByIdAsync(Guid? solutionTypeId)
        {
            if (solutionTypeId == null)
                return new InvalidParametersResultModel<GetTechnologyTypeViewModel>();

            var technologyType = await _context.TechnologyTypes
                .FirstOrDefaultAsync(x => x.Id == solutionTypeId);

            if (technologyType == null)
                return new NotFoundResultModel<GetTechnologyTypeViewModel>();

            return new SuccessResultModel<GetTechnologyTypeViewModel>(_mapper.Map<GetTechnologyTypeViewModel>(technologyType));
        }

        /// <summary>
        /// Add technology type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddTechnologyTypeAsync(TechnologyTypeViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var technologyType = _mapper.Map<TechnologyType>(model);

            await _context.TechnologyTypes.AddAsync(technologyType);
            var result = await _context.PushAsync();

            return result.Map(technologyType.Id);
        }

        /// <summary>
        /// Update technology type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateTechnologyTypeAsync(TechnologyTypeViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var technologyType = await _context.TechnologyTypes.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (technologyType == null)
                return new NotFoundResultModel();

            technologyType = _mapper.Map<TechnologyType>(model);

             _context.TechnologyTypes.Update(technologyType);
            return await _context.PushAsync();
        }

        /// <summary>
        /// Disable technology type 
        /// </summary>
        /// <param name="technologyTypeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisableTechnologyTypeAsync(Guid? technologyTypeId) =>
            await _context.DisableRecordAsync<TechnologyType>(technologyTypeId);

        /// <summary>
        /// Activate technology type
        /// </summary>
        /// <param name="technologyTypeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivateTechnologyTypeAsync(Guid? technologyTypeId) =>
            await _context.ActivateRecordAsync<TechnologyType>(technologyTypeId);

        /// <summary>
        /// Delete technology type
        /// </summary>
        /// <param name="technologyTypeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteTechnologyTypeAsync(Guid? technologyTypeId) =>
            await _context.RemovePermanentRecordAsync<TechnologyType>(technologyTypeId);

        #endregion


        #region ProductType

        /// <summary>
        /// Get all product type
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetProductTypeViewModel>>> GetAllProductTypeAsync(
            bool includeDeleted = false)
        {
            var productType = await _context.ProductTypes
                .Where(x => !x.IsDeleted || includeDeleted)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<GetProductTypeViewModel>>(_mapper.Map<IEnumerable<GetProductTypeViewModel>>(productType));
        }

        /// <summary>
        /// get all paginated product type
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<GetProductTypeViewModel>>> GetAllPaginatedProductTypeAsync(
            PageRequest request)
        {
            if (request == null)
                return new InvalidParametersResultModel<PagedResult<GetProductTypeViewModel>>();

            var pageResult = await _context.ProductTypes
                .Where(x => x.IsDeleted || request.IncludeDeleted)
                .GetPagedAsync(request);

            var map = pageResult.Map(_mapper.Map<IEnumerable<GetProductTypeViewModel>>(pageResult.Result));

            return new SuccessResultModel<PagedResult<GetProductTypeViewModel>>(map);
        }

        /// <summary>
        /// Get product type by id
        /// </summary>
        /// <param name="serviceTypeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetProductTypeViewModel>> GetProductTypeByIdAsync(
            Guid? serviceTypeId)
        {
            if (serviceTypeId == null)
                return new InvalidParametersResultModel<GetProductTypeViewModel>();

            var productTypes = await _context.ProductTypes
                .FirstOrDefaultAsync(x => x.Id == serviceTypeId);

            if (productTypes == null)
                return new NotFoundResultModel<GetProductTypeViewModel>();

            return new SuccessResultModel<GetProductTypeViewModel>(_mapper.Map<GetProductTypeViewModel>(productTypes));
        }

        /// <summary>
        /// Add product type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddProductTypeAsync(ProductTypeViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var productType = _mapper.Map<ProductType>(model);

            await _context.ProductTypes.AddAsync(productType);
            var result = await _context.PushAsync();

            return result.Map(productType.Id);
        }


        /// <summary>
        /// Update product type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateProductTypeAsync(ProductTypeViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var productType = await _context.ProductTypes.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (productType == null)
                return new NotFoundResultModel();

            productType = _mapper.Map<ProductType>(model);

            _context.ProductTypes.Update(productType);
            return await _context.PushAsync();
        }

        /// <summary>
        /// Disable product type
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisableProductTypeAsync(Guid? productTypeId) =>
            await _context.DisableRecordAsync<ProductType>(productTypeId);

        /// <summary>
        /// Activate product type
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivateProductTypeAsync(Guid? productTypeId) =>
            await _context.ActivateRecordAsync<ProductType>(productTypeId);

        /// <summary>
        /// Delete product type
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteProductTypeAsync(Guid? productTypeId) =>
            await _context.RemovePermanentRecordAsync<ProductType>(productTypeId);

        #endregion


        #region ServiceType

        /// <summary>
        /// Get all service type
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetServiceTypeViewModel>>> GetAllServiceTypeAsync(
            bool includeDeleted = false)
        {
            var serviceTypes = await _context.ServiceTypes
                .Where(x => !x.IsDeleted || includeDeleted)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<GetServiceTypeViewModel>>(_mapper.Map<IEnumerable<GetServiceTypeViewModel>>(serviceTypes));
        }

        /// <summary>
        /// get all paginated service type
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<GetServiceTypeViewModel>>> GetAllPaginatedServiceTypeAsync(
            PageRequest request)
        {
            if (request == null)
                return new InvalidParametersResultModel<PagedResult<GetServiceTypeViewModel>>();

            var pageResult = await _context.ServiceTypes
                .Where(x => x.IsDeleted || request.IncludeDeleted)
                .GetPagedAsync(request);

            var map = pageResult.Map(_mapper.Map<IEnumerable<GetServiceTypeViewModel>>(pageResult.Result));

            return new SuccessResultModel<PagedResult<GetServiceTypeViewModel>>(map);
        }

        /// <summary>
        /// Get service type by id
        /// </summary>
        /// <param name="serviceTypeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetServiceTypeViewModel>> GetServiceTypeByIdAsync(
            Guid? serviceTypeId)
        {
            if (serviceTypeId == null)
                return new InvalidParametersResultModel<GetServiceTypeViewModel>();

            var serviceTypes = await _context.ServiceTypes
                .FirstOrDefaultAsync(x => x.Id == serviceTypeId);

            if (serviceTypes == null)
                return new NotFoundResultModel<GetServiceTypeViewModel>();

            return new SuccessResultModel<GetServiceTypeViewModel>(_mapper.Map<GetServiceTypeViewModel>(serviceTypes));
        }

        /// <summary>
        /// Add service type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddServiceTypeAsync(ServiceTypeViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var serviceType = _mapper.Map<ServiceType>(model);

            await _context.ServiceTypes.AddAsync(serviceType);
            var result = await _context.PushAsync();

            return result.Map(serviceType.Id);
        }


        /// <summary>
        /// Update service type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateServiceTypeAsync(ServiceTypeViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var serviceType = await _context.ServiceTypes.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (serviceType == null)
                return new NotFoundResultModel();

            serviceType = _mapper.Map<ServiceType>(model);

            _context.ServiceTypes.Update(serviceType);
            return await _context.PushAsync();
        }

        /// <summary>
        /// Disable service type
        /// </summary>
        /// <param name="serviceTypeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisableServiceTypeAsync(Guid? serviceTypeId) =>
            await _context.DisableRecordAsync<ServiceType>(serviceTypeId);

        /// <summary>
        /// Activate service type
        /// </summary>
        /// <param name="serviceTypeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivateServiceTypeAsync(Guid? serviceTypeId) =>
            await _context.ActivateRecordAsync<ServiceType>(serviceTypeId);

        /// <summary>
        /// Delete service type
        /// </summary>
        /// <param name="serviceTypeId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteServiceTypeAsync(Guid? serviceTypeId) =>
            await _context.RemovePermanentRecordAsync<ServiceType>(serviceTypeId);

        #endregion


    }
}
