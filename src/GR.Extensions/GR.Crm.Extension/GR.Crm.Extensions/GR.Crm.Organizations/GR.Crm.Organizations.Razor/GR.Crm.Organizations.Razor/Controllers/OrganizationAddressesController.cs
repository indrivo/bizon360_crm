using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GR.Core.BaseControllers;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Organizations.Abstractions;
using GR.Crm.Organizations.Abstractions.ViewModels.OrganizationAddressViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GR.Crm.Organizations.Razor.Controllers
{
    [Authorize]
    public class OrganizationAddressesController : BaseGearController
    {
        #region Injectable

        /// <summary>
        /// address service
        /// </summary>
        private readonly IOrganizationAddressService _addressService;

        #endregion


        public OrganizationAddressesController(IOrganizationAddressService addressService)
        {
            _addressService = addressService;
        }


        /// <summary>
        /// Get all organization addresses
        /// </summary>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<GetOrganizationAddressViewModel>>))]
        public async Task<JsonResult> GetAllAddresses(bool includeDeleted = false)
            => await JsonAsync(_addressService.GetAllAddressesAsync(includeDeleted), SerializerSettings);

        /// <summary>
        /// Get  organization addresses by id
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<GetOrganizationAddressViewModel>))]
        public async Task<JsonResult> GetAddressById([Required]Guid addressId)
            => await JsonAsync(_addressService.GetAddressByIdAsync(addressId), SerializerSettings);

        /// <summary>
        /// Get organization addresses by organization id
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="includeDeleted"></param>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<GetOrganizationAddressViewModel>>))]
        public async Task<JsonResult> GetAddressesByOrganizationId([Required]Guid organizationId, bool includeDeleted = false)
            => await JsonAsync(_addressService.GetAddressesByOrganizationIdAsync(organizationId, includeDeleted), SerializerSettings);

        /// <summary>
        /// Add organization addresses
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> AddOrganizationAddress([Required] OrganizationAddressViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_addressService.AddOrganizationAddressAsync(model), SerializerSettings);
        }

        /// <summary>
        /// Update organization addresses
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> UpdateOrganizationAddress([Required] OrganizationAddressViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_addressService.UpdateOrganizationAddressAsync(model), SerializerSettings);
        }


        /// <summary>
        /// Disable organization addresses by id
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(Task<ResultModel>))]
        public async Task<JsonResult> DisableAddressById([Required]Guid addressId)
            => await JsonAsync(_addressService.DisableAddressByIdAsync(addressId), SerializerSettings);


        /// <summary>
        /// Activate organization addresses by id
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(Task<ResultModel>))]
        public async Task<JsonResult> ActivateAddressById([Required]Guid addressId)
            => await JsonAsync(_addressService.ActivateAddressByIdAsync(addressId), SerializerSettings);


        /// <summary>
        /// Delete organization addresses by id
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(Task<ResultModel>))]
        public async Task<JsonResult> DeleteOrganizationAddress([Required]Guid addressId)
            => await JsonAsync(_addressService.DeleteOrganizationAddressAsync(addressId), SerializerSettings);



        /// <summary>
        /// Get all paginated cities
        /// </summary>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<CityViewModel>))]
        public async Task<JsonResult> GetCityById([Required] Guid cityId)
            => await JsonAsync(_addressService.GetCityByIdAsync(cityId), SerializerSettings);


        /// <summary>
        /// Get all paginated cities
        /// </summary>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<PagedResult<CityViewModel>>))]
        public async Task<JsonResult> GetAllPaginatedCities(PageRequest request)
            => await JsonAsync(_addressService.GetAllPaginatedCitiesAsync(request), SerializerSettings);

        /// <summary>
        /// Get all cities
        /// </summary>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<CityViewModel>>))]
        public async Task<JsonResult> GetAllCities()
            => await JsonAsync(_addressService.GetAllCitiesAsync(), SerializerSettings);

        /// <summary>
        /// Get cities by region id
        /// </summary>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<CityViewModel>>))]
        public async Task<JsonResult> GetAllCitiesByRegionId([Required]Guid regionId)
            => await JsonAsync(_addressService.GetAllCitiesByRegionIdAsync(regionId), SerializerSettings);


        /// <summary>
        /// Add city
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> AddCity([Required] AddCityViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_addressService.AddCityAsync(model), SerializerSettings);
        }


        /// <summary>
        /// Update city
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdateCity([Required] AddCityViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_addressService.UpdateCityAsync(model), SerializerSettings);
        }

        /// <summary>
        /// Delete city
        /// </summary>
        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> DeleteCityByI([Required]Guid cityId)
            => await JsonAsync(_addressService.DeleteCityByIdAsync(cityId), SerializerSettings);


        /// <summary>
        /// Disable city
        /// </summary>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> DisableCity([Required]Guid cityId)
            => await JsonAsync(_addressService.DisableCityAsync(cityId), SerializerSettings);

        /// <summary>
        /// Activate city
        /// </summary>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> ActivateCity([Required]Guid cityId)
            => await JsonAsync(_addressService.ActivateCityAsync(cityId), SerializerSettings);



        /// <summary>
        /// Get region by id
        /// </summary>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<RegionViewModel>))]
        public async Task<JsonResult> GetRegionById([Required]Guid regionId)
            => await JsonAsync(_addressService.GetRegionByIdAsync(regionId), SerializerSettings);

        /// <summary>
        /// Get all paginated regions
        /// </summary>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<PagedResult<RegionViewModel>>))]
        public async Task<JsonResult> GetAllPaginatedRegions(PageRequest request)
            => await JsonAsync(_addressService.GetAllPaginatedRegionsAsync(request), SerializerSettings);

        /// <summary>
        /// Get all regions
        /// </summary>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<RegionViewModel>>))]
        public async Task<JsonResult> GetAllRegions()
            => await JsonAsync(_addressService.GetAllRegionsAsync(), SerializerSettings);

        /// <summary>
        /// Add region
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> AddRegion([Required] RegionViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_addressService.AddRegionAsync(model), SerializerSettings);
        }


        /// <summary>
        /// Update region
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdateRegion([Required] RegionViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_addressService.UpdateRegionAsync(model), SerializerSettings);
        }

        /// <summary>
        /// Delete region
        /// </summary>
        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> DeleteRegionById([Required]Guid regionId)
            => await JsonAsync(_addressService.DeleteRegionByIdAsync(regionId), SerializerSettings);

        /// <summary>
        /// Disable region
        /// </summary>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> DisableRegion([Required]Guid regionId)
            => await JsonAsync(_addressService.DisableRegionAsync(regionId), SerializerSettings);

        /// <summary>
        /// Activate region
        /// </summary>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> ActivateRegion([Required]Guid regionId)
            => await JsonAsync(_addressService.ActivateRegionAsync(regionId), SerializerSettings);




        #region Country

        /// <summary>
        /// Get country by id
        /// </summary>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<CountryViewModel>))]
        public async Task<JsonResult> GetCountryById([Required] Guid countryId)
            => await JsonAsync(_addressService.GetCountryByIdAsync(countryId), SerializerSettings);

        /// <summary>
        /// Get all paginated countries
        /// </summary>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<PagedResult<CountryViewModel>>))]
        public async Task<JsonResult> GetAllPaginatedCountries(PageRequest request)
            => await JsonAsync(_addressService.GetAllPaginatedCountriesAsync(request), SerializerSettings);

        /// <summary>
        /// Get all countries
        /// </summary>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<CountryViewModel>>))]
        public async Task<JsonResult> GetAllCountries(bool includeDeleted = false)
            => await JsonAsync(_addressService.GetAllCountriesAsync(includeDeleted), SerializerSettings);

        /// <summary>
        /// Add country
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> AddCountry([Required] CountryViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_addressService.AddCountryAsync(model), SerializerSettings);
        }


        /// <summary>
        /// Update country
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdateCountry([Required] CountryViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_addressService.UpdateCountryAsync(model), SerializerSettings);
        }

        /// <summary>
        /// Delete country
        /// </summary>
        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> DeleteCountryById([Required] Guid countryId)
            => await JsonAsync(_addressService.DeleteCountryByIdAsync(countryId), SerializerSettings);

        /// <summary>
        /// Disable country
        /// </summary>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> DisableCountry([Required] Guid countryId)
            => await JsonAsync(_addressService.DisableCountryAsync(countryId), SerializerSettings);

        /// <summary>
        /// Activate country
        /// </summary>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> ActivateCountry([Required] Guid countryId)
            => await JsonAsync(_addressService.ActivateCountryAsync(countryId), SerializerSettings);

        #endregion
    }
}
