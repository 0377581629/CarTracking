import 'package:aspnet_zero_app/abp_client/interfaces/multi_tenancy_config.dart';
import 'package:aspnet_zero_app/abp_client/models/multi_tenancy/multi_tenancy_side_config.dart';

class MultiTenancyConfig implements IMultiTenancyConfig {
  @override
  bool? isEnabled;
  @override
  bool? ignoreFeatureCheckForHostUsers;
  @override
  MultiTenancySideConfig? sides;
}
