import 'package:aspnet_zero_app/abp/models/auth/login_informations.dart';

abstract class ISessionAppService {
  Future<LoginInformations> getCurrentLoginInformations();
  Future<UpdateUserSignInToken> updateUserSignInToken();
}
