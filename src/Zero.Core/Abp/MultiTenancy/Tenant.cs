using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Abp.Collections.Extensions;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Timing;
using Zero.Authorization.Users;
using Zero.Editions;
using Zero.MultiTenancy.Payments;

namespace Zero.MultiTenancy
{
    /// <summary>
    /// Represents a Tenant in the system.
    /// A tenant is a isolated customer for the application
    /// which has it's own users, roles and other application entities.
    /// </summary>
    public class Tenant : AbpTenant<User>
    {
        public const int MaxLogoMimeTypeLength = 64;

        //Can add application specific tenant properties here

        public DateTime? SubscriptionEndDateUtc { get; set; }

        public bool IsInTrialPeriod { get; set; }

        public virtual Guid? CustomCssId { get; set; }

        public virtual Guid? LogoId { get; set; }

        [MaxLength(MaxLogoMimeTypeLength)]
        public virtual string LogoFileType { get; set; }

        public SubscriptionPaymentType SubscriptionPaymentType { get; set; }

        public Tenant()
        {

        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {

        }

        public virtual bool HasLogo()
        {
            return LogoId != null && LogoFileType != null;
        }

        public void ClearLogo()
        {
            LogoId = null;
            LogoFileType = null;
        }

        public void UpdateSubscriptionDateForPayment(PaymentPeriodType paymentPeriodType, EditionPaymentType editionPaymentType)
        {
            switch (editionPaymentType)
            {
                case EditionPaymentType.NewRegistration:
                case EditionPaymentType.BuyNow:
                    {
                        SubscriptionEndDateUtc = Clock.Now.ToUniversalTime().AddDays((int)paymentPeriodType);
                        break;
                    }
                case EditionPaymentType.Extend:
                    ExtendSubscriptionDate(paymentPeriodType);
                    break;
                case EditionPaymentType.Upgrade:
                    if (HasUnlimitedTimeSubscription())
                    {
                        SubscriptionEndDateUtc = Clock.Now.ToUniversalTime().AddDays((int)paymentPeriodType);
                    }
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private void ExtendSubscriptionDate(PaymentPeriodType paymentPeriodType)
        {
            if (SubscriptionEndDateUtc == null)
            {
                throw new InvalidOperationException("Can not extend subscription date while it's null!");
            }

            if (IsSubscriptionEnded())
            {
                SubscriptionEndDateUtc = Clock.Now.ToUniversalTime();
            }

            SubscriptionEndDateUtc = SubscriptionEndDateUtc.Value.AddDays((int)paymentPeriodType);
        }

        private bool IsSubscriptionEnded()
        {
            return SubscriptionEndDateUtc < Clock.Now.ToUniversalTime();
        }

        public int CalculateRemainingHoursCount()
        {
            return SubscriptionEndDateUtc != null
                ? (int)(SubscriptionEndDateUtc.Value - Clock.Now.ToUniversalTime()).TotalHours //converting it to int is not a problem since max value ((DateTime.MaxValue - DateTime.MinValue).TotalHours = 87649416) is in range of integer.
                : 0;
        }

        public bool HasUnlimitedTimeSubscription()
        {
            return SubscriptionEndDateUtc == null;
        }
        
        #region Extent Base

        public int? ParentId { get; set; }
        public string Code { get; set; }

        public string WebTitle { get; set; }

        public string WebDescription { get; set; }

        public string WebAuthor { get; set; }

        public string WebKeyword { get; set; }

        public string WebFavicon { get; set; }

        public string Domain { get; set; }
        
        #endregion
        
        #region Extent UI

        public virtual Guid? LoginLogoId { get; set; }

        public virtual Guid? MenuLogoId { get; set; }

        public virtual Guid? LoginBackgroundId { get; set; }
        [MaxLength(MaxLogoMimeTypeLength)] public virtual string LoginLogoFileType { get; set; }

        [MaxLength(MaxLogoMimeTypeLength)] public virtual string MenuLogoFileType { get; set; }

        [MaxLength(MaxLogoMimeTypeLength)] public virtual string LoginBackgroundFileType { get; set; }
        
        public virtual bool HasLoginLogo()
        {
            return LoginLogoId != null && LoginLogoFileType != null;
        }

        public void ClearLoginLogo()
        {
            LoginLogoId = null;
            LoginLogoFileType = null;
        }
        
        public virtual bool HasLoginBackground()
        {
            return LoginBackgroundId != null && LoginBackgroundFileType != null;
        }

        public void ClearLoginBackground()
        {
            LoginBackgroundId = null;
            LoginBackgroundFileType = null;
        }
        
        public virtual bool HasMenuLogo()
        {
            return MenuLogoId != null && MenuLogoFileType != null;
        }

        public void ClearMenuLogo()
        {
            MenuLogoId = null;
            MenuLogoFileType = null;
        }

        #endregion
        
        #region Static function

        public static string CreateCode(params int[] numbers)
        {
            if (numbers.IsNullOrEmpty())
            {
                return null;
            }

            return numbers
                .Select(number => number.ToString(new string('0', ZeroConst.CodeUnitLength)))
                .JoinAsString(".");
        }

        public static string AppendCode(string parentCode, string childCode)
        {
            if (childCode.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(childCode), "childCode can not be null or empty.");
            }

            if (parentCode.IsNullOrEmpty())
            {
                return childCode;
            }

            return parentCode + "." + childCode;
        }

        public static string GetRelativeCode(string code, string parentCode)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            if (parentCode.IsNullOrEmpty())
            {
                return code;
            }

            if (code.Length == parentCode.Length)
            {
                return null;
            }

            return code.Substring(parentCode.Length + 1);
        }

        public static string CalculateNextCode(string code)
        {
            if (code.IsNullOrEmpty())
                code = "00001";

            var parentCode = GetParentCode(code);
            var lastUnitCode = GetLastUnitCode(code);

            return AppendCode(parentCode, CreateCode(Convert.ToInt32(lastUnitCode) + 1));
        }

        public static string GetLastUnitCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var splittedCode = code.Split('.');
            return splittedCode[splittedCode.Length - 1];
        }

        public static string GetParentCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var splittedCode = code.Split('.');
            if (splittedCode.Length == 1)
            {
                return null;
            }

            return splittedCode.Take(splittedCode.Length - 1).JoinAsString(".");
        }

        #endregion
    }
}