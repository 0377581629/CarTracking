import 'package:aspnet_zero_app/abp/manager/interfaces/application_context.dart';
import 'package:aspnet_zero_app/configuration/abp_config.dart';
import 'package:get_it/get_it.dart';

class LocalizationHelper {
  final getIt = GetIt.I;
  IApplicationContext? _applicationContext;

  LocalizationHelper() {
    _applicationContext = getIt.get<IApplicationContext>();
  }

  String get(String inputKey) {
    if (inputKey.isNotEmpty && !inputKey.startsWith('MB_')) {
      inputKey = 'MB_' + inputKey;
    }
    if (_applicationContext?.configuration?.localization?.values == null) {
      return inputKey;
    }
    if (_applicationContext!.configuration!.localization!.values!
        .containsKey(AbpConfig.languageSource)) {
      var langSources = _applicationContext!
          .configuration!.localization?.values![AbpConfig.languageSource];
      if (langSources!.containsKey(inputKey)) {
        return langSources[inputKey].toString();
      }
    }
    return '[$inputKey]';
  }
}
