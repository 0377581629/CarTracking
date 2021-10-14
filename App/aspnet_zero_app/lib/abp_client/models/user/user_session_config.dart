import 'package:json_annotation/json_annotation.dart';
part 'user_session_config.g.dart';

@JsonSerializable()
class UserSessionConfig {
  int? userId;
  int? tenantId;
  int? impersonatorUserId;
  int? impersonatorTenantId;
  int? multiTenancySide;
  UserSessionConfig(
      {this.userId,
      this.tenantId,
      this.impersonatorTenantId,
      this.impersonatorUserId,
      this.multiTenancySide});

  factory UserSessionConfig.fromJson(Map<String, dynamic> json) =>
      _$UserSessionConfigFromJson(json);

  Map<String, dynamic> toJson() => _$UserSessionConfigToJson(this);
}
