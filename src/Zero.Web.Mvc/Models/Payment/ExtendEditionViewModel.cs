using System.Collections.Generic;
using Zero.Editions.Dto;
using Zero.MultiTenancy.Payments;

namespace Zero.Web.Models.Payment
{
    public class ExtendEditionViewModel
    {
        public EditionSelectDto Edition { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}