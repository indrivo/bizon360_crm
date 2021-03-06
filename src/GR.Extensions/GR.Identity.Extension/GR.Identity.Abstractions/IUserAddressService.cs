﻿using GR.Core.Helpers;
using GR.Identity.Abstractions.Models.AddressModels;
using GR.Identity.Abstractions.ViewModels.UserProfileAddress;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GR.Identity.Abstractions
{
    public interface IUserAddressService
    {
        /// <summary>
        /// Get addresses for current user
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<Address>>> GetUserAddressesAsync();

        /// <summary>
        /// Get addresses for user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<Address>>> GetUserAddressesAsync(Guid? userId);

        /// <summary>
        /// Get all countries
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<Country>>> GetCountriesAsync();

        /// <summary>
        /// Get country by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResultModel<Country>> GetCountryByIdAsync(string id);

        /// <summary>
        /// Get all cities for country
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<StateOrProvince>>> GetCitiesByCountryIdAsync(string countryId);

        /// <summary>
        /// Get city by id
        /// </summary> 
        /// <param name="cityId"></param>
        /// <returns></returns>
       Task<ResultModel<StateOrProvince>> GetCityByIdAsync(int? cityId);

        /// <summary>
        /// Delete address
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteAddressAsync(Guid? id);

        /// <summary>
        /// Add new address for current user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddAddressAsync(AddNewAddressViewModel model);

        /// <summary>
        /// Get address by id
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        Task<ResultModel<Address>> GetAddressByIdAsync(Guid? addressId);


        /// <summary>
        /// Get district by id
        /// </summary>
        /// <param name="districtId"></param>
        /// <returns></returns>
        Task<ResultModel<District>> GetDistrictByIdAsync(Guid? districtId);
    }
}