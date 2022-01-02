using Zero.Editions.Dto;

namespace Zero.Abp.Payments.Dto
{
    public class UserPaymentInfoDto
    {
        public EditionSelectDto Edition { get; set; }

        public decimal AdditionalPrice { get; set; }

        public bool IsLessThanMinimumUpgradePaymentAmount()
        {
            return AdditionalPrice < ZeroConst.MinimumUpgradePaymentAmount;
        }
    }
}
