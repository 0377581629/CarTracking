class AppSettings {}

class HostManagement extends AppSettings {
  static const String BillingLegalName = "App.HostManagement.BillingLegalName";
  static const String BillingAddress = "App.HostManagement.BillingAddress";
}

class TenantManagement extends AppSettings {
  static const String AllowSelfRegistration = "App.TenantManagement.AllowSelfRegistration";
  static const String IsNewRegisteredTenantActiveByDefault = "App.TenantManagement.IsNewRegisteredTenantActiveByDefault";
  static const String UseCaptchaOnRegistration = "App.TenantManagement.UseCaptchaOnRegistration";
  static const String DefaultEdition = "App.TenantManagement.DefaultEdition";
  static const String SubscriptionExpireNotifyDayCount = "App.TenantManagement.SubscriptionExpireNotifyDayCount";
  static const String BillingLegalName = "App.TenantManagement.BillingLegalName";
  static const String BillingAddress = "App.TenantManagement.BillingAddress";
  static const String BillingTaxVatNo = "App.TenantManagement.BillingTaxVatNo";
}

class UserManagement extends AppSettings {
  static const String AllowSelfRegistration = "App.UserManagement.AllowSelfRegistration";
  static const String IsNewRegisteredUserActiveByDefault = "App.UserManagement.IsNewRegisteredUserActiveByDefault";
  static const String UseCaptchaOnRegistration = "App.UserManagement.UseCaptchaOnRegistration";
  static const String UseCaptchaOnLogin = "App.UserManagement.UseCaptchaOnLogin";
  static const String SmsVerificationEnabled = "App.UserManagement.SmsVerificationEnabled";
  static const String IsCookieConsentEnabled = "App.UserManagement.IsCookieConsentEnabled";
  static const String IsQuickThemeSelectEnabled = "App.UserManagement.IsQuickThemeSelectEnabled";
  static const String AllowOneConcurrentLoginPerUser = "App.UserManagement.AllowOneConcurrentLoginPerUser";
  static const String AllowUsingGravatarProfilePicture = "App.UserManagement.AllowUsingGravatarProfilePicture";
  static const String UseGravatarProfilePicture = "App.UserManagement.UseGravatarProfilePicture";

  // Subscription User
  static const String SubscriptionUser = "App.UserManagement.SubscriptionUser";
  static const String SubscriptionTrialDays = "App.UserManagement.SubscriptionTrialDays";
  static const String SubscriptionCurrency = "App.UserManagement.SubscriptionCurrency";
  static const String SubscriptionMonthlyPrice = "App.UserManagement.SubscriptionMonthlyPrice";
  static const String SubscriptionYearlyPrice = "App.UserManagement.SubscriptionYearlyPrice";
}

class TwoFactorLogin extends UserManagement {
  static const String IsEnabled = "Abp.Zero.UserManagement.TwoFactorLogin.IsEnabled";
  static const String IsRememberBrowserEnabled = "Abp.Zero.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled";
  static const String IsEmailProviderEnabled = "Abp.Zero.UserManagement.TwoFactorLogin.IsEmailProviderEnabled";
  static const String IsSmsProviderEnabled = "Abp.Zero.UserManagement.TwoFactorLogin.IsSmsProviderEnabled";
  static const String IsGoogleAuthenticatorEnabled = "App.UserManagement.TwoFactorLogin.IsGoogleAuthenticatorEnabled";
}

class SessionTimeOut extends UserManagement {
  static const String IsEnabled = "App.UserManagement.SessionTimeOut.IsEnabled";
  static const String TimeOutSecond = "App.UserManagement.SessionTimeOut.TimeOutSecond";
  static const String ShowTimeOutNotificationSecond = "App.UserManagement.SessionTimeOut.ShowTimeOutNotificationSecond";
  static const String ShowLockScreenWhenTimedOut = "App.UserManagement.SessionTimeOut.ShowLockScreenWhenTimedOut";
}

class ExternalLoginProvider extends AppSettings {
  static const String Facebook = "ExternalLoginProvider.Facebook";
  static const String Facebook_IsDeactivated = "ExternalLoginProvider.Facebook.IsDeactivated";

  static const String Google = "ExternalLoginProvider.Google";
  static const String Google_IsDeactivated = "ExternalLoginProvider.Google.IsDeactivated";

  static const String Twitter = "ExternalLoginProvider.Twitter";
  static const String Twitter_IsDeactivated = "ExternalLoginProvider.Twitter.IsDeactivated";

  static const String Microsoft = "ExternalLoginProvider.Microsoft";
  static const String Microsoft_IsDeactivated = "ExternalLoginProvider.Microsoft.IsDeactivated";

  static const String OpenIdConnect = "ExternalLoginProvider.OpenIdConnect";
  static const String OpenIdConnect_IsDeactivated = "ExternalLoginProvider.OpenIdConnect.IsDeactivated";

  static const String WsFederation = "ExternalLoginProvider.WsFederation";
  static const String WsFederation_IsDeactivated = "ExternalLoginProvider.WsFederation.IsDeactivated";
}

class Tenant extends ExternalLoginProvider {
  static const String Facebook = "ExternalLoginProvider.Facebook.Tenant";
  static const String Google = "ExternalLoginProvider.Google.Tenant";
  static const String Twitter = "ExternalLoginProvider.Twitter.Tenant";
  static const String Microsoft = "ExternalLoginProvider.Microsoft.Tenant";
  static const String OpenIdConnect = "ExternalLoginProvider.OpenIdConnect.Tenant";
  static const String WsFederation = "ExternalLoginProvider.WsFederation.Tenant";
}
