import 'package:aspnet_zero_app/abp_client/models/multi_tenancy/multi_tenancy_side_config.dart';

abstract class IMultiTenancyConfig {
  bool? isEnabled;
  bool? ignoreFeatureCheckForHostUsers;
  MultiTenancySideConfig? sides;
}
