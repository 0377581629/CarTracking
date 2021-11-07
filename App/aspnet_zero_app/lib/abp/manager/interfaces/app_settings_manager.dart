abstract class IAppSettingsManager {
  String? getSetting(String key);

  int getNumberSetting(String key, int nullValue);

  bool confirmSetting(String key, dynamic value);
}