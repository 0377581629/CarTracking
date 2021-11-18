enum PaymentPeriodType { daily, weekly, monthly, annual }

extension PaymentPeriodTypeExtension on PaymentPeriodType {
  int get dayCount {
    switch (this) {
      case PaymentPeriodType.daily:
        return 1;
      case PaymentPeriodType.weekly:
        return 7;
      case PaymentPeriodType.monthly:
        return 30;
      case PaymentPeriodType.annual:
        return 365;
      default:
        return 1;
    }
  }

  String get value {
    switch (this) {
      case PaymentPeriodType.daily:
        return "Daily";
      case PaymentPeriodType.weekly:
        return "Weekly";
      case PaymentPeriodType.monthly:
        return "Monthly";
      case PaymentPeriodType.annual:
        return "Annual";
      default:
        return "Daily";
    }
  }
}
