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
using GR.Crm.Organizations.Abstractions;
using GR.Crm.Organizations.Abstractions.Enums;
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.ViewModels.OrganizationsViewModels;
using GR.Identity.Abstractions;
using GR.Notifications.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;


namespace GR.Crm.Organizations
{
    public class OrganizationService : ICrmOrganizationService
    {

        #region Injectable

        /// <summary>
        /// Inject organization context
        /// </summary>
        private readonly ICrmOrganizationContext _organizationContext;

        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Inject notification
        /// </summary>
        private readonly INotify<GearRole> _notify;
        /// <summary>
        /// Inject address service
        /// </summary>
        private readonly IOrganizationAddressService _addressService;

        #endregion


        private const string notificationTitle = "Organization Notification";
        private const string verifiedAfiliat = "Virified afiliat #{0} if exist in sistem.";

        public OrganizationService(ICrmOrganizationContext organizationContext,
            IMapper mapper,
            INotify<GearRole> notify,
            IOrganizationAddressService addressService)
        {
            _organizationContext = organizationContext;
            _mapper = mapper;
            _notify = notify;
            _addressService = addressService;
        }

        /// <summary>
        /// Get all active organizations
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<GetOrganizationViewModel>>> GetPaginatedOrganizationAsync(PageRequest request)
        {
            if (request == null)
            {
                return new InvalidParametersResultModel<PagedResult<GetOrganizationViewModel>>();
            }

            var query = _organizationContext.Organizations
                .Include(x => x.Contacts)
                .ThenInclude(x => x.ContactWebProfiles)
                .ThenInclude(x => x.WebProfile)
                .Include(i => i.Industry)
                .Include(i => i.Addresses)
                .ThenInclude(i => i.City)
                .ThenInclude(i => i.Region)
                .Include(i=> i.Employee)
                .Where(x => !x.IsDeleted || request.IncludeDeleted);

            var pagedResult = await query.GetPagedAsync(request);
            var map = pagedResult.Map(_mapper.Map<IEnumerable<GetOrganizationViewModel>>(pagedResult.Result));

            return new SuccessResultModel<PagedResult<GetOrganizationViewModel>>(map);
        }

        /// <summary>
        /// Find organization by id
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetOrganizationViewModel>> FindOrganizationByIdAsync(Guid? organizationId)
        {
            if (organizationId == null)
                return new InvalidParametersResultModel<GetOrganizationViewModel>();

            var organizationBd = await _organizationContext.Organizations
                .Include(x => x.Contacts)
                .ThenInclude(x => x.ContactWebProfiles)
                .ThenInclude(x => x.WebProfile)
                .Include(i => i.Industry)
                .Include(i => i.Addresses)
                .ThenInclude(i => i.City)
                .ThenInclude(i => i.Region)
                .Include(i => i.Employee)
                .FirstOrDefaultAsync(x => x.Id == organizationId);

            if (organizationBd == null)
                return new NotFoundResultModel<GetOrganizationViewModel>();

            var organization = _mapper.Map<GetOrganizationViewModel>(organizationBd);
            return new SuccessResultModel<GetOrganizationViewModel>(organization);

        }


