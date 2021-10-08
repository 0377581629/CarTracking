class AbpConfig {
  static const hostUrl = "https://192.168.1.150:44302/";
  static const userAgent = "AbpApiClient";
  static const tenantResolveKey = "Abp.TenantId";
  static const loginUrlSegment = "api/TokenAuth/Authenticate";
  static const refreshTokenUrlSegment = "api/TokenAuth/RefreshToken";
  static const refreshTokenExpirationDays = 365;
}
