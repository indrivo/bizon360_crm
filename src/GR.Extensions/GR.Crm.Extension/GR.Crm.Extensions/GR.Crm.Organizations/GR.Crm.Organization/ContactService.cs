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
using GR.Crm.Organizations.Abstractions.ViewModels.ContactsViewModels;
using GR.Crm.Organizations.Abstractions.ViewModels.WebProfilesViewModels;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GR.Crm.Organizations
{
    public class ContactService : ICrmContactService
    {

        #region Injectable

        /// <summary>
        /// Inject context
        /// </summary>
        private readonly ICrmOrganizationContext _organizationContext;

        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;

        #endregion


        public ContactService(ICrmOrganizationContext organizationContext, IMapper mapper)
        {
            _organizationContext = organizationContext;
            _mapper = mapper;
        }


        /// <summary>
        /// Get contact by id
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetContactViewModel>> GetContactByIdAsync(Guid? contactId)
        {
            if (contactId == null)
                return new InvalidParametersResultModel<GetContactViewModel>();


            var contact = await _organizationContext.Contacts
                .Include(i => i.ContactWebProfiles)
                .ThenInclude(i => i.WebProfile)
                .Include(i=> i.Organization)
                .Include(i=> i.JobPosition)
                .FirstOrDefaultAsync(x => x.Id == contactId);

            if (contact == null)
                return new NotFoundResultModel<GetContactViewModel>();

            return new SuccessResultModel<GetContactViewModel>(contact.Adapt<GetContactViewModel>());

        }

        /// <summary>
        /// get contacts by organizationId
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetContactViewModel>>> GetContactsByOrganizationIdAsync(Guid? organizationId, bool includeDeleted = false)
        {
            if (organizationId == null)
                return new InvalidParametersResultModel<IEnumerable<GetContactViewModel>>();

            var listContacts = await _organizationContext.Contacts
                .Include(i => i.ContactWebProfiles)
                .ThenInclude(i => i.WebProfile)
                .Include(i => i.Organization)
                .Include(i => i.JobPosition)
                .Where(x => x.OrganizationId == organizationId && (!x.IsDeleted || includeDeleted))
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<GetContactViewModel>>(listContacts.Adapt<IEnumerable<GetContactViewModel>>());
        }

        /// <summary>
        /// Get all contacts paginated
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<PaginateContactViewModel>>> GetAllContactsPaginatedAsync(PageRequest request)
        {
            var query = await  _organizationContext.Contacts
                .Include(i=> i.JobPosition)
                .Include(i=> i.Organization)
                .Where(x => !x.IsDeleted || request.IncludeDeleted)
                .Select(s=> new PaginateContactViewModel
                {
                    Id = s.Id,
                    Created = s.Created,
                    JobPosition = s.JobPosition.Name,
                    Organization = s.Organization.Name,
                    Email = s.Email,
                    LastName = s.LastName,
                    FirstName = s.FirstName,
                    Phone =  s.Phone,
                    IsDeleted = s.IsDeleted,
                }).GetPagedAsync(request);

            return new SuccessResultModel<PagedResult<PaginateContactViewModel>>(query.Map(query).Result);
        }

        /// <summary>
        /// Get all contacts
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetContactViewModel>>> GetAllContactsAsync(bool includeDeleted = false)
        {
            var listContacts = await _organizationContext.Contacts
                .Include(i => i.ContactWebProfiles)
                .ThenInclude(i => i.WebProfile)
                .Include(i => i.Organization)
                .Include(i => i.JobPosition)
                .Where(x=> !x.IsDeleted || includeDeleted)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<GetContactViewModel>>(listContacts.Adapt<IEnumerable<GetContactViewModel>>());
        }

        /// <summary>
        /// Add new contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddNewContactAsync(ContactViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();


            var existContact = await _organizationContext.Contacts.FirstOrDefaultAsync(x =>
                x.FirstName.Trim().ToLower().Equals(model.FirstName.Trim().ToLower())
                && x.LastName.Trim().ToLower().Equals(model.LastName.Trim().ToLower())
                && x.Email.Trim().ToLower().Equals(model.Email.Trim().ToLower())
                && x.OrganizationId.Equals(model.OrganizationId));

            if(existContact != null)
                return new ResultModel<Guid>{IsSuccess = false, Errors = new List<IErrorModel>{new ErrorModel{Message = "Contact "+ model.FirstName + " " + model.LastName + " exist"}}};


            var newContact = new Contact
            {
                OrganizationId = model.OrganizationId,
                JobPositionId = model.JobPositionId,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.Phone,
                Description = model.Description,
            };

            await _organizationContext.Contacts.AddAsync(newContact);
            var result = await _organizationContext.PushAsync();

            return new ResultModel<Guid>{IsSuccess = result.IsSuccess, Errors = result.Errors, Result =  newContact.Id};

        }

        /// <summary>
        /// Update contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> UpdateContactAsync(ContactViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var contact = await _organizationContext.Contacts
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (contact == null)
                return new NotFoundResultModel<Guid>();

            contact.Email = model.Email;
            contact.FirstName = model.FirstName;
            contact.LastName = model.LastName;
            contact.JobPositionId = model.JobPositionId;
            contact.OrganizationId = model.OrganizationId;
            contact.Phone = model.Phone;
            contact.Description = model.Description;

            _organizationContext.Contacts.Update(contact);
            var result = await _organizationContext.PushAsync();

            return new ResultModel<Guid>
                {IsSuccess = result.IsSuccess, Errors = result.Errors, Result = contact.Id};
        }

        /// <summary>
        /// Delete permanently contact async
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteContactAsync(Guid? contactId)
        {
            if (contactId == null)
                return new InvalidParametersResultModel();

            var contact = await _organizationContext.Contacts.FirstOrDefaultAsync(x => x.Id == contactId);

            if (contact == null)
                return new NotFoundResultModel();

            _organizationContext.Contacts.Remove(contact);
            return await _organizationContext.PushAsync();

        }

        /// <summary>
        /// Deactivate contact by id 
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeactivateContactAsync(Guid? contactId) =>
            await _organizationContext.DisableRecordAsync<Contact>(contactId);

        /// <summary>
        /// Activate contact by id
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivateContactAsync(Guid? contactId) =>
            await _organizationContext.ActivateRecordAsync<Contact>(contactId);

        /// <summary>
        /// Get web profile by id
        /// </summary>
        /// <param name="webProfileId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetWebProfileViewModel>> GetWebProfileByIdAsync(Guid? webProfileId)
        {
            if (webProfileId == null)
                return new InvalidParametersResultModel<GetWebProfileViewModel>();

            var webProfile = await _organizationContext.WebProfiles
                .FirstOrDefaultAsync(x => x.Id == webProfileId);

            if (webProfile == null)
                return new NotFoundResultModel<GetWebProfileViewModel>();

            var getWebProfile = new GetWebProfileViewModel
            {
                ProviderName = webProfile.ProviderName,
                Url = webProfile.Url,
                IconUrl = webProfile.Id.ToString()
            };

            return new SuccessResultModel<GetWebProfileViewModel>(getWebProfile);

        }

        /// <summary>
        /// Get image byte array
        /// </summary>
        /// <param name="webProfileId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<WebProfileImageViewModel>> GetImageByteArrayAsync(Guid? webProfileId)
        {
            if (webProfileId == null)
                return new InvalidParametersResultModel<WebProfileImageViewModel>();

            var webProfile = await _organizationContext.WebProfiles
                .FirstOrDefaultAsync(x => x.Id == webProfileId);

            if (webProfile == null)
                return new NotFoundResultModel<WebProfileImageViewModel>();

            return new SuccessResultModel<WebProfileImageViewModel>(
                new WebProfileImageViewModel
                {
                    Name = webProfile.IconName,
                    ListByte = webProfile.Icon
                });
        }

        /// <summary>
        /// Get all web  profile
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetWebProfileViewModel>>> GetAllProfileAsync()
        {

            var webProfile = await _organizationContext.WebProfiles
                .ToListAsync();


            return new SuccessResultModel<IEnumerable<GetWebProfileViewModel>>(webProfile.Adapt<IEnumerable<GetWebProfileViewModel>>());

        }

        /// <summary>
        /// Add new web profile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> AddWebProfileAsync(WebProfileViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var webProfile = new WebProfile
            {
                ProviderName = model.ProviderName,
                Url = model.ProviderName,
                Icon = await GetBytesAsync(model.Icon),
                IconName = model.Icon.FileName
            };

            await _organizationContext.WebProfiles.AddAsync(webProfile);
            var result = await _organizationContext.PushAsync();
            result.Result = webProfile.Id;

            return result;
        }

        /// <summary>
        /// Update web profile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateWebProfileAsync(WebProfileViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var webProfile = await _organizationContext.WebProfiles
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            webProfile.ProviderName = model.ProviderName;
            webProfile.Url = model.Url;
            if (model.Icon != null)
            {
                webProfile.Icon = await GetBytesAsync(model.Icon);
                webProfile.IconName = model.Icon.FileName;
            }

            _organizationContext.WebProfiles.Update(webProfile);
            return await _organizationContext.PushAsync();
        }

        /// <summary>
        /// Delete web profile
        /// </summary>
        /// <param name="webProfileId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteWebProfileAsync(Guid? webProfileId)
        {
            if (webProfileId == null)
                return new InvalidParametersResultModel();

            var webProfile = await _organizationContext.WebProfiles
                .FirstOrDefaultAsync(x => x.Id == webProfileId);

            if (webProfile == null)
                return new NotFoundResultModel();


            _organizationContext.WebProfiles.Remove(webProfile);
            return await _organizationContext.PushAsync();
        }

        /// <summary>
        /// Transform IFromFile to byte array
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        private static async Task<byte[]> GetBytesAsync(IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Get contactWebProfile by id
        /// </summary>
        /// <param name="contactWebProfileId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<ContactWebProfileViewModel>> GetContactWebProfileByIdAsync(Guid? contactWebProfileId)
        {
            if (contactWebProfileId == null)
                return new InvalidParametersResultModel<ContactWebProfileViewModel>();

            var contactWebProfile = await _organizationContext.ContactWebProfiles
                .Include(x => x.WebProfile)
                .FirstOrDefaultAsync(x => x.Id == contactWebProfileId);

            if (contactWebProfile == null)
                return new NotFoundResultModel<ContactWebProfileViewModel>();

            return new SuccessResultModel<ContactWebProfileViewModel>(contactWebProfile.Adapt<ContactWebProfileViewModel>());
        }

        /// <summary>
        /// Get contactWebProfile by contact id
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<ContactWebProfileViewModel>> GetContactWebProfileByContactIdAsync(Guid? contactId)
        {
            if (contactId == null)
                return new InvalidParametersResultModel<ContactWebProfileViewModel>();

            var contactWebProfile = await _organizationContext.ContactWebProfiles
                .Include(x => x.WebProfile)
                .FirstOrDefaultAsync(x => x.ContactId == contactId);

            if (contactWebProfile == null)
                return new NotFoundResultModel<ContactWebProfileViewModel>();

            return new SuccessResultModel<ContactWebProfileViewModel>(contactWebProfile.Adapt<ContactWebProfileViewModel>());
        }

        /// <summary>
        /// Get all contactWebProfile
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<ContactWebProfileViewModel>>> GetAllContactWebProfileAsync()
        {
            var listContactWebProfile = await _organizationContext.ContactWebProfiles
                .Include(x => x.WebProfile)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<ContactWebProfileViewModel>>(listContactWebProfile.Adapt<IEnumerable<ContactWebProfileViewModel>>());
        }

        /// <summary>
        /// Add new contactWebProfile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> AddContactWebProfileAsync(ContactWebProfileViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var newContactWebProfile = new ContactWebProfile
            {
                WebProfileId = model.WebProfileId,
                UserName = model.UserName,
                Url = model.Url,
                ContactId = model.ContactId
            };

            await _organizationContext.ContactWebProfiles.AddAsync(newContactWebProfile);
            var result = await _organizationContext.PushAsync();
            result.Result = newContactWebProfile.Id;

            return result;
        }

        /// <summary>
        /// Update contactWebProfile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateContactWebProfileAsync(ContactWebProfileViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var contactWebProfile = await _organizationContext.ContactWebProfiles
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            contactWebProfile.UserName = model.UserName;
            contactWebProfile.Url = model.Url;
            contactWebProfile.ContactId = model.ContactId;
            contactWebProfile.WebProfileId = model.ContactId;

            _organizationContext.Update(contactWebProfile);

            return await _organizationContext.PushAsync();
        }

        /// <summary>
        /// Delete contactWebProfile
        /// </summary>
        /// <param name="contactWebProfileId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteContactWebProfileAsync(Guid? contactWebProfileId)
        {
            if (contactWebProfileId == null)
                return new InvalidParametersResultModel();

            var contactWebProfile = await _organizationContext.ContactWebProfiles
                .FirstOrDefaultAsync(x => x.Id == contactWebProfileId);

            if (contactWebProfile == null)
                return new NotFoundResultModel();

            _organizationContext.ContactWebProfiles.Remove(contactWebProfile);

            return await _organizationContext.PushAsync();
        }

        /// <summary>
        /// Disable contact webProfile
        /// </summary>
        /// <param name="contactWebProfileId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisableContactWebProfileAsync(Guid? contactWebProfileId) =>
            await _organizationContext.DisableRecordAsync<ContactWebProfile>(contactWebProfileId);

        /// <summary>
        /// Activate contact webProfile
        /// </summary>
        /// <param name="contactWebProfileId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivateContactWebProfileAsync(Guid? contactWebProfileId) =>
            await _organizationContext.ActivateRecordAsync<ContactWebProfile>(contactWebProfileId);

    }
}
