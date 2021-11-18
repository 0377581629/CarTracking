import 'package:aspnet_zero_app/abp/models/auth/authenticate_model.dart';
import 'package:aspnet_zero_app/abp/models/auth/authenticate_result_model.dart';

abstract class IAccessTokenManager {
  AuthenticateModel? authenticateModel;

  AuthenticateResultModel? authenticateResult;

  String getAccessToken();

  Future<AuthenticateResultModel> loginAsync();

  Future refreshTokenAsync();

  void logout();

  bool isUserLoggedIn();

  bool isTokenExpired();

  bool isRefreshTokenExpired();

  IAccessTokenManager(this.authenticateResult);
}
