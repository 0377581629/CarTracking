import 'package:aspnet_zero_app/abp/abp_client/models/auth/authenticate_model.dart';
import 'package:aspnet_zero_app/abp/abp_client/models/auth/authenticate_result_model.dart';

abstract class IAccountService {
  AuthenticateModel? authenticateModel;
  AuthenticateResultModel? authenticateResultModel;
  Future loginUser();
  Future logout();
}
