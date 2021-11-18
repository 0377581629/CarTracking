import 'package:aspnet_zero_app/abp/interfaces/session_service.dart';
import 'package:aspnet_zero_app/abp/models/auth/login_informations.dart';
import 'package:aspnet_zero_app/abp/models/common/ajax_response.dart';
import 'package:aspnet_zero_app/helpers/http_client.dart';

class SessionAppService implements ISessionAppService {
  @override
  Future<LoginInformations> getCurrentLoginInformations() async {
    var client = await HttpClient().createClient();
    var clientResult = await client
        .get("api/services/app/Session/GetCurrentLoginInformations");
    var ajaxResponse = AjaxResponse<LoginInformations>.fromJson(
        clientResult.data,
        (data) => LoginInformations.fromJson((data as Map<String, dynamic>)));
    return ajaxResponse.result!;
  }

  @override
  Future<UpdateUserSignInToken> updateUserSignInToken() async {
    var client = await HttpClient().createClient();
    var clientResult = await client.put("app/session/updateUserSignInToken");
    var ajaxResponse = AjaxResponse<UpdateUserSignInToken>.fromJson(
        clientResult.data,
        (data) =>
            UpdateUserSignInToken.fromJson((data as Map<String, dynamic>)));
    return ajaxResponse.result!;
  }
}
