import 'package:tuple/tuple.dart';

class UserAuthConfig {
  Tuple2<String, String> allPermissions;
  Tuple2<String, String> grantedPermissions;
  UserAuthConfig(this.allPermissions, this.grantedPermissions);
}
