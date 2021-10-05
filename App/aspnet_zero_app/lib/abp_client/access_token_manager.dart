import 'package:aspnet_zero_app/abp_client/interfaces/access_token_manager.dart';
import 'package:aspnet_zero_app/abp_client/models/auth/authenticate_result_model.dart';

class AccessTokenManager implements IAccessTokenManager {
  static const _loginUrlSegment = "api/TokenAuth/Authenticate";
  static const _refreshTokenUrlSegment = "api/TokenAuth/RefreshToken";

  @override
  DateTime accessTokenRetrieveTime;

  @override
  AbpAuthenticateResultModel authenticateResult;

  @override
  String getAccessToken() {
    // TODO: implement getAccessToken
    throw UnimplementedError();
  }

  @override
  // TODO: implement isRefreshTokenExpired
  bool get isRefreshTokenExpired => throw UnimplementedError();

  @override
  // TODO: implement isUserLoggedIn
  bool get isUserLoggedIn => throw UnimplementedError();

  @override
  Future<AbpAuthenticateResultModel> loginAsync() {
    // TODO: implement loginAsync
    throw UnimplementedError();
  }

  @override
  void logout() {
    // TODO: implement logout
  }

  @override
  Future refreshTokenAsync() {
    // TODO: implement refreshTokenAsync
    throw UnimplementedError();
  }
}
