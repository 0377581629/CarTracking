abstract class IAppSettingsManager {
  String? getSetting(String key);

  bool confirmSetting(String key, dynamic value);
}