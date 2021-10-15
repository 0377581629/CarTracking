// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'login_informations.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

UserLoginInfo _$UserLoginInfoFromJson(Map<String, dynamic> json) =>
    UserLoginInfo(
      id: json['id'] as int?,
      name: json['name'] as String?,
      surname: json['surname'] as String?,
      userName: json['userName'] as String?,
      emailAddress: json['emailAddress'] as String?,
      profilePictureId: json['profilePictureId'] as String?,
      isInTrialPeriod: json['isInTrialPeriod'] as bool?,
      subscriptionEndDateUtc: json['subscriptionEndDateUtc'] == null
          ? null
          : DateTime.parse(json['subscriptionEndDateUtc'] as String),
      subscriptionPaymentType: json['subscriptionPaymentType'] as int?,
    );

Map<String, dynamic> _$UserLoginInfoToJson(UserLoginInfo instance) =>
    <String, dynamic>{
      'id': instance.id,
      'name': instance.name,
      'surname': instance.surname,
      'userName': instance.userName,
      'emailAddress': instance.emailAddress,
      'profilePictureId': instance.profilePictureId,
      'isInTrialPeriod': instance.isInTrialPeriod,
      'subscriptionEndDateUtc':
          instance.subscriptionEndDateUtc?.toIso8601String(),
      'subscriptionPaymentType': instance.subscriptionPaymentType,
    };

TenantLoginInfo _$TenantLoginInfoFromJson(Map<String, dynamic> json) =>
    TenantLoginInfo(
      creationTime: json['creationTime'] == null
          ? null
          : DateTime.parse(json['creationTime'] as String),
      creationTimeString: json['creationTimeString'] as String?,
      customCssId: json['customCssId'] as String?,
      edition: json['edition'] == null
          ? null
          : EditionInfo.fromJson(json['edition'] as Map<String, dynamic>),
      isInTrialPeriod: json['isInTrialPeriod'] as bool?,
      loginBackgroundFileType: json['loginBackgroundFileType'] as String?,
      loginBackgroundId: json['loginBackgroundId'] as String?,
      loginLogoFileType: json['loginLogoFileType'] as String?,
      loginLogoId: json['loginLogoId'] as String?,
      logoFileType: json['logoFileType'] as String?,
      logoId: json['logoId'] as String?,
      menuLogoFileType: json['menuLogoFileType'] as String?,
      menuLogoId: json['menuLogoId'] as String?,
      name: json['name'] as String?,
      paymentPeriodType: json['paymentPeriodType'] as int?,
      subscriptionDateString: json['subscriptionDateString'] as String?,
      subscriptionEndDateUtc: json['subscriptionEndDateUtc'] == null
          ? null
          : DateTime.parse(json['subscriptionEndDateUtc'] as String),
      subscriptionPaymentType: json['subscriptionPaymentType'] as int?,
      tenancyName: json['tenancyName'] as String?,
      useSubscriptionUser: json['useSubscriptionUser'] as bool?,
      webAuthor: json['webAuthor'] as String?,
      webDescription: json['webDescription'] as String?,
      webFavicon: json['webFavicon'] as String?,
      webKeyword: json['webKeyword'] as String?,
      webTitle: json['webTitle'] as String?,
    );

Map<String, dynamic> _$TenantLoginInfoToJson(TenantLoginInfo instance) =>
    <String, dynamic>{
      'tenancyName': instance.tenancyName,
      'name': instance.name,
      'logoId': instance.logoId,
      'logoFileType': instance.logoFileType,
      'customCssId': instance.customCssId,
      'subscriptionEndDateUtc':
          instance.subscriptionEndDateUtc?.toIso8601String(),
      'isInTrialPeriod': instance.isInTrialPeriod,
      'subscriptionPaymentType': instance.subscriptionPaymentType,
      'edition': instance.edition,
      'creationTime': instance.creationTime?.toIso8601String(),
      'paymentPeriodType': instance.paymentPeriodType,
      'subscriptionDateString': instance.subscriptionDateString,
      'creationTimeString': instance.creationTimeString,
      'loginLogoId': instance.loginLogoId,
      'menuLogoId': instance.menuLogoId,
      'loginBackgroundId': instance.loginBackgroundId,
      'loginLogoFileType': instance.loginLogoFileType,
      'menuLogoFileType': instance.menuLogoFileType,
      'loginBackgroundFileType': instance.loginBackgroundFileType,
      'webTitle': instance.webTitle,
      'webDescription': instance.webDescription,
      'webAuthor': instance.webAuthor,
      'webKeyword': instance.webKeyword,
      'webFavicon': instance.webFavicon,
      'useSubscriptionUser': instance.useSubscriptionUser,
    };

