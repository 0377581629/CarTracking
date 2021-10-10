// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'login_informations.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

UserLoginInfo _$UserLoginInfoFromJson(Map<String, dynamic> json) =>
    UserLoginInfo(
      json['id'] as int,
      json['name'] as String,
      json['surname'] as String,
      json['userName'] as String,
      json['emailAddress'] as String,
      json['profilePictureId'] as String,
      json['isInTrialPeriod'] as bool,
      json['subscriptionEndDateUtc'] == null
          ? null
          : DateTime.parse(json['subscriptionEndDateUtc'] as String),
      json['subscriptionPaymentType'] as String,
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
      DateTime.parse(json['creationTime'] as String),
      json['creationTimeString'] as String,
      json['customCssId'] as String?,
      EditionInfo.fromJson(json['edition'] as Map<String, dynamic>),
      json['isInTrialPeriod'] as bool,
      json['loginBackgroundFileType'] as String,
      json['loginBackgroundId'] as String?,
      json['loginLogoFileType'] as String,
      json['loginLogoId'] as String?,
      json['logoFileType'] as String,
      json['logoId'] as String?,
      json['menuLogoFileType'] as String,
      json['menuLogoId'] as String?,
      json['name'] as String,
      _$enumDecode(_$PaymentPeriodTypeEnumMap, json['paymentPeriodType']),
      json['subscriptionDateString'] as String,
      json['subscriptionEndDateUtc'] == null
          ? null
          : DateTime.parse(json['subscriptionEndDateUtc'] as String),
      json['subscriptionPaymentType'] as String,
      json['tenancyName'] as String,
      json['useSubscriptionUser'] as bool,
      json['webAuthor'] as String,
      json['webDescription'] as String,
      json['webFavicon'] as String,
      json['webKeyword'] as String,
      json['webTitle'] as String,
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
      'creationTime': instance.creationTime.toIso8601String(),
      'paymentPeriodType':
          _$PaymentPeriodTypeEnumMap[instance.paymentPeriodType],
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

K _$enumDecode<K, V>(
  Map<K, V> enumValues,
  Object? source, {
  K? unknownValue,
}) {
  if (source == null) {
    throw ArgumentError(
      'A value must be provided. Supported values: '
      '${enumValues.values.join(', ')}',
    );
  }

  return enumValues.entries.singleWhere(
    (e) => e.value == source,
    orElse: () {
      if (unknownValue == null) {
        throw ArgumentError(
          '`$source` is not one of the supported values: '
          '${enumValues.values.join(', ')}',
        );
      }
      return MapEntry(unknownValue, enumValues.values.first);
    },
  ).key;
}

const _$PaymentPeriodTypeEnumMap = {
  PaymentPeriodType.daily: 'daily',
  PaymentPeriodType.weekly: 'weekly',
  PaymentPeriodType.monthly: 'monthly',
  PaymentPeriodType.annual: 'annual',
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
