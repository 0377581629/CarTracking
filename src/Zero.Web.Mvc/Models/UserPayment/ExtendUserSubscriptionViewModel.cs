using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Zero.Editions.Dto;
using Zero.MultiTenancy.Payments;

namespace Zero.Web.Models.UserPayment
{
    public class ExtendUserSubscriptionViewModel
    {
        public long UserId { get; set; }
        
        public string UserEmail { get; set; }
        
        public double MonthlyPrice { get; set; }
        
        public double YearlyPrice { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}