import 'package:aspnet_zero_app/abp/enums/tenant_availability_state.dart';
import 'package:aspnet_zero_app/abp/interfaces/account_service.dart';
import 'package:aspnet_zero_app/abp/manager/interfaces/application_context.dart';
import 'package:aspnet_zero_app/abp/interfaces/data_storage_service.dart';
import 'package:aspnet_zero_app/abp/models/common/ajax_response.dart';
import 'package:aspnet_zero_app/abp/models/multi_tenancy/tenant_information.dart';
import 'package:aspnet_zero_app/abp/models/tenancy/is_tenancy_available.dart';
import 'package:aspnet_zero_app/configuration/abp_config.dart';
import 'package:aspnet_zero_app/helpers/localization_helper.dart';
import 'package:aspnet_zero_app/ui/form_submission_status.dart';
import 'package:dio/dio.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:get_it/get_it.dart';

import 'change_tenancy_event.dart';
import 'change_tenancy_state.dart';

final lang = LocalizationHelper();
final getIt = GetIt.I;

class ChangeTenancyBloc extends Bloc<ChangeTenancyEvent, ChangeTenancyState> {
  IAccountService accountService;

  ChangeTenancyBloc({required this.accountService}) : super(ChangeTenancyState());

  @override
  Stream<ChangeTenancyState> mapEventToState(ChangeTenancyEvent event) async* {
    if (event is ChangeTenancyTenancyNameChanged) {
      yield state.copyWith(tenancyName: event.tenancyName);
    } else if (event is ChangeTenancySubmitted) {
      yield state.copyWith(formStatus: FormSubmitting());
      try {
        var tenant = await accountService.isTenantAvailable(IsTenantAvailableInput(state.tenancyName));
        var tenantState = TenantAvailabilityState.values[tenant.state!];
        switch (tenantState) {
          case TenantAvailabilityState.unknown:
            yield state.copyWith(formStatus: SubmissionFailed(Exception(lang.get('UnknownError'))), tenantResult: tenant);
            break;
          case TenantAvailabilityState.available:
            yield state.copyWith(formStatus: SubmissionSuccess(), tenantResult: tenant);
            var appContext = getIt.get<IApplicationContext>();
            var dataStorage = getIt.get<IDataStorageService>();
            appContext.setAsTenant(state.tenantResult!.tenantId!, state.tenancyName);
            if (!state.tenantResult!.serverRootAddress!.contains('localhost')) {
              AbpConfig.hostUrl = state.tenantResult!.serverRootAddress!;
            }
            dataStorage.storeTenantInfo(TenantInformation(state.tenantResult!.tenantId!, state.tenancyName));
            break;
          case TenantAvailabilityState.inActive:
            yield state.copyWith(formStatus: SubmissionFailed(Exception(lang.get('TenantIsNotActive'))), tenantResult: tenant);
            break;
          case TenantAvailabilityState.notFound:
            yield state.copyWith(formStatus: SubmissionFailed(Exception(lang.get('NotFoundTenancy') + ' : ' + state.tenancyName)), tenantResult: tenant);
            break;
        }
        yield state.copyWith(formStatus: const InitialFormStatus());
      } on DioError catch (e) {
        if (e.response != null && e.response!.data is Map<String, dynamic>) {
          var simpleResponse = SimpleAjaxResponse.fromJson(e.response!.data);
          if (simpleResponse.errorInfo != null) {
            yield state.copyWith(
                formStatus: SubmissionFailed(e),
                exceptionMessage: simpleResponse.errorInfo!.message);
            return;
          }
        }
        yield state.copyWith(
            formStatus: SubmissionFailed(e));
      } catch (e) {
        yield state.copyWith(
            formStatus: SubmissionFailed(e as Exception));
      }
    }
  }
}
