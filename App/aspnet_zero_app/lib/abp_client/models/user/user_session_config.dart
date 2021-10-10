import 'package:aspnet_zero_app/abp_client/enums/multi_tenancy_sides.dart';
import 'package:json_annotation/json_annotation.dart';
part 'user_session_config.g.dart';

@JsonSerializable()
class UserSessionConfig {
  int? userId;
  int? tenantId;
  int? impersonatorUserId;
  int? impersonatorTenantId;
  MultiTenancySides multiTenancySide;
  UserSessionConfig(this.multiTenancySide);

  factory UserSessionConfig.fromJson(Map<String, dynamic> json) =>
      _$UserSessionConfigFromJson(json);

  Map<String, dynamic> toJson() => _$UserSessionConfigToJson(this);
}
