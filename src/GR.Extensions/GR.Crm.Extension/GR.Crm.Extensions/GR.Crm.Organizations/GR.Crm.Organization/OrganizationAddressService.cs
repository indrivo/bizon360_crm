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
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.ViewModels.OrganizationAddressViewModels;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace GR.Crm.Organizations
{
    public class OrganizationAddressService : IOrganizationAddressService
    {

        #region Injectable

        /// <summary>
        /// Context
        /// </summary>
        private ICrmOrganizationContext _context;

        /// <summary>
        /// Mapper inject
        /// </summary>
        private IMapper _mapper;


        #endregion

        public OrganizationAddressService(ICrmOrganizationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get organization address by id
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetOrganizationAddressViewModel>> GetAddressByIdAsync(Guid? addressId)
        {
            if (addressId == null)
                return new InvalidParametersResultModel<GetOrganizationAddressViewModel>();

            var address = await _context.OrganizationAddresses
                .Include(i => i.Organization)
                .Include(i => i.City)
                .ThenInclude(i => i.Region)
                .FirstOrDefaultAsync(x => x.Id == addressId);

            if (address == null)
                return new NotFoundResultModel<GetOrganizationAddressViewModel>();

            return new SuccessResultModel<GetOrganizationAddressViewModel>(_mapper.Map<GetOrganizationAddressViewModel>(address));
        }

        /// <summary>
        /// Get all organization address
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetOrganizationAddressViewModel>>> GetAllAddressesAsync(bool includeDeleted = false)
        {
            var addressesBd = await _context.OrganizationAddresses
                .Include(i => i.Organization)
                .Include(i => i.City)
                .ThenInclude(i => i.Region)
                .Where(x => !x.IsDeleted || includeDeleted)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<GetOrganizationAddressViewModel>>(_mapper.Map<List<GetOrganizationAddressViewModel>>(addressesBd));
        }

        /// <summary>
        /// Get organization addresses by organization id
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetOrganizationAddressViewModel>>> GetAddressesByOrganizationIdAsync(Guid? organizationId, bool includeDeleted = false)
        {
            if (organizationId == null)
                return new InvalidParametersResultModel<IEnumerable<GetOrganizationAddressViewModel>>();

            var addressesBd = await _context.OrganizationAddresses
                .Include(i => i.Organization)
                .Include(i => i.City)
                .ThenInclude(i => i.Region)
                .Where(x => x.OrganizationId == organizationId && (!x.IsDeleted || includeDeleted))
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<GetOrganizationAddressViewModel>>(_mapper.Map<IEnumerable<GetOrganizationAddressViewModel>>(addressesBd));
        }


        /// <summary>
        /// Add organization address
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddOrganizationAddressAsync(OrganizationAddressViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var organizationAddress = _mapper.Map<OrganizationAddress>(model);

            //    new OrganizationAddress
            //{
            //    OrganizationId = model.OrganizationId,
            //    CityId = model.CityId,
            //    Street = model.Street,
            //    Zip = model.Zip
            //};

            _context.OrganizationAddresses.Add(organizationAddress);
            var result = await _context.PushAsync();

            return new ResultModel<Guid> { IsSuccess = result.IsSuccess, Errors = result.Errors, Result = organizationAddress.Id };
        }

        /// <summary>
        /// Update organization address
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> UpdateOrganizationAddressAsync(OrganizationAddressViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var organization = await _context.OrganizationAddresses.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (organization == null)
                return new NotFoundResultModel<Guid>();

            organization.OrganizationId = model.OrganizationId;
            organization.CityId = model.CityId;
            organization.Street = model.Street;
            organization.Zip = model.Zip;

            _context.OrganizationAddresses.Update(organization);
            var result = await _context.PushAsync();
            return new ResultModel<Guid> { IsSuccess = result.IsSuccess, Errors = result.Errors, Result = organization.Id };
        }

        /// <summary>
        /// Delete organization address by id
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteOrganizationAddressAsync(Guid? addressId)
        {
            if (addressId == null)
                return new InvalidParametersResultModel();

            var address = await _context.OrganizationAddresses.FirstOrDefaultAsync(x => x.Id == addressId);

            if (address == null)
                return new NotFoundResultModel();

            _context.OrganizationAddresses.Remove(address);
            return await _context.PushAsync();
        }

        /// <summary>
        /// Disable organization address by id
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisableAddressByIdAsync(Guid? addressId) =>
            await _context.DisableRecordAsync<OrganizationAddress>(addressId);


        /// <summary>
        /// Activate organization address by id
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivateAddressByIdAsync(Guid? addressId) =>
            await _context.ActivateRecordAsync<OrganizationAddress>(addressId);



        #region City



        /// <summary>
        /// Get all paginated cities
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<CityViewModel>>> GetAllPaginatedCitiesAsync(PageRequest request)
        {
            var cities = await _context.Cities
                .Include(x => x.Region)
                .Where(x => !x.IsDeleted || request.IncludeDeleted)
                .OrderBy(o => o.Name)
                .GetPagedAsync(request);


            var map = cities.Map(_mapper.Map<IEnumerable<CityViewModel>>(cities.Result));

            return new SuccessResultModel<PagedResult<CityViewModel>>(map);
        }

        /// <summary>
        /// Get all cities
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<CityViewModel>>> GetAllCitiesAsync()
        {
            var cities = await _context.Cities
                .Include(x => x.Region)
                .OrderBy(o => o.Name)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<CityViewModel>>(cities.Adapt<IEnumerable<CityViewModel>>());
        }

        /// <summary>
        /// Get cities by region id
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<CityViewModel>>> GetAllCitiesByRegionIdAsync(Guid? regionId)
        {
            if (regionId == null)
                return new InvalidParametersResultModel<IEnumerable<CityViewModel>>();

            var cities = await _context.Cities
                .Include(x => x.Region)
                .Where(x => x.RegionId == regionId)
                .OrderBy(o => o.Name)
                .ToListAsync();


            return new SuccessResultModel<IEnumerable<CityViewModel>>(cities.Adapt<IEnumerable<CityViewModel>>());
        }


        /// <summary>
        /// Get cities by  id
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<CityViewModel>> GetCityByIdAsync(Guid? cityId)
        {
            if (cityId == null)
                return new InvalidParametersResultModel<CityViewModel>();

            var city = await _context.Cities
                .Include(x => x.Region)
                .FirstOrDefaultAsync(x => x.Id == cityId);

            if (city == null)
                return new NotFoundResultModel<CityViewModel>();


            return new SuccessResultModel<CityViewModel>(_mapper.Map<CityViewModel>(city));
        }

        /// <summary>
        /// Add city
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddCityAsync(AddCityViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var newCity = new City
            {
                Name = model.Name,
                RegionId = model.RegionId
            };

            _context.Cities.Add(newCity);
            var result = await _context.PushAsync();

            return new ResultModel<Guid> { IsSuccess = result.IsSuccess, Errors = result.Errors, Result = newCity.Id };
        }


        /// <summary>
        /// Update city
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateCityAsync(AddCityViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var city = await _context.Cities.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (city == null)
                return new NotFoundResultModel();

            city.Name = model.Name;
            city.RegionId = model.RegionId;

            _context.Cities.Update(city);
            return await _context.PushAsync();
        }

        /// <summary>
        /// Delete city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteCityByIdAsync(Guid? cityId)
        {
            if (cityId == null)
                return new InvalidParametersResultModel();

            var city = await _context.Cities
                .FirstOrDefaultAsync(x => x.Id == cityId);

            if (city == null)
                return new NotFoundResultModel();

            _context.Cities.Remove(city);
            return await _context.PushAsync();
        }


        /// <summary>
        /// Disable city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisableCityAsync(Guid? cityId) =>
            await _context.DisableRecordAsync<City>(cityId);

        /// <summary>
        /// Activate city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivateCityAsync(Guid? cityId) =>
            await _context.ActivateRecordAsync<City>(cityId);


        #endregion


        #region Regions



        /// <summary>
        /// Get region by  id
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<RegionViewModel>> GetRegionByIdAsync(Guid? regionId)
        {
            if (regionId == null)
                return new InvalidParametersResultModel<RegionViewModel>();

            var region = await _context.Regions
                .Include(i => i.Country)
                .FirstOrDefaultAsync(x => x.Id == regionId);

            if (region == null)
                return new NotFoundResultModel<RegionViewModel>();

            return new SuccessResultModel<RegionViewModel>(_mapper.Map<RegionViewModel>(region));
        }

        /// <summary>
        /// Get all paginated regions
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<RegionViewModel>>> GetAllPaginatedRegionsAsync(PageRequest request)
        {
            var regions = await _context.Regions
                .Include(i => i.Cities)
                .Include(i => i.Country)
                .Where(x => !x.IsDeleted || request.IncludeDeleted)
                .OrderBy(o => o.Name)
                .GetPagedAsync(request);


            var map = regions.Map(_mapper.Map<IEnumerable<RegionViewModel>>(regions.Result));

            return new SuccessResultModel<PagedResult<RegionViewModel>>(map);
        }

        /// <summary>
        /// Get all regions
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<RegionViewModel>>> GetAllRegionsAsync()
        {
            var regions = await _context.Regions
                .Include(i => i.Cities)
                .Include(i => i.Country)
                .OrderBy(o => o.Name)
                .ToListAsync();
            return new SuccessResultModel<IEnumerable<RegionViewModel>>(regions.Adapt<IEnumerable<RegionViewModel>>());
        }

        /// <summary>
        /// Add region
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddRegionAsync(RegionViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var region = new Region
            {
                Name = model.Name,
                CountryId = model.CountryId
            };

            await _context.Regions.AddAsync(region);
            var result = await _context.PushAsync();

            return new ResultModel<Guid> { IsSuccess = result.IsSuccess, Errors = result.Errors, Result = region.Id };

        }

        /// <summary>
        /// Update region
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateRegionAsync(RegionViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();


            var region = await _context.Regions.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (region == null)
                return new NotFoundResultModel();

            region.Name = model.Name;
            region.CountryId = model.CountryId;

            _context.Regions.Update(region);
            return await _context.PushAsync();
        }

        /// <summary>
        /// Delete region
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteRegionByIdAsync(Guid? regionId)
        {
            if (regionId == null)
                return new InvalidParametersResultModel();

            var region = await _context.Regions
                .FirstOrDefaultAsync(x => x.Id == regionId);

            if (region == null)
                return new NotFoundResultModel();

            _context.Regions.Remove(region);
            return await _context.PushAsync();
        }

        /// <summary>
        /// Disable region
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisableRegionAsync(Guid? regionId) =>
            await _context.DisableRecordAsync<Region>(regionId);

        /// <summary>
        /// Activate region
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivateRegionAsync(Guid? regionId) =>
            await _context.ActivateRecordAsync<Region>(regionId);


        #endregion

        #region Country

        /// <summary>
        /// Get country by  id
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<CountryViewModel>> GetCountryByIdAsync(Guid? countryId)
        {
            if (countryId == null)
                return new InvalidParametersResultModel<CountryViewModel>();

            var country = await _context.Countries
                .FirstOrDefaultAsync(x => x.Id == countryId);

            if (country == null)
                return new NotFoundResultModel<CountryViewModel>();

            return new SuccessResultModel<CountryViewModel>(_mapper.Map<CountryViewModel>(country));
        }

        /// <summary>
        /// Get all paginated countries
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<CountryViewModel>>> GetAllPaginatedCountriesAsync(PageRequest request)
        {
            var countries = await _context.Countries
                .Where(x => !x.IsDeleted || request.IncludeDeleted)
                .OrderBy(o => o.Name)
                .GetPagedAsync(request);


            var map = countries.Map(_mapper.Map<IEnumerable<CountryViewModel>>(countries.Result));

            return new SuccessResultModel<PagedResult<CountryViewModel>>(map);
        }

        /// <summary>
        /// Get all countries
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<CountryViewModel>>> GetAllCountriesAsync(bool includeDeleted = false)
        {
            var countries = await _context.Countries
                .OrderBy(o => o.Name)
                .Where(x=> !x.IsDeleted || includeDeleted)
                .ToListAsync();
            return new SuccessResultModel<IEnumerable<CountryViewModel>>(_mapper.Map<IEnumerable<CountryViewModel>>(countries));
        }

        /// <summary>
        /// Add county
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddCountryAsync(CountryViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var country = new CrmCountry
            {
                Name = model.Name
            };

            await _context.Countries.AddAsync(country);
            var result = await _context.PushAsync();

            return result.Map(country.Id);

        }

        /// <summary>
        /// Update country
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateCountryAsync(CountryViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var country = await _context.Countries.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (country == null)
                return new NotFoundResultModel();

            country.Name = model.Name;

            _context.Countries.Update(country);
            return await _context.PushAsync();
        }

        /// <summary>
        /// Delete country
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteCountryByIdAsync(Guid? countryId)
        {
            if (countryId == null)
                return new InvalidParametersResultModel();

            var country = await _context.Countries
                .FirstOrDefaultAsync(x => x.Id == countryId);

            if (country == null)
                return new NotFoundResultModel();

            _context.Countries.Remove(country);
            return await _context.PushAsync();
        }

        /// <summary>
        /// Disable country
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisableCountryAsync(Guid? countryId) =>
            await _context.DisableRecordAsync<CrmCountry>(countryId);

        /// <summary>
        /// Activate country
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivateCountryAsync(Guid? countryId) =>
            await _context.ActivateRecordAsync<CrmCountry>(countryId);


        #endregion


        /// <summary>
        /// Seed team role 
        /// </summary>
        /// <returns></returns>
        public virtual async Task SeedRegions()
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Configuration/Regions.json");
            var items = JsonParser.ReadArrayDataFromJsonFile<ICollection<SeedRegions>>(path).ToList();

            if (items == null && !items.Any())
                return;

            foreach (var region in items)
            {

                var newRegion = new Region
                {
                    Name = region.Name
                };

                await _context.Regions.AddAsync(newRegion);

                foreach (var city in region.Cities)
                {
                    var newCity = new City
                    {
                        Name = city.Name,
                        RegionId = newRegion.Id,
                    };

                    await _context.Cities.AddAsync(newCity);
                }

            }

            var result = await _context.PushAsync();
        }

    }
}
