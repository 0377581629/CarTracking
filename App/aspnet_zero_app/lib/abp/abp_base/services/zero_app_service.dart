import 'package:aspnet_zero_app/abp/abp_base/interfaces/zero_app_service.dart';
import 'package:aspnet_zero_app/abp/models/common/ajax_response.dart';
import 'package:dio/dio.dart';

class ZeroAppService implements IZeroAppService {
  @override
  Future getTenancyByName(String tenancyName) async {
    try {
      
    } on DioError catch (e) {
      if (e.response != null && e.response!.data is Map<String, dynamic>) {
        var simpleResponse = SimpleAjaxResponse.fromJson(e.response!.data);
        if (simpleResponse.errorInfo != null) {
        }
      }
    }
  }
}