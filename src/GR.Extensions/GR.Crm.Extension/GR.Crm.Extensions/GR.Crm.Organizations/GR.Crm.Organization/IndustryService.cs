using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Organizations.Abstractions;
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.ViewModels.IndustriesViewModels;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace GR.Crm.Organizations
{
    public class IndustryService : ICrmIndustryService
    {
        #region Injectable

        /// <summary>
        /// organization context
        /// </summary>
        private readonly ICrmOrganizationContext _context;

        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;


        #endregion

        public IndustryService(ICrmOrganizationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        /// <summary>
        /// Get All paginated Industries
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<GetIndustryViewModel>>> GetAllPaginatedIndustriesAsync(PageRequest request)
        {
            var listIndustries = await _context.Industries
                .Where(x => !x.IsDeleted || request.IncludeDeleted)
                .GetPagedAsync(request);

            var map = listIndustries.Map(_mapper.Map<IEnumerable<GetIndustryViewModel>>(listIndustries.Result));

            return new SuccessResultModel<PagedResult<GetIndustryViewModel>>(map);
        }


        /// <summary>
        /// Get All Industries
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetIndustryViewModel>>> GetAllIndustriesAsync(bool includeDeleted = false)
        {
            var listIndustries = await _context.Industries
                .Where(x=> !x.IsDeleted || includeDeleted)
                .ToListAsync();

            return  new SuccessResultModel<IEnumerable<GetIndustryViewModel>>(listIndustries.Adapt<IEnumerable<GetIndustryViewModel>>());
        }

        /// <summary>
        /// Get industry by id
        /// </summary>
        /// <param name="industryId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetIndustryViewModel>> GetIndustryByIdAsync(Guid? industryId)
        {
            if(industryId == null)
                return new InvalidParametersResultModel<GetIndustryViewModel>();

            var industry = await _context.Industries
                .FirstOrDefaultAsync(x => x.Id == industryId);

            if(industry == null)
                return  new NotFoundResultModel<GetIndustryViewModel>();

            return new SuccessResultModel<GetIndustryViewModel>(industry.Adapt<GetIndustryViewModel>());
        }

        /// <summary>
        /// Deactivate industry by id
        /// </summary>
        /// <param name="industryId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisableIndustryAsync(Guid? industryId) =>
            await _context.DisableRecordAsync<Industry>(industryId);

        /// <summary>
        /// Activate Industry by id
        /// </summary>
        /// <param name="industryId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivateIndustryAsync(Guid? industryId) =>
            await _context.ActivateRecordAsync<Industry>(industryId);


        /// <summary>
        /// Delete industry
        /// </summary>
        /// <param name="industryId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteIndustryAsync(Guid? industryId)
        {
           if(industryId == null)
               return new InvalidParametersResultModel();

           var industry = await _context.Industries.FirstOrDefaultAsync(x => x.Id == industryId);

           if(industry == null)
               return  new NotFoundResultModel();

           _context.Industries.Remove(industry);
           return await _context.PushAsync();

        }

        /// <summary>
        /// Add new industry
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddNewIndustryAsync(IndustryViewModel model)
        {
            if(model == null)
                return new InvalidParametersResultModel<Guid>();

            var newIndustry = new Industry
            {
                Name = model.Name
            };

            await _context.Industries.AddAsync(newIndustry);
            var result = await _context.PushAsync();

            return  new ResultModel<Guid>{IsSuccess = result.IsSuccess, Errors = result.Errors, Result = newIndustry.Id };
        }

        /// <summary>
        /// Update industry
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateIndustryAsync(IndustryViewModel model)
        {
            if(model == null)
                return  new InvalidParametersResultModel();

            var industry = await _context.Industries.FirstOrDefaultAsync(x => x.Id == model.Id);

            if(industry == null)
                return new NotFoundResultModel();

            industry.Name = model.Name;

            _context.Industries.Update(industry);
            return await _context.PushAsync();
        }

    }
}
