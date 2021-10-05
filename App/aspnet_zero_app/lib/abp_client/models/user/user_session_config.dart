import 'package:aspnet_zero_app/abp_client/enums/multi_tenancy_sides.dart';

class UserSessionConfig {
  int? userId;
  int? tenantId;
  int? impersonatorUserId;
  int? impersonatorTenantId;
  MultiTenancySides multiTenancySide;
  UserSessionConfig(this.multiTenancySide);
}
