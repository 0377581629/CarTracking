import 'package:aspnet_zero_app/abp/enums/subscription_payment_type.dart';
import 'package:aspnet_zero_app/abp/models/edition/edition_info.dart';
import 'package:json_annotation/json_annotation.dart';
part 'login_informations.g.dart';

@JsonSerializable(explicitToJson: true)
class UserLoginInfo {
  int? id;
  String? name;
  String? surname;
  String? userName;
  String? emailAddress;
  String? profilePictureId;
  bool? isInTrialPeriod;
  DateTime? subscriptionEndDateUtc;
  int? subscriptionPaymentType;

  bool isInTrial() {
    return isInTrialPeriod!;
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
      {this.id,
      this.name,
      this.surname,
      this.userName,
      this.emailAddress,
      this.profilePictureId,
      this.isInTrialPeriod,
      this.subscriptionEndDateUtc,
      this.subscriptionPaymentType});

  factory UserLoginInfo.fromJson(Map<String, dynamic> json) =>
      _$UserLoginInfoFromJson(json);

  Map<String, dynamic> toJson() => _$UserLoginInfoToJson(this);
}

@JsonSerializable(explicitToJson: true)
class TenantLoginInfo {
  String? tenancyName;

  String? name;

  String? logoId;

  String? logoFileType;

  String? customCssId;

  DateTime? subscriptionEndDateUtc;

  bool? isInTrialPeriod;

  int? subscriptionPaymentType;

  EditionInfo? edition;

  DateTime? creationTime;

  int? paymentPeriodType;

  String? subscriptionDateString;

  String? creationTimeString;

  bool isInTrial() {
    return isInTrialPeriod!;
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

  String? loginLogoId;

  String? menuLogoId;

  String? loginBackgroundId;

  String? loginLogoFileType;

  String? menuLogoFileType;

  String? loginBackgroundFileType;

  String? webTitle;

  String? webDescription;

  String? webAuthor;

  String? webKeyword;

  String? webFavicon;

  bool? useSubscriptionUser;

  TenantLoginInfo(
      {this.creationTime,
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
      this.webTitle});

  factory TenantLoginInfo.fromJson(Map<String, dynamic> json) =>
      _$TenantLoginInfoFromJson(json);

  Map<String, dynamic> toJson() => _$TenantLoginInfoToJson(this);
}

@JsonSerializable(explicitToJson: true)
class ApplicationLoginInfo {
  String? version;
  DateTime? releaseDate;
  String? currency;
  String? currencySign;
  bool? allowTenantsToChangeEmailSettings;
  bool? userDelegationIsEnabled;
  double? twoFactorCodeExpireSeconds;
  Map<String, bool>? features;
  bool? useSubscriptionUser;
  ApplicationLoginInfo(
      {this.version,
      this.releaseDate,
      this.currency,
      this.currencySign,
      this.allowTenantsToChangeEmailSettings,
      this.userDelegationIsEnabled,
      this.twoFactorCodeExpireSeconds,
      this.features,
      this.useSubscriptionUser});
  factory ApplicationLoginInfo.fromJson(Map<String, dynamic> json) =>
      _$ApplicationLoginInfoFromJson(json);

  Map<String, dynamic> toJson() => _$ApplicationLoginInfoToJson(this);
}

@JsonSerializable(explicitToJson: true)
class LoginInformations {
  UserLoginInfo? user;
  UserLoginInfo? impersonatorUser;
  TenantLoginInfo? tenant;
  TenantLoginInfo? impersonatorTenant;
  ApplicationLoginInfo? application;
  LoginInformations(
      {this.user,
      this.impersonatorUser,
      this.tenant,
      this.impersonatorTenant,
      this.application});
  factory LoginInformations.fromJson(Map<String, dynamic> json) =>
      _$LoginInformationsFromJson(json);

  Map<String, dynamic> toJson() => _$LoginInformationsToJson(this);
}

@JsonSerializable(explicitToJson: true)
class UpdateUserSignInToken {
  String? signInToken;
  String? encodedUserId;
  String? encodedTenantId;
  UpdateUserSignInToken(
      {this.signInToken, this.encodedUserId, this.encodedTenantId});
  factory UpdateUserSignInToken.fromJson(Map<String, dynamic> json) =>
      _$UpdateUserSignInTokenFromJson(json);

  Map<String, dynamic> toJson() => _$UpdateUserSignInTokenToJson(this);
}
