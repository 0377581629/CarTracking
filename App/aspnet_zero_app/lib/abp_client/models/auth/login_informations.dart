import 'package:aspnet_zero_app/abp_client/enums/payment_period_type.dart';
import 'package:aspnet_zero_app/abp_client/enums/subscription_payment_type.dart';
import 'package:aspnet_zero_app/abp_client/models/edition/edition_info.dart';
import 'package:tuple/tuple.dart';
import 'package:uuid/uuid.dart';

class UserLoginInfo {
  int id;
  String name;
  String surname;
  String userName;
  String emailAddress;
  String profilePictureId;
  bool isInTrialPeriod;
  DateTime? subscriptionEndDateUtc;
  SubscriptionPaymentType subscriptionPaymentType;
  bool isInTrial() {
    return isInTrialPeriod;
  }

  bool subscriptionIsExpiringSoon(int notifyDayCount) {
    if (subscriptionEndDateUtc != null) {
      return DateTime.now()
          .toUtc()
          .add(Duration(days: notifyDayCount))
          .isAfter(subscriptionEndDateUtc!);
    }
    return false;
  }

  int getSubscriptionExpiringDayCount() {
    if (subscriptionEndDateUtc == null) {
      return 0;
    } else {
      var today = DateTime.now().toUtc();
      return subscriptionEndDateUtc!
          .toUtc()
          .subtract(Duration(milliseconds: today.millisecondsSinceEpoch))
          .day;
    }
  }

  UserLoginInfo(
      this.id,
      this.name,
      this.surname,
      this.userName,
      this.emailAddress,
      this.profilePictureId,
      this.isInTrialPeriod,
      this.subscriptionEndDateUtc,
      this.subscriptionPaymentType);
}

class TenantLoginInfo {
  String tenancyName;

  String name;

  UuidValue? logoId;

  String logoFileType;

  UuidValue? customCssId;

  DateTime? subscriptionEndDateUtc;

  bool isInTrialPeriod;

  SubscriptionPaymentType subscriptionPaymentType;

  EditionInfo edition;

  DateTime creationTime;

  PaymentPeriodType paymentPeriodType;

  String subscriptionDateString;

  String creationTimeString;

  bool isInTrial() {
    return isInTrialPeriod;
  }

  bool subscriptionIsExpiringSoon(int notifyDayCount) {
    if (subscriptionEndDateUtc != null) {
      return DateTime.now()
          .toUtc()
          .add(Duration(days: notifyDayCount))
          .isAfter(subscriptionEndDateUtc!);
    }
    return false;
  }

  int getSubscriptionExpiringDayCount() {
    if (subscriptionEndDateUtc == null) {
      return 0;
    } else {
      var today = DateTime.now().toUtc();
      return subscriptionEndDateUtc!
          .toUtc()
          .subtract(Duration(milliseconds: today.millisecondsSinceEpoch))
          .day;
    }
  }

  bool hasRecurringSubscription() {
    return subscriptionPaymentType != SubscriptionPaymentType.manual;
  }

  UuidValue? loginLogoId;

  UuidValue? menuLogoId;

  UuidValue? loginBackgroundId;

  String loginLogoFileType;

  String menuLogoFileType;

  String loginBackgroundFileType;

  String webTitle;

  String webDescription;

  String webAuthor;

  String webKeyword;

  String webFavicon;

  bool useSubscriptionUser;

  TenantLoginInfo(
      this.creationTime,
      this.creationTimeString,
      this.customCssId,
      this.edition,
      this.isInTrialPeriod,
      this.loginBackgroundFileType,
      this.loginBackgroundId,
      this.loginLogoFileType,
      this.loginLogoId,
      this.logoFileType,
      this.logoId,
      this.menuLogoFileType,
      this.menuLogoId,
      this.name,
      this.paymentPeriodType,
      this.subscriptionDateString,
      this.subscriptionEndDateUtc,
      this.subscriptionPaymentType,
      this.tenancyName,
      this.useSubscriptionUser,
      this.webAuthor,
      this.webDescription,
      this.webFavicon,
      this.webKeyword,
      this.webTitle);
}

class ApplicationLoginInfo {
  String version;
  DateTime releaseDate;
  String currency;
  String currencySign;
  bool allowTenantsToChangeEmailSettings;
  bool userDelegationIsEnabled;
  double twoFactorCodeExpireSeconds;
  Tuple2<String, bool> features;
  bool useSubscriptionUser;
  ApplicationLoginInfo(
      this.version,
      this.releaseDate,
      this.currency,
      this.currencySign,
      this.allowTenantsToChangeEmailSettings,
      this.userDelegationIsEnabled,
      this.twoFactorCodeExpireSeconds,
      this.features,
      this.useSubscriptionUser);
}

class LoginInformations {
  UserLoginInfo user;
  UserLoginInfo impersonatorUser;
  TenantLoginInfo tenant;
  TenantLoginInfo impersonatorTenant;
  ApplicationLoginInfo application;
  LoginInformations(this.user, this.impersonatorUser, this.tenant,
      this.impersonatorTenant, this.application);
}
