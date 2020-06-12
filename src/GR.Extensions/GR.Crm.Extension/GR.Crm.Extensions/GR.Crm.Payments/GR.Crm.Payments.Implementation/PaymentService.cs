using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using AutoMapper;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Organizations.Abstractions;
using GR.Crm.Organizations.Abstractions.Enums;
using GR.Crm.Organizations.Abstractions.ViewModels.OrganizationsViewModels;
using GR.Crm.Payments.Abstractions;
using GR.Crm.Payments.Abstractions.Models;
using GR.Crm.Payments.Abstractions.ViewModels.PaymentDashboardViewModel;
using GR.Crm.Payments.Abstractions.ViewModels.PaymentsViewModels;
using GR.Crm.Payments.Abstractions.ViewModels.XmlReadViewModels;
using GR.Identity.Abstractions;
using GR.Notifications.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GR.Crm.Payments.Implementation
{
    public class PaymentService : IPaymentService
    {

        #region Injected

        /// <summary>
        /// Inject context
        /// </summary>
        private readonly IPaymentContext _context;

        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Inject organization service
        /// </summary>
        private readonly ICrmOrganizationService _organizationService;

        /// <summary>
        /// Inject product service
        /// </summary>
        private readonly IProductService _productService;

        /// <summary>
        /// Inject notification
        /// </summary>
        private readonly INotify<GearRole> _notifyService;


        #endregion


        public PaymentService(IPaymentContext context,
            IMapper mapper,
            ICrmOrganizationService organizationService,
            IProductService productService,
            INotify<GearRole> notifyService)
        {
            _context = context;
            _mapper = mapper;
            _organizationService = organizationService;
            _productService = productService;
            _notifyService = notifyService;
        }

        /// <summary>
        /// Get payment by id
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetPaymentViewModel>> GetPaymentByIdAsync(Guid? paymentId)
        {
            if (paymentId == null)
                return new InvalidParametersResultModel<GetPaymentViewModel>();

            var payment = await _context.PaymentMappers
                .Include(i => i.Payment)
                .Include(i => i.Organization)
                .Include(i => i.Product)
                .Include(i => i.PaymentCode)
                .FirstOrDefaultAsync(x => x.Id == paymentId);

            if (payment == null)
                return new NotFoundResultModel<GetPaymentViewModel>();

            return new SuccessResultModel<GetPaymentViewModel>(_mapper.Map<GetPaymentViewModel>(payment));
        }

        /// <summary>
        /// Get all payments
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetPaymentViewModel>>> GetAllPaymentsAsync(bool includeDeleted = false)
        {
            var payments = await _context.PaymentMappers
                .Include(i => i.Payment)
                .Include(i => i.Organization)
                .Include(i => i.Product)
                .Include(i => i.PaymentCode)
                .Where(x => !x.IsDeleted || includeDeleted)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<GetPaymentViewModel>>(_mapper.Map<IEnumerable<GetPaymentViewModel>>(payments));
        }

        /// <summary>
        /// Get all paginated payments
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<GetTablePaymentViewModel>>> GetAllPaginatedPaymentsAsync(PageRequest request)
        {
            var query = _context.PaymentMappers
                .Include(i => i.Payment)
                .Include(i => i.Organization)
                .Include(i => i.Product)
               .Include(i => i.PaymentCode)
                .Where(x => !x.IsDeleted || request.IncludeDeleted)
                .Select(s => new GetTablePaymentViewModel
                {
                    Payment = s.Payment,
                    Organization = s.Organization,
                    Created = s.Created,
                    Id = s.Id,
                    OrganizationId = s.OrganizationId,
                    Author = s.Author,
                    Product = s.Product,
                    IsDeleted = s.IsDeleted,
                    ProductId = s.ProductId,
                    Changed = s.Changed,
                    PaymentCode = s.PaymentCode,
                    PaymentCodeId = s.PaymentCodeId,
                    ModifiedBy = s.ModifiedBy,
                    TenantId = s.TenantId,
                    PaymentId = s.PaymentId,
                    Idno = s.Organization != null ? s.Organization.FiscalCode : "",
                    OrganizationName = s.Organization != null ? s.Organization.Name : "",
                    PaymentDate = s.Payment.DateTransaction
                }).SetPeriodByProperty(request.PageRequestFilters, "PaymentDate");


            request.PageRequestFilters = request.PageRequestFilters.Where(x =>
                    !string.Equals(x.Propriety.Trim(), "StartDate", StringComparison.CurrentCultureIgnoreCase)
                    && !string.Equals(x.Propriety.Trim(), "EndDate", StringComparison.CurrentCultureIgnoreCase));


            var pagedResult = await query.GetPagedAsync(request);

            
           // var map = pagedResult.Map(_mapper.Map<IEnumerable<GetPaymentViewModel>>(pagedResult.Result));
            return new SuccessResultModel<PagedResult<GetTablePaymentViewModel>>(pagedResult);
        }

        /// <summary>
        /// Add payment
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddPaymentAsync(AddPaymentViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var organizationRequest = await _organizationService.FindOrganizationByIdAsync(model.OrganizationId);
            if (!organizationRequest.IsSuccess)
                return new ResultModel<Guid> { IsSuccess = false, Errors = organizationRequest.Errors };

            //var productRequest = await _productService.GetProductByIdAsync(model.ProductId);
            //if (!productRequest.IsSuccess)
            //    return new ResultModel<Guid> { IsSuccess = false, Errors = productRequest.Errors };

            var payment = _mapper.Map<Payment>(model);
            await _context.Payments.AddAsync(payment);

            payment.FiscalCode = organizationRequest.Result.FiscalCode;

            var mappedPayment = new PaymentMapped
            {
                OrganizationId = model.OrganizationId,
                ProductId = model.ProductId,
                PaymentId = payment.Id,
                PaymentCodeId = model.PaymentCodeId,
            };

            await _context.PaymentMappers.AddAsync(mappedPayment);
            var addPaymentResult = await _context.PushAsync();


            return addPaymentResult.IsSuccess
                ? new SuccessResultModel<Guid>(mappedPayment.Id)
                : addPaymentResult.Map<Guid>();
        }

        /// <summary>
        /// update payment
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdatePaymentAsync(UpdatePaymentViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var payment = await _context.PaymentMappers.Include(i => i.Payment).FirstOrDefaultAsync(x => x.Id == model.Id);

            if (payment == null)
                return new NotFoundResultModel();

            var organizationRequest = await _organizationService.FindOrganizationByIdAsync(model.OrganizationId);
            if (!organizationRequest.IsSuccess)
                return new ResultModel { IsSuccess = false, Errors = organizationRequest.Errors };

            var productRequest = await _productService.GetProductByIdAsync(model.ProductId);
            if (!productRequest.IsSuccess)
                return new ResultModel { IsSuccess = false, Errors = productRequest.Errors };

            payment.Payment.BankAccount = productRequest.Result.BankAccount;
            payment.Payment.Currency = model.Currency;
            payment.Payment.DateTransaction = model.DateTransaction;
            payment.Payment.FiscalCode = organizationRequest.Result.FiscalCode;
            payment.Payment.PaymentDestination = model.PaymentDestination;
            payment.OrganizationId = model.OrganizationId;
            payment.ProductId = model.ProductId;
            payment.Payment.TotalPrice = model.TotalPrice;
            payment.Payment.TotalTVA = model.TotalTVA;
            payment.Payment.TVA = model.TVA;
            payment.Payment.TotalPriceWithoutTVA = model.TotalPriceWithoutTVA;
            payment.Payment.UnitPriceWithoutTVA = model.UnitPriceWithoutTVA;
            payment.Payment.Quantity = model.Quantity;
            payment.PaymentCodeId = model.PaymentCodeId;



            _context.PaymentMappers.Update(payment);

            return await _context.PushAsync();
        }

        /// <summary>
        /// Delete payment
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeletePaymentAsync(Guid? paymentId) =>
            await _context.RemovePermanentRecordAsync<PaymentMapped>(paymentId);

        /// <summary>
        /// Disable payment
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisablePaymentAsync(Guid? paymentId) =>
            await _context.DisableRecordAsync<PaymentMapped>(paymentId);

        /// <summary>
        /// Activate payment
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivatePaymentAsync(Guid? paymentId) =>
            await _context.ActivateRecordAsync<PaymentMapped>(paymentId);



        #region Import payments


        /// <summary>
        /// ExtractData to excel
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ImportXmlAsync(IFormFile formFile)
        {
            Documents documents;

            using (var stream = new MemoryStream())
            {
                try
                {
                    if (formFile.FileName.IndexOf(".xml", StringComparison.CurrentCultureIgnoreCase) == -1)
                    {
                        return new InvalidParametersResultModel();
                    }

                    formFile.CopyTo(stream);
                    var bytes = stream.ToArray();
                    var xmlString = Encoding.UTF8.GetString(bytes);
                    var xmlDocument = XDocument.Parse(xmlString);

                    var xmlSerializer = new XmlSerializer(typeof(Documents));

                    using (var reader = xmlDocument.CreateReader())
                    {
                        documents = (Documents)xmlSerializer.Deserialize(reader);
                    }
                }
                catch (Exception ex)
                {
                    return new ResultModel { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = ex.Message } } };
                }
            }

            return await SavePaymentsFromImport(documents);
        }


        /// <summary>
        /// Import payment 
        /// </summary>
        /// <param name="documents"></param>
        /// <returns></returns>
        private async Task<ResultModel> SavePaymentsFromImport(Documents documents)
        {
            if (documents == null)
                return new InvalidParametersResultModel();

            if (!documents.Document.Any())
                return new NotFoundResultModel();

            var listErrors = new List<IErrorModel>();


            var listDocuments = documents.Document;


            foreach (var payment in listDocuments)
            {
                try
                {
                    var buyer = payment.SupplierInfo.Buyer;
                    if(buyer == null)
                        continue;

                    var organizationRequest = await _organizationService.GetOrganizationByFiscalCodeAsync(buyer.IDNO);
                    Guid? organizationId = null;

                    if (organizationRequest.IsSuccess)
                    {
                        organizationId = organizationRequest.Result.Id;
                    }
                    else
                    {
                        var addOrganizationRequest = await _organizationService.AddNewOrganizationAsync(new OrganizationViewModel
                        {
                            Name = buyer.Title,
                            FiscalCode = buyer.IDNO,
                            Bank = buyer.BankAccount.BranchTitle,
                            Email = "Email@mail.com",
                            Phone = "00000000",
                            ClientType = OrganizationType.Client
                        });

                        if (!addOrganizationRequest.IsSuccess)
                        {
                            listErrors.AddRange(addOrganizationRequest.Errors);
                            continue;
                        }
                        organizationId = addOrganizationRequest.Result;
                    }

                    var paymentDate = payment.SupplierInfo.DeliveryDate.StringToDateTime();


                    if (!paymentDate.HasValue)
                    {
                        listErrors.Add(new ErrorModel
                        { Message = "Data [" + payment.SupplierInfo.DeliveryDate + "] wrong format" });
                        continue;
                    }


                    var listRows = payment.SupplierInfo.Merchandises.Row;
                    if (listRows == null || !listRows.Any())
                        continue;

                    foreach (var row in listRows)
                    {
                        var newPayment = new AddPaymentViewModel
                        {
                            DateTransaction = paymentDate.Value,
                            OrganizationId = organizationId.Value,
                            TotalPrice = row.TotalPrice,
                            TotalTVA = row.TotalTVA,
                            TVA = row.TVA,
                            TotalPriceWithoutTVA = row.TotalPriceWithoutTVA,
                            UnitPriceWithoutTVA = row.UnitPriceWithoutTVA,
                            Quantity = row.Quantity,
                            Currency = "MDL",
                            PaymentDestination = row.Name,
                            DocumentNumber = payment.AdditionalInformation.field,
                        };

                        var paymentCodeRequest = await GetPaymentCodeByNameAsync(row.Name);
                        if (!paymentCodeRequest.IsSuccess)
                        {
                            listErrors.Add(new ErrorModel
                            { Message = "Payment name  [" + row.Name + "] not exist" });
                            continue;
                        }

                        newPayment.PaymentCodeId = paymentCodeRequest.Result.Id;

                        if (await ExistPaymentAsync(newPayment))
                            continue;

                        var addPaymentResult = await AddPaymentAsync(newPayment);

                        if (!addPaymentResult.IsSuccess)
                            listErrors.AddRange(addPaymentResult.Errors);
                    }
                }
                catch (Exception ex)
                {
                    listErrors.Add(new ErrorModel
                    { Message = "Error import payment code [" + payment.AdditionalInformation.field + "]  [" + ex.Message + "]" + "[" + ex.StackTrace + "]" });
                }

            }

            listErrors = listErrors.DistinctBy(x => x.Message).ToList();
            return listErrors.Any() ? new ResultModel { IsSuccess = false, Errors = listErrors } : new ResultModel { IsSuccess = true };
        }


        /// <summary>
        /// Get work category by payment name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>

        private  async Task<ResultModel<PaymentCode>> GetPaymentCodeByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                return new InvalidParametersResultModel<PaymentCode>();

            var listTest = await _context.PaymentCodes.ToListAsync();

            var myRegex = new Regex("[^a-z]", RegexOptions.IgnoreCase);
            //string s = MyRegex.Replace(@"your 76% strings &*81 gose _ here and collect you want_{ (7 438 ?. !`", @"");

            var test = myRegex.Replace(name, string.Empty);

            var paymentCode = await _context.PaymentCodes.FirstOrDefaultAsync(x =>
                string.Equals(myRegex.Replace(x.Name, string.Empty), myRegex.Replace(name, string.Empty), StringComparison.InvariantCultureIgnoreCase));

            if (paymentCode == null)
                return new NotFoundResultModel<PaymentCode>();

            return new SuccessResultModel<PaymentCode>(paymentCode);

        }


        /// <summary>
        /// verified exist payment
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<bool> ExistPaymentAsync(AddPaymentViewModel model)
        {
            var payment = await _context.PaymentMappers
                .Include(i => i.Payment)
                .FirstOrDefaultAsync(x => x.Payment.DateTransaction == model.DateTransaction
                                          && x.Payment.TotalPrice == model.TotalPrice
                                          && x.OrganizationId == model.OrganizationId
                                          && x.ProductId == model.ProductId
                                          && x.Payment.DocumentNumber == model.DocumentNumber);
            return payment != null;
        }


        #endregion

    }
}
