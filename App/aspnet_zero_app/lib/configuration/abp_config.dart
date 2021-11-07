class AbpConfig {
  static String hostUrl = "https://10.0.2.2:44302/";

  static const userAgent = "AbpApiClient";

  static const fixedMultiTenant = false;

  static const tenantName = '';

  static const tenantResolveKey = "Abp.TenantId";

  static const languageKey = ".AspNetCore.Culture";

  static const appName = "ZeroBase-App";

  static const languageSource = "AbpZero";
}

class DataStorageKey {
  static const currentSessionTokenInfo = "CurrentSession.TokenInfo";
  static const currentSessionLoginInfo = "CurrentSession.LoginInfo";
  static const currentSessionTenantInfo = "CurrentSession.TenantInfo";
}
