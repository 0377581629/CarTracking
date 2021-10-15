class AbpConfig {
  static const hostUrl = "https://todo.368up.com/";
  static const userAgent = "AbpApiClient";
  static const tenantResolveKey = "Abp.TenantId";
  static const loginUrlSegment = "api/TokenAuth/Authenticate";
  static const refreshTokenUrlSegment = "api/TokenAuth/RefreshToken";
  static const refreshTokenExpirationDays = 365;
}
