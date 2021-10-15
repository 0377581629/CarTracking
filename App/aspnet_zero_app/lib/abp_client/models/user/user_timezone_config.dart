import 'package:json_annotation/json_annotation.dart';
part 'user_timezone_config.g.dart';

@JsonSerializable()
class UserIanaTimeZoneConfig {
  String? timeZoneId;
  UserIanaTimeZoneConfig({this.timeZoneId});
  factory UserIanaTimeZoneConfig.fromJson(Map<String, dynamic> json) =>
      _$UserIanaTimeZoneConfigFromJson(json);

  Map<String, dynamic> toJson() => _$UserIanaTimeZoneConfigToJson(this);
}

@JsonSerializable()
class UserWindowsTimeZoneConfig {
  String? timeZoneId;
  double? baseUtcOffsetInMilliseconds;
  double? currentUtcOffsetInMilliseconds;
  bool? isDaylightSavingTimeNow;
  UserWindowsTimeZoneConfig(
      {this.timeZoneId,
      this.baseUtcOffsetInMilliseconds,
      this.currentUtcOffsetInMilliseconds,
      this.isDaylightSavingTimeNow});

  factory UserWindowsTimeZoneConfig.fromJson(Map<String, dynamic> json) =>
      _$UserWindowsTimeZoneConfigFromJson(json);

  Map<String, dynamic> toJson() => _$UserWindowsTimeZoneConfigToJson(this);
}

@JsonSerializable()
class UserTimeZoneConfig {
  UserWindowsTimeZoneConfig? windows;
  UserIanaTimeZoneConfig? iana;
  UserTimeZoneConfig({this.windows, this.iana});

  factory UserTimeZoneConfig.fromJson(Map<String, dynamic> json) =>
      _$UserTimeZoneConfigFromJson(json);

  Map<String, dynamic> toJson() => _$UserTimeZoneConfigToJson(this);
}

@JsonSerializable()
class UserTimmingConfig {
  UserTimeZoneConfig? timeZoneInfo;
  UserTimmingConfig({this.timeZoneInfo});

  factory UserTimmingConfig.fromJson(Map<String, dynamic> json) =>
      _$UserTimmingConfigFromJson(json);

  Map<String, dynamic> toJson() => _$UserTimmingConfigToJson(this);
}
