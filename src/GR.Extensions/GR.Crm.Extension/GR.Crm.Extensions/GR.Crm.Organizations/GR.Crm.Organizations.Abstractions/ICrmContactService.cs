using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Organizations.Abstractions.ViewModels.ContactsViewModels;
using GR.Crm.Organizations.Abstractions.ViewModels.WebProfilesViewModels;

namespace GR.Crm.Organizations.Abstractions
{
    public interface ICrmContactService
    {
        
        /// <summary>
        /// Get all contacts by filter
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<PagedResult<PaginateContactViewModel>>> GetAllContactsPaginatedAsync(PageRequest request);


        /// <summary>
        /// Get all contacts
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetContactViewModel>>> GetAllContactsAsync(bool includeDeleted);


        /// <summary>
        /// Get contact by id
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        Task<ResultModel<GetContactViewModel>> GetContactByIdAsync(Guid? contactId);

        /// <summary>
        /// get contacts by organizationId
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetContactViewModel>>> GetContactsByOrganizationIdAsync(Guid? organizationId, bool includeDeleted);

        /// <summary>
        /// Add new contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddNewContactAsync(ContactViewModel model);

        /// <summary>
        /// Update contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> UpdateContactAsync(ContactViewModel model);


        /// <summary>
        /// Delete permanently contact async
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteContactAsync(Guid? contactId);

        /// <summary>
        /// Deactivate contact by id 
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        Task<ResultModel> DeactivateContactAsync(Guid? contactId);

        /// <summary>
        /// Activate contact by id
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateContactAsync(Guid? contactId);

        /// <summary>
        /// Get web profile by id
        /// </summary>
        /// <param name="webProfileId"></param>
        /// <returns></returns>
        Task<ResultModel<GetWebProfileViewModel>> GetWebProfileByIdAsync(Guid? webProfileId);

        /// <summary>
        /// Get image byte array
        /// </summary>
        /// <param name="webProfileId"></param>
        /// <returns></returns>
        Task<ResultModel<WebProfileImageViewModel>> GetImageByteArrayAsync(Guid? webProfileId);

        /// <summary>
        /// Get all web  profile
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetWebProfileViewModel>>> GetAllProfileAsync();


        /// <summary>
        /// Add new web profile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> AddWebProfileAsync(WebProfileViewModel model);


        /// <summary>
        /// Update web profile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateWebProfileAsync(WebProfileViewModel model);


        /// <summary>
        /// Delete web profile
        /// </summary>
        /// <param name="webProfileId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteWebProfileAsync(Guid? webProfileId);



        /// <summary>
        /// Get contactWebProfile by id
        /// </summary>
        /// <param name="contactWebProfileId"></param>
        /// <returns></returns>
        Task<ResultModel<ContactWebProfileViewModel>> GetContactWebProfileByIdAsync(Guid? contactWebProfileId);

        /// <summary>
        /// Get contactWebProfile by contact id
        /// </summary>
        /// <param name="contactWebProfileId"></param>
        /// <returns></returns>
        Task<ResultModel<ContactWebProfileViewModel>> GetContactWebProfileByContactIdAsync(Guid? contactId);

        /// <summary>
        /// Get all contactWebProfile
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<ContactWebProfileViewModel>>> GetAllContactWebProfileAsync();

        /// <summary>
        /// Add new contactWebProfile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> AddContactWebProfileAsync(ContactWebProfileViewModel model);

        /// <summary>
        /// Update contactWebProfile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateContactWebProfileAsync(ContactWebProfileViewModel model);

        /// <summary>
        /// Delete contactWebProfile
        /// </summary>
        /// <param name="contactWebProfileId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteContactWebProfileAsync(Guid? contactWebProfileId);

        /// <summary>
        /// Disable contact webProfile
        /// </summary>
        /// <param name="contactWebProfileId"></param>
        /// <returns></returns>
        Task<ResultModel> DisableContactWebProfileAsync(Guid? contactWebProfileId);

        /// <summary>
        /// Activate contact webProfile
        /// </summary>
        /// <param name="contactWebProfileId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateContactWebProfileAsync(Guid? contactWebProfileId);
    }
}
