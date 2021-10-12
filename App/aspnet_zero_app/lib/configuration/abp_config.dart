class AbpConfig {
  static const hostUrl = "https://thegioidichvu.vn/";
  static const userAgent = "AbpApiClient";

  static const tenantResolveKey = "Abp.TenantId";

  static const loginUrlSegment = "api/TokenAuth/Authenticate";
  static const refreshTokenUrlSegment = "api/TokenAuth/RefreshToken";

  static const languageKey = ".AspNetCore.Culture";
}

class DataStorageKey {
  static const currentSessionTokenInfo = "CurrentSession.TokenInfo";
  static const currentSessionLoginInfo = "CurrentSession.LoginInfo";
  static const currentSessionTenantInfo = "CurrentSession.TenantInfo";
}
