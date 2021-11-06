import 'package:aspnet_zero_app/abp/models/auth/authenticate_model.dart';
import 'package:aspnet_zero_app/abp/models/auth/authenticate_result_model.dart';
import 'package:aspnet_zero_app/abp/models/auth/forgot_password_model.dart';
import 'package:aspnet_zero_app/abp/models/auth/login_result.dart';
import 'package:aspnet_zero_app/abp/models/auth/register_model.dart';
import 'package:aspnet_zero_app/abp/models/auth/register_result.dart';
import 'package:aspnet_zero_app/abp/models/auth/reset_password_model.dart';
import 'package:aspnet_zero_app/abp/models/tenancy/is_tenancy_available.dart';

abstract class IAccountService {

  AuthenticateModel? authenticateModel;

  AuthenticateResultModel? authenticateResultModel;

  RegisterModel? registerModel;

  ResetPasswordModel? resetPasswordModel;

  ForgotPasswordModel? forgotPasswordModel;

  Future<LoginResultOutput> loginUser();

  Future logout();

  Future<RegisterResult> registerUser();

  Future resetPassword();

  Future forgotPassword();

  Future<IsTenantAvailableOutput> isTenantAvailable(IsTenantAvailableInput input);
}
