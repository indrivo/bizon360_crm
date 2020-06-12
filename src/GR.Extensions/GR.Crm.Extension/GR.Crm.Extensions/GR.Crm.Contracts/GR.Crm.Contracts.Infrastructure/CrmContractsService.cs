using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using GR.Core.Attributes.Documentation;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Global;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Contracts.Abstractions;
using GR.Crm.Contracts.Abstractions.Models;
using GR.Crm.Contracts.Abstractions.ViewModels;
using GR.Identity.Abstractions;
using GR.Notifications.Abstractions;
using GR.Notifications.Abstractions.Models.Notifications;
using Microsoft.EntityFrameworkCore;

namespace GR.Crm.Contracts.Infrastructure
{
    [Author(Authors.LUPEI_NICOLAE)]
    public class CrmContractsService : ICrmContractsService
    {
        #region Injectable

        /// <summary>
        /// Inject contracts context
        /// </summary>
        private readonly IContractsContext _context;

        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Inject notification
        /// </summary>
        private readonly INotify<GearRole> _notify;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        /// <param name="notify"></param>
        public CrmContractsService(IContractsContext context, 
            IMapper mapper,
            INotify<GearRole> notify)
        {
            _context = context;
            _mapper = mapper;
            _notify = notify;
        }

        /// <summary>
        /// Add new contract template
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddContractTemplateAsync([Required] AddTemplateViewModel model)
        {
            if (model == null) return new InvalidParametersResultModel<Guid>();
            var template = _mapper.Map<ContractTemplate>(model);
            await _context.ContractTemplates.AddAsync(template);
            var dbResult = await _context.PushAsync();

            await _notify.SendNotificationAsync(new Notification
            {
                Content = $"Add new document template {template?.Name}",
                Subject = "Info",
                NotificationTypeId = NotificationType.Info
            });


            return !dbResult.IsSuccess ? dbResult.Map<Guid>() : new SuccessResultModel<Guid>(template.Id);
        }

        /// <summary>
        /// Update new contract template
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateContractTemplateAsync([Required] UpdateTemplateViewModel model)
        {
            if (model == null) return new InvalidParametersResultModel();

            var contract = await _context.ContractTemplates
                .FirstOrDefaultAsync(x => x.Id.Equals(model.Id));

            if (contract == null)
                return new NotFoundResultModel();

            contract.Name = model.Name;
            contract.Description = model.Description;

            _context.ContractTemplates.Update(contract);
            return await _context.PushAsync();
        }

        /// <summary>
        /// Delete contract template
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteContractTemplateAsync(Guid? contractId) =>
            await _context.RemovePermanentRecordAsync<ContractTemplate>(contractId);


        /// <summary>
        /// Disable contract template
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisableContractTemplateAsync(Guid? contractId) =>
            await _context.DisableRecordAsync<ContractTemplate>(contractId);

        /// <summary>
        /// Activate contract template
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivateContractTemplateAsync(Guid? contractId) =>
            await _context.ActivateRecordAsync<ContractTemplate>(contractId);


        /// <summary>
        /// Add section to contract template
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddSectionToContractTemplateAsync([Required]AddSectionViewModel model)
        {
            if (model == null) return new InvalidParametersResultModel<Guid>();
            var section = _mapper.Map<ContractSection>(model);

            section.Order = await GetContractSectionCountAsync(model.ContractTemplateId);

            await _context.ContractSections.AddAsync(section);
            var dbResult = await _context.PushAsync();
            return dbResult.IsSuccess ? new SuccessResultModel<Guid>(section.Id) : dbResult.Map<Guid>();
        }

        /// <summary>
        /// Get contract template sections
        /// </summary>
        /// <param name="contractTemplateId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetTemplateSectionsViewModel>>>
            GetContractTemplateSectionsAsync(Guid? contractTemplateId)
        {
            if (!contractTemplateId.HasValue)
                return new InvalidParametersResultModel<IEnumerable<GetTemplateSectionsViewModel>>();
            var sections = await _context.ContractSections.Where(x => x.ContractTemplateId.Equals(contractTemplateId))
                .ToListAsync();
            var responseCollection = _mapper.Map<IEnumerable<GetTemplateSectionsViewModel>>(sections);
            return new SuccessResultModel<IEnumerable<GetTemplateSectionsViewModel>>(responseCollection);
        }

        /// <summary>
        /// Find template by id with includes
        /// </summary>
        /// <param name="contractTemplateId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<ContractTemplate>> FindContractTemplateByIdWithIncludesAsync(Guid? contractTemplateId)
        {
            if (contractTemplateId == null) return new InvalidParametersResultModel<ContractTemplate>();
            var template = await _context.ContractTemplates
                .AsNoTracking()
                .Include(x => x.Sections)
                .FirstOrDefaultAsync(x => x.Id.Equals(contractTemplateId));
            if (template == null) return new NotFoundResultModel<ContractTemplate>();
            template.Sections = template.Sections.OrderBy(x => x.Order).ToList();
            return new SuccessResultModel<ContractTemplate>(template);
        }

