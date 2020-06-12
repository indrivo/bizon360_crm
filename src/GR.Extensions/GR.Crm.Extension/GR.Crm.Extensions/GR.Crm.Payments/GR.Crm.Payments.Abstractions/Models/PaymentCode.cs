using System;
using GR.Core;
using GR.Crm.Organizations.Abstractions.Models;
using NPOI.HPSF;

namespace GR.Crm.Payments.Abstractions.Models
{
    public class PaymentCode: BaseModel
    {
        /// <summary>
        /// Code
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name { get; set; }


    }
}
