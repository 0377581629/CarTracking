import 'package:aspnet_zero_app/abp/models/multi_tenancy/multi_tenancy_config.dart';
import 'package:aspnet_zero_app/abp/models/user/user_auth_config.dart';
import 'package:aspnet_zero_app/abp/models/user/user_clock_config.dart';
import 'package:aspnet_zero_app/abp/models/user/user_feature_config.dart';
import 'package:aspnet_zero_app/abp/models/user/user_localize_config.dart';
import 'package:aspnet_zero_app/abp/models/user/user_nav_config.dart';
import 'package:aspnet_zero_app/abp/models/user/user_security_config.dart';
import 'package:aspnet_zero_app/abp/models/user/user_session_config.dart';
import 'package:aspnet_zero_app/abp/models/user/user_setting_config.dart';
import 'package:aspnet_zero_app/abp/models/user/user_timezone_config.dart';
import 'package:json_annotation/json_annotation.dart';
part 'user_configuration.g.dart';

@JsonSerializable(explicitToJson: true)
class UserConfiguration {
  MultiTenancyConfig? multiTenancy;

  UserSessionConfig? session;

  UserLocalizationConfig? localization;

  UserFeatureConfig? features;

  UserAuthConfig? auth;

  UserNavConfig? nav;

  UserSettingConfig? setting;

  UserClockConfig? clock;

  UserTimmingConfig? timing;

  UserSecurityConfig? security;

  Map<String, dynamic>? custom;

  UserConfiguration(
      {this.multiTenancy,
      this.session,
      this.localization,
      this.features,
      this.auth,
      this.nav,
      this.setting,
      this.clock,
      this.timing,
      this.security,
      this.custom});

  factory UserConfiguration.fromJson(Map<String, dynamic> json) =>
      _$UserConfigurationFromJson(json);

  Map<String, dynamic> toJson() => _$UserConfigurationToJson(this);
}