        /// <summary>
        /// Get all contract template
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<ContractTemplate>>> GetAllContractTemplateAsync(bool includeDeleted = false)
        {

            var litTemplate = await _context.ContractTemplates
                .Include(i => i.Sections)
                .Where(x => !x.IsDeleted || includeDeleted)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<ContractTemplate>>(litTemplate);
        }



        /// <summary>
        /// Get all paginated contract template
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<PagedResult<ContractTemplate>> GetAllPaginatedContractTemplateAsync(PageRequest request)
        {

            var litTemplate = await _context.ContractTemplates
                .Include(i => i.Sections)
                .Where(x => !x.IsDeleted || request.IncludeDeleted)
                .GetPagedAsync(request);

            var map = _mapper.Map<PagedResult<ContractTemplate>>(litTemplate);

            return map;
        }


        /// <summary>
        /// Find section by id
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<ContractSection>> FindSectionByIdAsync(Guid? sectionId)
        {
            if (sectionId == null) return new InvalidParametersResultModel<ContractSection>();
            var section = await _context.ContractSections
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id.Equals(sectionId));
            if (section == null) return new NotFoundResultModel<ContractSection>();
            return new SuccessResultModel<ContractSection>(section);
        }

        /// <summary>
        /// Update section template value
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateContractSectionAsync([Required] UpdateTemplateSectionViewModel model)
        {
            if (model == null) return new InvalidParametersResultModel();
            var sectionRequest = await FindSectionByIdAsync(model.Id);
            if (!sectionRequest.IsSuccess) return sectionRequest.ToBase();
            var section = sectionRequest.Result;
            section.Name = model.Name;
            section.TemplateValue = model.TemplateValue;
            _context.ContractSections.Update(section);
            return await _context.PushAsync();
        }

        /// <summary>
        /// Delete contractSection
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteContractSectionAsync(Guid? sectionId) =>
           await _context.RemovePermanentRecordAsync<ContractSection>(sectionId);


        /// <summary>
        /// Order stages
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<ResultModel> OrderSectionAsync(IEnumerable<OrderSectionViewModel> data)
        {
            foreach (var sectionOrder in data)
            {
                var section = await _context.ContractSections.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id.Equals(sectionOrder.SectionId));
                if (section == null) continue;
                section.Order = sectionOrder.Order;
                _context.ContractSections.Update(section);
            }

            return await _context.PushAsync();
        }


        /// <summary>
        /// Generate contract document from templates
        /// Token format: Some {Name} was pushed
        /// </summary>
        /// <param name="contractTemplateId"></param>
        /// <param name="dataTokens"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<MemoryStream>> GenerateDocumentFromTemplateAsync(Guid? contractTemplateId,
            IDictionary<string, string> dataTokens)
        {
            var templateRequest = await FindContractTemplateByIdWithIncludesAsync(contractTemplateId);
            if (!templateRequest.IsSuccess) return templateRequest.Map<MemoryStream>();
            var template = templateRequest.Result;



            var textHtml = template.Sections.OrderBy(o => o.Order).Aggregate("<html><head><meta charset='UTF - 8'></head><body>", (current, section) => current + section.TemplateValue);
            textHtml += "</body></html>";


            using (var memStream = new MemoryStream())
            {
                using (var wordDoc = WordprocessingDocument.Create(memStream,
                    DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))
                {

                    wordDoc.AddMainDocumentPart();
                    var mainDoc = new Document();
                    var body = new Body();


                    //Inject data into template value with marked tokens
                        var textWithTokens = textHtml.Inject(new Hashtable((IDictionary)dataTokens));

                        var cid = "MainSection";
                        //var ms = new MemoryStream(Encoding.UTF8.GetBytes(textWithTokens));
                        var ms = new MemoryStream(new UTF8Encoding(true).GetPreamble().Concat(Encoding.UTF8.GetBytes(textWithTokens)).ToArray());
                        var formatImportPart = wordDoc.MainDocumentPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html, cid);
                        formatImportPart.FeedData(ms);
                        var altChunk = new AltChunk { Id = cid };
                        body.Append(altChunk);
                    

                    mainDoc.AppendChild(body);
                    wordDoc.MainDocumentPart.Document = mainDoc;

                    wordDoc.Close();
                }

                return new SuccessResultModel<MemoryStream>(memStream);
            }
        }



        #region Helpers

        /// <summary>
        /// Get section count
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        protected virtual async Task<int> GetContractSectionCountAsync(Guid? contractId)
        {
            if (contractId == null) return default;
            var count = await _context.ContractSections
                .AsNoTracking()
                .NonDeleted()
                .CountAsync(x => x.ContractTemplateId.Equals(contractId));
            return count;
        }

        #endregion
    }
}