        /// <summary>
        /// Find organization by fiscal code
        /// </summary>
        /// <param name="fiscalCode"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetOrganizationViewModel>> GetOrganizationByFiscalCodeAsync(string fiscalCode)
        {
            if (string.IsNullOrEmpty(fiscalCode))
                return new InvalidParametersResultModel<GetOrganizationViewModel>();

            var organizationBd = await _organizationContext.Organizations
                .Include(x => x.Contacts)
                .ThenInclude(x => x.ContactWebProfiles)
                .ThenInclude(x => x.WebProfile)
                .Include(i => i.Industry)
                .Include(i => i.Addresses)
                .ThenInclude(i => i.City)
                .ThenInclude(i => i.Region)
                .Include(i => i.Employee)
                .FirstOrDefaultAsync(x => string.Equals(x.FiscalCode.Trim(), fiscalCode.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if (organizationBd == null)
                return new NotFoundResultModel<GetOrganizationViewModel>();

            return new SuccessResultModel<GetOrganizationViewModel>(_mapper.Map<GetOrganizationViewModel>(organizationBd));

        }


        /// <summary>
        /// Get all active organizations
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetOrganizationViewModel>>> GetAllActiveOrganizationsAsync(bool includeDeleted = false)
        {

            var listOrganizations = await _organizationContext.Organizations
                .Include(x => x.Contacts)
                .ThenInclude(x => x.ContactWebProfiles)
                .ThenInclude(x => x.WebProfile)
                .Include(i => i.Industry)
                .Include(i => i.Addresses)
                .ThenInclude(i => i.City)
                .ThenInclude(i => i.Region)
                .Include(i => i.Employee)
                .Where(x => !x.IsDeleted || includeDeleted)
                .ToListAsync();


            return new SuccessResultModel<IEnumerable<GetOrganizationViewModel>>
                (_mapper.Map<IEnumerable<GetOrganizationViewModel>>(listOrganizations));
        }

        /// <summary>
        /// Delete organization async
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteOrganizationAsync(Guid? organizationId)
        {
            if (organizationId == null)
                return new InvalidParametersResultModel();

            var organization = await _organizationContext.Organizations
                .FirstOrDefaultAsync(x => x.Id == organizationId);


            if (organization == null)
                return new NotFoundResultModel();

            _organizationContext.Organizations.Remove(organization);
            return await _organizationContext.PushAsync();
        }

        /// <summary>
        /// Deactivate organization
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeactivateOrganizationAsync(Guid? organizationId) =>
            await _organizationContext.DisableRecordAsync<Organization>(organizationId);

        /// <summary>
        /// Activate organization
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivateOrganizationAsync(Guid? organizationId) =>
            await _organizationContext.ActivateRecordAsync<Organization>(organizationId);

        /// <summary>
        /// add new organization async
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddNewOrganizationAsync(OrganizationViewModel model)
        {
            if (model == null)
                return new NotFoundResultModel<Guid>();

            var organizationBd =
                await _organizationContext.Organizations
                    .FirstOrDefaultAsync(x => string.Equals(x.FiscalCode.Trim(), model.FiscalCode.Trim(), StringComparison.CurrentCultureIgnoreCase));


            if (organizationBd != null)
                return new ResultModel<Guid>
                {
                    IsSuccess = false, Errors = new List<IErrorModel> {new ErrorModel {Message = "Organization exist"}}
                };

            var newOrganization = _mapper.Map<Organization>(model);
            await _organizationContext.Organizations.AddAsync(newOrganization);
            var result = await _organizationContext.PushAsync();

            return result.Map(newOrganization.Id);


        }

        /// <summary>
        /// update exist client async
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> UpdateOrganizationAsync(OrganizationViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();


            var organization = await _organizationContext.Organizations
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (organization == null)
                return new NotFoundResultModel<Guid>();

            organization.Name = model.Name;
            organization.Brand = model.Brand;
            organization.ClientType = model.ClientType;
            organization.Email = model.Email;
            organization.Phone = model.Phone;
            organization.WebSite = model.WebSite;
            organization.FiscalCode = model.FiscalCode;
            organization.IBANCode = model.IBANCode;
            organization.Bank = model.Bank;
            organization.EmployeeId = model.EmployeeId;
            organization.IndustryId = model.IndustryId;
            organization.Description = model.Description;
            organization.CodSwift = model.CodSwift;
            organization.VitCode = model.VitCode;

            _organizationContext.Organizations.Update(organization);
            var result = await _organizationContext.PushAsync();

            return new ResultModel<Guid> { IsSuccess = result.IsSuccess, Errors = result.Errors, Result = organization.Id };
        }

        /// <summary>
        /// Migration organization to type
        /// </summary>
        /// <param name="prospectId"></param>
        /// /// <param name="type"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> MigrationOrganizationToTypeAsync(Guid? prospectId, OrganizationType type)
        {
            if (prospectId == null)
                return new InvalidParametersResultModel();

            var prospect = await _organizationContext.Organizations
                .FirstOrDefaultAsync(x => x.ClientType == OrganizationType.Prospect && x.Id == prospectId);

            if (prospect == null)
                return new NotFoundResultModel();

            prospect.ClientType = type;

            _organizationContext.Organizations.Update(prospect);

            return await _organizationContext.PushAsync();
        }

        /// <summary>
        /// Get all active organizations by type 
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetOrganizationViewModel>>> GetAllActiveOrganizationsByTypeAsync(OrganizationType type, bool includeDeleted = false)
        {
            var listOrganizations = await _organizationContext.Organizations
                .Include(x => x.Contacts)
                .ThenInclude(x => x.ContactWebProfiles)
                .ThenInclude(x => x.WebProfile)
                .Include(i => i.Industry)
                .Where(x => x.ClientType == type && (!x.IsDeleted || includeDeleted))
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<GetOrganizationViewModel>>(
                _mapper.Map<IEnumerable<GetOrganizationViewModel>>(listOrganizations));
        }

       


        /// <summary>
        /// Import organization 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ImportOrganizationAsync(IFormFile formFile)
        {

            var listExcel = new List<ExcelReadOrganizationViewModel>();

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);

                IWorkbook workbook;

                if (formFile.FileName.IndexOf(".xlsx", StringComparison.Ordinal) > 0)
                    workbook = WorkbookFactory.Create(stream);
                else if (formFile.FileName.IndexOf(".xls", StringComparison.Ordinal) > 0)
                    workbook = new HSSFWorkbook(stream);
                else
                    return new ResultModel
                    {
                        IsSuccess = false,
                        Errors = new List<IErrorModel> { new ErrorModel { Message = "File is not in excel format" } }
                    };

                var sheet = workbook.GetSheetAt(0);


                for (var item = sheet.FirstRowNum + 1; item <= sheet.LastRowNum; item++)
                {
                    var row = sheet.GetRow(item);
                    if (row == null)
                        continue;

                    if (string.IsNullOrEmpty(row.GetCell(3).CellStringValue())
                        || string.IsNullOrEmpty(row.GetCell(5).CellStringValue())
                        || string.IsNullOrEmpty(row.GetCell(7).CellStringValue()))
                        continue;

                    listExcel.Add(new ExcelReadOrganizationViewModel
                    {
                        FiscalCode = row.GetCell(3).CellStringValue(),
                        IBAN = row.GetCell(4).CellStringValue(),
                        Name = row.GetCell(5).CellStringValue(),
                        Address = row.GetCell(6).CellStringValue(),
                        WorkCategory = row.GetCell(7).CellStringValue(),
                        Bank = row.GetCell(11).CellStringValue(),
                    });
                }
            }


            if (!listExcel.Any())
            {
                return new NotFoundResultModel();
            }

            await AddOrganizationFromExcelAsync(listExcel);

            return new ResultModel { IsSuccess = true };
        }


