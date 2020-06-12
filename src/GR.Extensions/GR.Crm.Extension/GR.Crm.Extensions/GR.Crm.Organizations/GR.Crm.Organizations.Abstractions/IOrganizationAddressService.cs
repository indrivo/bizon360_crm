using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Organizations.Abstractions.ViewModels.OrganizationAddressViewModels;

namespace GR.Crm.Organizations.Abstractions
{
    public interface IOrganizationAddressService
    {

        /// <summary>
        /// Get organization address by id
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        Task<ResultModel<GetOrganizationAddressViewModel>> GetAddressByIdAsync(Guid? addressId);

        /// <summary>
        /// Get all organization address
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetOrganizationAddressViewModel>>> GetAllAddressesAsync(bool includeDeleted);

        /// <summary>
        /// Get organization addresses by organization id
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetOrganizationAddressViewModel>>> GetAddressesByOrganizationIdAsync(Guid? organizationId, bool includeDeleted);

        /// <summary>
        /// Add organization address
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddOrganizationAddressAsync(OrganizationAddressViewModel model);

        /// <summary>
        /// Update organization address
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> UpdateOrganizationAddressAsync(OrganizationAddressViewModel model);

        /// <summary>
        /// Delete organization address by id
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteOrganizationAddressAsync(Guid? addressId);

        /// <summary>
        /// Disable organization address by id
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        Task<ResultModel> DisableAddressByIdAsync(Guid? addressId);

        /// <summary>
        /// Activate organization address by id
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateAddressByIdAsync(Guid? addressId);


        /// <summary>
        /// Get cities by  id
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        Task<ResultModel<CityViewModel>> GetCityByIdAsync(Guid? cityId);

        /// <summary>
        /// Get all paginated cities
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<PagedResult<CityViewModel>>> GetAllPaginatedCitiesAsync(PageRequest request);


        /// <summary>
        /// Disable city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        Task<ResultModel> DisableCityAsync(Guid? cityId);

        /// <summary>
        /// Activate city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateCityAsync(Guid? cityId);


        /// <summary>
        /// Get all cities
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<CityViewModel>>> GetAllCitiesAsync();

        /// <summary>
        /// Get cities by region id
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<CityViewModel>>> GetAllCitiesByRegionIdAsync(Guid? regionId);

        /// <summary>
        /// Add city
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
       Task<ResultModel<Guid>> AddCityAsync(AddCityViewModel model);

        /// <summary>
        /// Update city
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateCityAsync(AddCityViewModel model);

        /// <summary>
        /// Delete city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteCityByIdAsync(Guid? cityId);


        /// <summary>
        /// Get region by  id
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        Task<ResultModel<RegionViewModel>> GetRegionByIdAsync(Guid? regionId);

        /// <summary>
        /// Get all paginated regions
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<PagedResult<RegionViewModel>>> GetAllPaginatedRegionsAsync(PageRequest request);
       

        /// <summary>
        /// Get all regions
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<RegionViewModel>>> GetAllRegionsAsync();

        /// <summary>
        /// Add region
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddRegionAsync(RegionViewModel model);

        /// <summary>
        /// Update region
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateRegionAsync(RegionViewModel model);

        /// <summary>
        /// Delete region
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
       Task<ResultModel> DeleteRegionByIdAsync(Guid? regionId);

        /// <summary>
        /// Disable region
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        Task<ResultModel> DisableRegionAsync(Guid? regionId);


        /// <summary>
        /// Activate region
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateRegionAsync(Guid? regionId);



        #region Country

        /// <summary>
        /// Get country by  id
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        Task<ResultModel<CountryViewModel>> GetCountryByIdAsync(Guid? countryId);

        /// <summary>
        /// Get all paginated countries
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<PagedResult<CountryViewModel>>> GetAllPaginatedCountriesAsync(PageRequest request);

        /// <summary>
        /// Get all countries
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<CountryViewModel>>> GetAllCountriesAsync(bool includeDeleted);


        /// <summary>
        /// Add county
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddCountryAsync(CountryViewModel model);

        /// <summary>
        /// Update country
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateCountryAsync(CountryViewModel model);

        /// <summary>
        /// Delete country
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteCountryByIdAsync(Guid? countryId);
        /// <summary>
        /// Disable country
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        Task<ResultModel> DisableCountryAsync(Guid? countryId);

        /// <summary>
        /// Activate country
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateCountryAsync(Guid? countryId);


        #endregion


        // Task SeedRegions();
    }
}
