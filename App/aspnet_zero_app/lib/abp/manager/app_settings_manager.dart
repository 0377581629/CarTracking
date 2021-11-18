import 'package:aspnet_zero_app/abp/manager/interfaces/app_settings_manager.dart';
import 'package:aspnet_zero_app/abp/manager/interfaces/application_context.dart';
import 'package:get_it/get_it.dart';

final getIt = GetIt.I;

class AppSettingsManager implements IAppSettingsManager {

  IApplicationContext? appContext;

  AppSettingsManager() {
    appContext = getIt.get<IApplicationContext>();
  }

  @override
  String? getSetting(String key) {
    if (appContext!.configuration == null ||
        appContext!.configuration!.setting == null ||
        !appContext!.configuration!.setting!.values!.containsKey(key)) {
      return null;
    }
    else {
      return appContext!.configuration!.setting!.values![key].toString();
    }
  }

  @override
  int getNumberSetting(String key, int nullValue) {
    if (appContext!.configuration == null ||
        appContext!.configuration!.setting == null ||
        !appContext!.configuration!.setting!.values!.containsKey(key)) {
      return nullValue;
    }
    else {
      return int.tryParse(appContext!.configuration!.setting!.values![key].toString()) ?? nullValue;
    }
  }

  @override
  bool confirmSetting(String key, dynamic value) {
    var setting = getSetting(key);
    if (value is bool) {
      value = value.toString().toLowerCase();
    }
    return setting != null && setting.toLowerCase() == value.toString().toLowerCase();
  }
}
