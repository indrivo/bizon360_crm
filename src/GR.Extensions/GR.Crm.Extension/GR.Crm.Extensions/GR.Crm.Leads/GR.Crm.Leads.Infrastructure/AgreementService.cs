using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Contracts.Abstractions;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Helpers;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Leads.Abstractions.ViewModels.AgreementsViewModels;
using GR.Crm.Organizations.Abstractions;
using GR.Identity.Abstractions;
using GR.Notifications.Abstractions;
using GR.Notifications.Abstractions.Models.Notifications;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace GR.Crm.Leads.Infrastructure
{
    public class AgreementService : IAgreementService
    {

        #region Injectable

        /// <summary>
        /// Inject context
        /// </summary>
        private readonly ILeadContext<Lead> _context;


        /// <summary>
        /// Inject contracts service
        /// </summary>
        private readonly ICrmContractsService _contractsService;

        /// <summary>
        /// InjectContact Service
        /// </summary>
        private readonly ICrmContactService _contactService;

        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Inject user manager
        /// </summary>
        private readonly IUserManager<GearUser> _userManager;

        /// <summary>
        /// Inject notification
        /// </summary>
        private readonly INotify<GearRole> _notify;

        #endregion


        public AgreementService(ILeadContext<Lead> context,
            ICrmContractsService contractsService,
            ICrmContactService contactService,
            IMapper mapper,
            IUserManager<GearUser> userManager, 
            INotify<GearRole> notify)
        {
            _context = context;
            _contractsService = contractsService;
            _contactService = contactService;
            _mapper = mapper;
            _userManager = userManager;
            _notify = notify;
        }

        /// <summary>
        /// Add agreement
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddAgreementAsync(AddAgreementViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var contactRequest = await _contactService.GetContactByIdAsync(model.ContactId);
            if (contactRequest.IsSuccess)
            {
                if (contactRequest.Result.OrganizationId != model.OrganizationId)
                    return new ResultModel<Guid> { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "The organization does not have this contact " } } };
            }

            var agreement = _mapper.Map<Agreement>(model);

            var existAgreement = _context.Agreements.Where(x =>
                string.Equals(x.Name, agreement.Name, StringComparison.CurrentCultureIgnoreCase));

            if (existAgreement.Any())
            {
                var version = await existAgreement.CountAsync();
                agreement.Name += " (" + version + ")";
            }


            _context.Agreements.Add(agreement);
            var result = await _context.PushAsync();

            if (result.IsSuccess)
            {
                await _notify.SendNotificationAsync(new List<Guid>{agreement.UserId}, new Notification
                {
                    Content = $"Add agreement {agreement?.Name}",
                    Subject = "Info",
                    NotificationTypeId = NotificationType.Info
                });
            }

            return result.Map<Guid>(agreement.Id);
        }

        /// <summary>
        /// Update agreement
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateAgreementAsync(UpdateAgreementViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var agreement = await _context.Agreements.FirstOrDefaultAsync(x => x.Id.Equals(model.Id));

            if (agreement == null)
                return new NotFoundResultModel();

            agreement.Name = model.Name;
            agreement.LeadId = model.LeadId;
            agreement.OrganizationId = model.OrganizationId;
            agreement.ContactId = model.ContactId;
            agreement.OrganizationAddressId = model.OrganizationAddressId;
            agreement.ProductId = model.ProductId;
            agreement.ContractTemplateId = model.ContractTemplateId;
            agreement.Values = model.Values;
            agreement.Commission = model.Commission;
            agreement.UserId = model.UserId;
            agreement.Description = model.Description;

            _context.Agreements.Update(agreement);
            return await _context.PushAsync();
        }

        /// <summary>
        /// Get agreement by id
        /// </summary>
        /// <param name="agreementId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetAgreementViewModel>> GetAgreementByIdAsync(Guid? agreementId)
        {
            if (agreementId == null)
                return new InvalidParametersResultModel<GetAgreementViewModel>();

            var agreement = await _context.Agreements
                .Include(i => i.Lead)
                .Include(i => i.Organization)
                .Include(i => i.Product)
                .Include(i => i.Contact)
                .Include(i => i.OrganizationAddress)
                .ThenInclude(i => i.City)
                .ThenInclude(i => i.Region)
                .FirstOrDefaultAsync(x => x.Id.Equals(agreementId));

            if (agreement == null)
                return new NotFoundResultModel<GetAgreementViewModel>();

            return new SuccessResultModel<GetAgreementViewModel>(_mapper.Map<GetAgreementViewModel>(agreement));
        }



        /// <summary>
        /// Get all paginated agreements for table
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<GetTableAgreementViewModel>>> GetAllTablePaginatedAgreementsAsync(PageRequest request)
        {

            var allUsers = await _userManager.UserManager.Users.ToListAsync();

            var pagedResult = await _context.Agreements
                .Include(i => i.Lead)
                .Include(i => i.Organization)
                .Include(i => i.Product)
                .Where(x => !x.IsDeleted || request.IncludeDeleted)
                .Select(s => new GetTableAgreementViewModel
                {
                    Id = s.Id,
                    Created = s.Created,
                    Name = s.Name,
                    OrganizationName = s.Organization.Name,
                    LeadName = s.Lead.Name,
                    IsDeleted = s.IsDeleted,
                    ProductName = s.Product.Name,
                    UserId = s.UserId,
                    UserName = allUsers.FirstOrDefault(x=> x.Id == s.UserId.ToString()) != null 
                               ? allUsers.FirstOrDefault(x => x.Id == s.UserId.ToString()).UserFirstName + " " + allUsers.FirstOrDefault(x => x.Id == s.UserId.ToString()).UserLastName
                               : ""
                })
                .GetPagedAsync(request);

            var map = pagedResult.Map(pagedResult.Result);

            return new SuccessResultModel<PagedResult<GetTableAgreementViewModel>>(map);
        }

        /// <summary>
        /// Get all paginated agreements
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<GetAgreementViewModel>>> GetAllPaginatedAgreementsAsync(PageRequest request)
        {
            //request.PageRequestFilters = new List<PageRequestFilter>
            //{
            //    new PageRequestFilter{Propriety = "UserId", Value = "cf09c97c-0061-4693-a000-10d39ebb32ed"},
            //    new PageRequestFilter{Propriety = "IsDeleted", Value = "true"}
            //};


            var pagedResult = await _context.Agreements
                .Include(i => i.Lead)
                .Include(i => i.Organization)
                .Include(i => i.Product)
                .Include(i => i.Contact)
                .Include(i => i.OrganizationAddress)
                .ThenInclude(i => i.City)
                .ThenInclude(i => i.Region)
                .Where(x => !x.IsDeleted || request.IncludeDeleted)
                .GetPagedAsync(request);


            var map = pagedResult.Map(_mapper.Map<IEnumerable<GetAgreementViewModel>>(pagedResult.Result));

            return new SuccessResultModel<PagedResult<GetAgreementViewModel>>(map);
        }


        /// <summary>
        /// Get all agreements
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetAgreementViewModel>>> GetAllAgreementsAsync(bool includeDeleted = false)
        {

            var agreements = await _context.Agreements
                .Include(i => i.Lead)
                .Include(i => i.Organization)
                .Include(i => i.Product)
                .Include(i => i.Contact)
                .Include(i => i.OrganizationAddress)
                .ThenInclude(i => i.City)
                .ThenInclude(i => i.Region)
                .Where(x => !x.IsDeleted || includeDeleted)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<GetAgreementViewModel>>(_mapper.Map<IEnumerable<GetAgreementViewModel>>(agreements));
        }

        /// <summary>
        /// Get all agreement by lead id
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetAgreementViewModel>>> GetAllAgreementsByLeadIdAsync(Guid? leadId)
        {
            if (leadId == null)
                return new InvalidParametersResultModel<IEnumerable<GetAgreementViewModel>>();

            var agreements = await _context.Agreements
                .Include(i => i.Lead)
                .Include(i => i.Organization)
                .Include(i => i.Product)
                .Include(i => i.Contact)
                .Include(i => i.OrganizationAddress)
                .ThenInclude(i => i.City)
                .ThenInclude(i => i.Region)
                .Where(x => x.LeadId == leadId)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<GetAgreementViewModel>>(_mapper.Map<IEnumerable<GetAgreementViewModel>>(agreements));
        }

        /// <summary>
        /// Get all agreements by organization id
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetAgreementViewModel>>> GetAllAgreementsByOrganizationIdIdAsync(Guid? organizationId)
        {
            if (organizationId == null)
                return new InvalidParametersResultModel<IEnumerable<GetAgreementViewModel>>();

            var agreements = await _context.Agreements
                .Include(i => i.Lead)
                .Include(i => i.Organization)
                .Include(i => i.Product)
                .Include(i => i.Contact)
                .Include(i => i.OrganizationAddress)
                .ThenInclude(i => i.City)
                .ThenInclude(i => i.Region)
                .Where(x => x.OrganizationId == organizationId)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<GetAgreementViewModel>>(_mapper.Map<IEnumerable<GetAgreementViewModel>>(agreements));
        }

        /// <summary>
        /// Disable agreements
        /// </summary>
        /// <param name="agreementId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisableAgreementAsync(Guid? agreementId)
        {

            var agreement = await _context.Agreements.AsNoTracking().FirstOrDefaultAsync(x => x.Id == agreementId);
            var currentUser = await _userManager.GetCurrentUserAsync();

            if (!currentUser.IsSuccess) return await _context.DisableRecordAsync<Agreement>(agreementId);

            if (agreement.Author != currentUser.Result.UserName)
                return new ResultModel
                {
                    IsSuccess = false,
                    Errors = new List<IErrorModel> { new ErrorModel { Message = "Not has permission to disable agreement" } }
                };

            return await _context.DisableRecordAsync<Agreement>(agreementId);
        }


        /// <summary>
        /// Activate agreement
        /// </summary>
        /// <param name="agreementId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivateAgreementAsync(Guid? agreementId) =>
            await _context.ActivateRecordAsync<Agreement>(agreementId);

        /// <summary>
        /// Delete permanently 
        /// </summary>
        /// <param name="agreementId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteAgreementAsync(Guid? agreementId) =>
            await _context.RemovePermanentRecordAsync<Agreement>(agreementId);


        #region Contract

        /// <summary>
        /// Generate contract
        /// </summary>
        /// <param name="agreementId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<MemoryStream>> GenerateContractForAgreementIdAsync(Guid? agreementId)
        {
            if (agreementId == null) return new InvalidParametersResultModel<MemoryStream>();

            var agreementRequest = await GetAgreementByIdAsync(agreementId);

            if (!agreementRequest.IsSuccess)
                return agreementRequest.Map<MemoryStream>();

            var agreement = agreementRequest.Result;

            var dataRequest = await GetTagsValues(agreementId);
            if (!dataRequest.IsSuccess) return dataRequest.Map<MemoryStream>();
            var data = dataRequest.Result;

            return await _contractsService.GenerateDocumentFromTemplateAsync(agreement.ContractTemplateId, data);
        }



        private async Task<ResultModel<IDictionary<string, string>>> GetTagsValues(Guid? agreementId)
        {
            if (agreementId == null)
                return new InvalidParametersResultModel<IDictionary<string, string>>();

            var agreement = await _context.Agreements
                .Include(i => i.Organization)
                .Include(i => i.Contact)
                .Include(i => i.Lead)
                .ThenInclude(i => i.Currency)
                .Include(i => i.OrganizationAddress)
                .ThenInclude(i => i.City)
                .ThenInclude(i => i.Region)
                .FirstOrDefaultAsync(x => x.Id.Equals(agreementId));

            var tagsValues = ContractTagsHelper.GetInjectedStrings(agreement);

            return new SuccessResultModel<IDictionary<string, string>>(tagsValues);
        }

        #endregion
    }
}
