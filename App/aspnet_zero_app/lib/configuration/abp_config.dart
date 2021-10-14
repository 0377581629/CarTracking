class AbpConfig {
  static const hostUrl = "https://192.168.1.180:44302/";
  static const userAgent = "AbpApiClient";

  static const tenantResolveKey = "Abp.TenantId";

  static const loginUrlSegment = "api/TokenAuth/Authenticate";
  static const refreshTokenUrlSegment = "api/TokenAuth/RefreshToken";

  static const languageKey = ".AspNetCore.Culture";

  static const appName = "ZeroBase-App";

  static const languageSource = "Zero";
}

class DataStorageKey {
  static const currentSessionTokenInfo = "CurrentSession.TokenInfo";
  static const currentSessionLoginInfo = "CurrentSession.LoginInfo";
  static const currentSessionTenantInfo = "CurrentSession.TenantInfo";
}
