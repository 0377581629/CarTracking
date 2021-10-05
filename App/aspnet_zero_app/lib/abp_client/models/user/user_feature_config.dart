import 'package:aspnet_zero_app/abp_client/models/common/string_value.dart';
import 'package:tuple/tuple.dart';

class UserFeatureConfig {
  Tuple2<String, StringValue> allFeatures;
  UserFeatureConfig(this.allFeatures);
}
