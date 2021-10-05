import 'package:aspnet_zero_app/abp_client/models/multi_tenancy/multi_tenancy_side_config.dart';

class MultiTenancyConfig {
  bool isEnabled;
  bool ignoreFeatureCheckForHostUsers;
  MultiTenancySideConfig sides;
  MultiTenancyConfig(
      this.isEnabled, this.ignoreFeatureCheckForHostUsers, this.sides);
}
