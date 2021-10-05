import 'package:aspnet_zero_app/abp_client/models/auth/authenticate_result_model.dart';

abstract class IAccessTokenManager {
  String getAccessToken();
  Future<AbpAuthenticateResultModel> loginAsync();
  Future refreshTokenAsync();
  void logout();
  bool _isUserLoggedIn;
  bool _isRefreshTokenExpired;
  bool get isUserLoggedIn => _isUserLoggedIn;
  bool get isRefreshTokenExpired => _isRefreshTokenExpired;
  AbpAuthenticateResultModel authenticateResult;
  DateTime accessTokenRetrieveTime;
  IAccessTokenManager(this._isUserLoggedIn, this._isRefreshTokenExpired,
      this.authenticateResult, this.accessTokenRetrieveTime);
}