        /// <summary>
        /// Import organization from excel data 
        /// </summary>
        /// <param name="listOrganization"></param>
        /// <returns></returns>
        private async Task<ResultModel> AddOrganizationFromExcelAsync(IEnumerable<ExcelReadOrganizationViewModel> listOrganization)
        {

            if (listOrganization == null || !listOrganization.Any())
                return new InvalidParametersResultModel();


            var toResult = new ResultModel { IsSuccess = true };

            foreach (var organization in listOrganization)
            {
                var workCategory = new List<Guid>();

                var result = await AddNewOrganizationAsync(new OrganizationViewModel
                {
                    Bank = organization.Bank,
                    Name = organization.Name,
                    FiscalCode = organization.FiscalCode,
                    IBANCode = organization.IBAN,
                    Email = "Email@mail.com",
                    Phone = "00000000",
                    ClientType = OrganizationType.Client
                });


                //if (result.IsSuccess && organizationAddressRequest.IsSuccess)
                //{
                // await _addressService.AddOrganizationAddressAsync(organizationAddressRequest.Result);
                // }

            }
            return toResult;
        }


        #region Helper

        //private async Task<ResultModel<GeoPosition>> GetAdressByString(string fullAddress)
        //{
        //    if (string.IsNullOrEmpty(fullAddress))
        //        return new InvalidParametersResultModel<GeoPosition>();

