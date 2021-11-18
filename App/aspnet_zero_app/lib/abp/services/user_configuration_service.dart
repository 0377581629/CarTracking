import 'package:aspnet_zero_app/abp/interfaces/user_configration_service.dart';
import 'package:aspnet_zero_app/abp/manager/interfaces/application_context.dart';
import 'package:aspnet_zero_app/abp/models/common/ajax_response.dart';
import 'package:aspnet_zero_app/abp/models/user/user_configuration.dart';
import 'package:aspnet_zero_app/helpers/http_client.dart';
import 'package:dio/dio.dart';
import 'package:get_it/get_it.dart';

final getIt = GetIt.I;

class UserConfigurationService implements IUserConfigurationService {
  IApplicationContext? appContext;

  UserConfigurationService() {
    appContext = getIt.get<IApplicationContext>();
  }

  @override
  Future<UserConfiguration> getUserConfiguration() async {
    Dio httpClient;

    if (appContext!.loginInfo != null) {
      httpClient = await HttpClient().createClient();
    } else {
      httpClient = await HttpClient().createSimpleClient();
    }

    var clientResponse = await httpClient.get('AbpUserConfiguration/GetAll');
    var abpResponse = AjaxResponse<UserConfiguration>.fromJson(
        clientResponse.data,
        (data) => UserConfiguration.fromJson(data as Map<String, dynamic>));
    return abpResponse.result!;
  }
}
