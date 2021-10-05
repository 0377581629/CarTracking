import 'package:aspnet_zero_app/abp_client/enums/multi_tenancy_sides.dart';

class MultiTenancySideConfig {
  MultiTenancySides host;
  MultiTenancySides tenant;
  MultiTenancySideConfig(this.host, this.tenant);
}
