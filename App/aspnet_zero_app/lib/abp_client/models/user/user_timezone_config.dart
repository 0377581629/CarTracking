class UserIanaTimeZoneConfig {
  String timeZoneId;
  UserIanaTimeZoneConfig(this.timeZoneId);
}

class UserWindowsTimeZoneConfig {
  String timeZoneId;
  double baseUtcOffsetInMilliseconds;
  double currentUtcOffsetInMilliseconds;
  bool isDaylightSavingTimeNow;
  UserWindowsTimeZoneConfig(this.timeZoneId, this.baseUtcOffsetInMilliseconds,
      this.currentUtcOffsetInMilliseconds, this.isDaylightSavingTimeNow);
}

class UserTimeZoneConfig {
  UserWindowsTimeZoneConfig windows;
  UserIanaTimeZoneConfig iana;
  UserTimeZoneConfig(this.windows, this.iana);
}

class UserTimmingConfig {
  UserTimeZoneConfig timeZoneInfo;
  UserTimmingConfig(this.timeZoneInfo);
}
