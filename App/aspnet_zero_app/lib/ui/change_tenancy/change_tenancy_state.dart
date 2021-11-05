import 'package:aspnet_zero_app/abp/models/tenancy/is_tenancy_available.dart';
import 'package:aspnet_zero_app/abp/models/tenancy/tenancy_dto.dart';

import '../form_submission_status.dart';

class ChangeTenancyState {
  final String tenancyName;

  bool get isValidTenancyName => tenancyName.length > 3;

  final FormSubmissionStatus formStatus;

  IsTenantAvailableOutput? tenantResult;

  String? exceptionMessage;

  ChangeTenancyState({this.tenancyName = '', this.formStatus = const InitialFormStatus(), this.tenantResult, this.exceptionMessage});

  ChangeTenancyState copyWith({String? tenancyName, FormSubmissionStatus? formStatus, IsTenantAvailableOutput? tenantResult, String? exceptionMessage}) {
    return ChangeTenancyState(
        tenancyName: tenancyName ?? this.tenancyName,
        formStatus: formStatus ?? this.formStatus,
        tenantResult: tenantResult ?? this.tenantResult,
        exceptionMessage: exceptionMessage ?? this.exceptionMessage);
  }
}
