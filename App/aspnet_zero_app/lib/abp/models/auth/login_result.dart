enum LoginResult {
  success,
  fail,
  needToChangePassword,
  requireTwoFactorVerification
}

class LoginResultOutput {
  LoginResult? result;
  String? exceptionMessage;
}