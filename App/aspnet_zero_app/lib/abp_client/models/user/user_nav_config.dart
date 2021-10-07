import 'package:aspnet_zero_app/abp_client/models/common/user_menu.dart';
import 'package:tuple/tuple.dart';

class UserNavConfig {
  Tuple2<String, UserMenu> menus;
  UserNavConfig(this.menus);
}
