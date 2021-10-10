import 'package:aspnet_zero_app/abp_base/interfaces/user_configration_service.dart';
import 'package:aspnet_zero_app/abp_client/models/common/ajax_response.dart';
import 'package:aspnet_zero_app/abp_client/models/user/user_configuration.dart';
import 'package:aspnet_zero_app/helpers/http_client.dart';
import 'package:dio/dio.dart';

class UserConfigurationService implements IUserConfigurationService {
  @override
  Future<UserConfiguration> getUserConfiguration() async {
    var httpClient = await HttpClient().createClient();
    var clientResponse = await httpClient.get('AbpUserConfiguration/GetAll');
    var abpResponse = AjaxResponse<UserConfiguration>.fromJson(
        clientResponse.data,
        (data) => UserConfiguration.fromJson(data as Map<String, dynamic>));
    return abpResponse.result!;
  }
}
