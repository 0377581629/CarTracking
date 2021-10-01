using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Zero.Abp.Authorization.Accounting
{
    [Table("AppUserInvoices")]
    public class UserInvoice : Entity<int>
    {
        public string InvoiceNo { get; set; }

        public DateTime InvoiceDate { get; set; }

        public string UserLegalName { get; set; }

        public string UserAddress { get; set; }

        public string UserTaxNo { get; set; }
        
        public string Currency { get; set; }
    }
}
