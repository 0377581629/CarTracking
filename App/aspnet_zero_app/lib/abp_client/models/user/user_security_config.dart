class UserAntiForgeryConfig {
  String tokenCookieName;

  String tokenHeaderName;

  UserAntiForgeryConfig(this.tokenCookieName, this.tokenHeaderName);
}

class UserSecurityConfig {
  UserAntiForgeryConfig antiForgery;
  UserSecurityConfig(this.antiForgery);
}
