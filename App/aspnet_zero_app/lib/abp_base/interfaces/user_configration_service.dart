import 'package:aspnet_zero_app/abp_client/models/user/user_configuration.dart';

abstract class IUserConfigurationService {
  Future<UserConfiguration> getUserConfiguration();
}
