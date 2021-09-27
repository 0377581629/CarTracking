﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Zero.Authorization.Users;
using Zero.MultiTenancy.Payments;

namespace Zero.Abp.Authorization.Users
{
    [Table("AppUserSubscriptionPayments")]
    public class UserSubscriptionPayment : FullAuditedEntity<long>
    {
        public string Description { get; set; }

        public SubscriptionPaymentGatewayType Gateway { get; set; }

        public decimal Amount { get; set; }

        public SubscriptionPaymentStatus Status { get; protected set; }

        public long UserId { get; set; }

        public User User { get; set; }
        public int DayCount { get; set; }

        public PaymentPeriodType? PaymentPeriodType { get; set; }

        public string ExternalPaymentId { get; set; }
        
        public string InvoiceNo { get; set; }
        
        public bool IsRecurring { get; set; }

        public string SuccessUrl { get; set; }

        public string ErrorUrl { get; set; }

        public void SetAsCancelled()
        {
            if (Status == SubscriptionPaymentStatus.NotPaid)
            {
                Status = SubscriptionPaymentStatus.Cancelled;
            }
        }

        public void SetAsFailed()
        {
            Status = SubscriptionPaymentStatus.Failed;
        }

        public void SetAsPaid()
        {
            if (Status == SubscriptionPaymentStatus.NotPaid)
            {
                Status = SubscriptionPaymentStatus.Paid;
            }
        }

        public void SetAsCompleted()
        {
            if (Status == SubscriptionPaymentStatus.Paid)
            {
                Status = SubscriptionPaymentStatus.Completed;
            }
        }

        public UserSubscriptionPayment()
        {
            Status = SubscriptionPaymentStatus.NotPaid;
        }

        public PaymentPeriodType GetPaymentPeriodType()
        {
            switch (DayCount)
            {
                case 1:
                    return MultiTenancy.Payments.PaymentPeriodType.Daily;
                case 7:
                    return MultiTenancy.Payments.PaymentPeriodType.Weekly;
                case 30:
                    return MultiTenancy.Payments.PaymentPeriodType.Monthly;
                case 365:
                    return MultiTenancy.Payments.PaymentPeriodType.Annual;
                default:
                    throw new NotImplementedException($"PaymentPeriodType for {DayCount} day could not found");
            }
        }
    }
}