        //    var cities = await _organizationContext.Cities.Include(s => s.Region).OrderByDescending(x => x.Name.Length).ToListAsync();
        //    var regions = await _organizationContext.Regions.ToListAsync();

        //    Guid? regionId = null;
        //    Guid? cityId = null;
        //    var street = "";

        //    var streetIndexes = new[] { "str.", "sos.", "bd." };
        //    var cityIndexes = new[] { "or.", "s." };
        //    var regionIndexes = new[] { "r-nul", "mun.", "r." };

        //    foreach (var streetIndex in streetIndexes)
        //    {
        //        if (!string.IsNullOrEmpty(street))
        //            break;

        //        var index = fullAddress.ToLower().IndexOf(streetIndex, StringComparison.Ordinal);

        //        if (index > -1)
        //        {
        //            street = fullAddress.Substring(index + streetIndex.Length);
        //            fullAddress = fullAddress.Substring(0, index);
        //        }
        //    }

        //    foreach (var regionIndex in regionIndexes)
        //    {
        //        if (regionId.HasValue)
        //            break;

        //        var index = fullAddress.ToLower().IndexOf(regionIndex, StringComparison.Ordinal);

        //        if (index > -1)
        //        {
        //            fullAddress = fullAddress.Replace("mun.", "Municipiul ");
        //            regionId = regions.FirstOrDefault(x => ContainsString(fullAddress, x.Name))?.Id;
        //        }
        //    }

        //    if (regionId.HasValue)
        //        cities = cities.Where(x => x.RegionId == regionId.Value).ToList();

        //    foreach (var cityIndex in cityIndexes)
        //    {
        //        if (cityId.HasValue)
        //            break;

        //        var index = fullAddress.ToLower().IndexOf(cityIndex, StringComparison.Ordinal);

        //        if (index > -1)
        //        {
        //            var cityAdress = fullAddress.Substring(index + cityIndex.Length);
        //            cityId = cities.FirstOrDefault(x => ContainsString(cityAdress, x.Name))?.Id;
        //        }
        //    }

        //    if (!cityId.HasValue)
        //    {
        //        cityId = cities.FirstOrDefault(x => ContainsString(fullAddress, x.Name))?.Id;
        //    }


        //    if (regionId.HasValue)
        //        return new SuccessResultModel<GeoPosition>(regions.FirstOrDefault(x => x.Id == regionId)?.GeoPosition ?? GeoPosition.BackOffice);

        //    if (cityId.HasValue)
        //        return new SuccessResultModel<GeoPosition>(cities.FirstOrDefault(x => x.Id == cityId)?.Region?.GeoPosition ?? GeoPosition.BackOffice);


        //    return new NotFoundResultModel<GeoPosition>();
        //}


        private bool ContainsString(string initial, string contains)
        {

            initial = initial.ToLower();
            contains = contains.ToLower();

            var replaceList = new Dictionary<string, string>() {
                {"ă", "a"},
                {"î", "i"},
                {"ș", "s"},
                {"ț", "t"},
                {"â", "a"}
            };


            foreach (var replace in replaceList)
            {
                initial = initial.Replace(replace.Key, replace.Value);
                contains = contains.Replace(replace.Key, replace.Value);
            }

            return initial.Contains(contains);
        }


        #endregion
    }
}