ApplicationLoginInfo _$ApplicationLoginInfoFromJson(
        Map<String, dynamic> json) =>
    ApplicationLoginInfo(
      version: json['version'] as String?,
      releaseDate: json['releaseDate'] == null
          ? null
          : DateTime.parse(json['releaseDate'] as String),
      currency: json['currency'] as String?,
      currencySign: json['currencySign'] as String?,
      allowTenantsToChangeEmailSettings:
          json['allowTenantsToChangeEmailSettings'] as bool?,
      userDelegationIsEnabled: json['userDelegationIsEnabled'] as bool?,
      twoFactorCodeExpireSeconds:
          (json['twoFactorCodeExpireSeconds'] as num?)?.toDouble(),
      features: (json['features'] as Map<String, dynamic>?)?.map(
        (k, e) => MapEntry(k, e as bool),
      ),
      useSubscriptionUser: json['useSubscriptionUser'] as bool?,
    );

Map<String, dynamic> _$ApplicationLoginInfoToJson(
        ApplicationLoginInfo instance) =>
    <String, dynamic>{
      'version': instance.version,
      'releaseDate': instance.releaseDate?.toIso8601String(),
      'currency': instance.currency,
      'currencySign': instance.currencySign,
      'allowTenantsToChangeEmailSettings':
          instance.allowTenantsToChangeEmailSettings,
      'userDelegationIsEnabled': instance.userDelegationIsEnabled,
      'twoFactorCodeExpireSeconds': instance.twoFactorCodeExpireSeconds,
      'features': instance.features,
      'useSubscriptionUser': instance.useSubscriptionUser,
    };

LoginInformations _$LoginInformationsFromJson(Map<String, dynamic> json) =>
    LoginInformations(
      user: json['user'] == null
          ? null
          : UserLoginInfo.fromJson(json['user'] as Map<String, dynamic>),
      impersonatorUser: json['impersonatorUser'] == null
          ? null
          : UserLoginInfo.fromJson(
              json['impersonatorUser'] as Map<String, dynamic>),
      tenant: json['tenant'] == null
          ? null
          : TenantLoginInfo.fromJson(json['tenant'] as Map<String, dynamic>),
      impersonatorTenant: json['impersonatorTenant'] == null
          ? null
          : TenantLoginInfo.fromJson(
              json['impersonatorTenant'] as Map<String, dynamic>),
      application: json['application'] == null
          ? null
          : ApplicationLoginInfo.fromJson(
              json['application'] as Map<String, dynamic>),
    );

Map<String, dynamic> _$LoginInformationsToJson(LoginInformations instance) =>
    <String, dynamic>{
      'user': instance.user,
      'impersonatorUser': instance.impersonatorUser,
      'tenant': instance.tenant,
      'impersonatorTenant': instance.impersonatorTenant,
      'application': instance.application,
    };

UpdateUserSignInToken _$UpdateUserSignInTokenFromJson(
        Map<String, dynamic> json) =>
    UpdateUserSignInToken(
      signInToken: json['signInToken'] as String?,
      encodedUserId: json['encodedUserId'] as String?,
      encodedTenantId: json['encodedTenantId'] as String?,
    );

Map<String, dynamic> _$UpdateUserSignInTokenToJson(
        UpdateUserSignInToken instance) =>
    <String, dynamic>{
      'signInToken': instance.signInToken,
      'encodedUserId': instance.encodedUserId,
      'encodedTenantId': instance.encodedTenantId,
    };
