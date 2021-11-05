import 'package:aspnet_zero_app/abp/models/auth/authenticate_model.dart';
import 'package:aspnet_zero_app/abp/models/auth/authenticate_result_model.dart';
import 'package:aspnet_zero_app/abp/models/auth/forgot_password_model.dart';
import 'package:aspnet_zero_app/abp/models/auth/login_result.dart';
import 'package:aspnet_zero_app/abp/models/auth/reset_password_model.dart';
import 'package:aspnet_zero_app/abp/models/tenancy/is_tenancy_available.dart';

abstract class IAccountService {
  AuthenticateModel? authenticateModel;
  AuthenticateResultModel? authenticateResultModel;

  ResetPasswordModel? resetPasswordModel;

  ForgotPasswordModel? forgotPasswordModel;

  Future<LoginResultOutput> loginUser();

  Future logout();

  Future resetPassword();

  Future forgotPassword();

  Future<IsTenantAvailableOutput> isTenantAvailable(IsTenantAvailableInput input);
}
