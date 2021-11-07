import 'package:aspnet_zero_app/abp/manager/interfaces/application_context.dart';
import 'package:aspnet_zero_app/configuration/abp_config.dart';
import 'package:aspnet_zero_app/flutter_flow/flutter_flow_theme.dart';
import 'package:flutter/material.dart';
import 'package:get_it/get_it.dart';

import 'localization_helper.dart';

final getIt = GetIt.I;
final lang = LocalizationHelper();

class UIHelper {
  static Widget appLogo({double? width = 240, double? height = 70}) {
    return Image.asset(
      'assets/images/trueinvest-logo.png',
      width: width!,
      height: height!,
      fit: BoxFit.contain,
    );
  }

  static void showSnackbar(BuildContext context, String message, {String? messType = 'info'}) {
    switch (messType) {
      case 'light':
        final snackBar = SnackBar(
          content: Text(
            message,
            style: const TextStyle(color: FlutterFlowTheme.lightMessColor),
            textAlign: TextAlign.center,
          ),
          duration: const Duration(seconds: 3),
          behavior: SnackBarBehavior.floating,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(10),
          ),
          backgroundColor: FlutterFlowTheme.lightMessBackgroundColor,
        );
        ScaffoldMessenger.of(context).showSnackBar(snackBar);
        break;
      case 'dark':
        final snackBar = SnackBar(
          content: Text(
            message,
            style: const TextStyle(color: FlutterFlowTheme.darkMessColor),
            textAlign: TextAlign.center,
          ),
          duration: const Duration(seconds: 3),
          behavior: SnackBarBehavior.floating,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(10),
          ),
          backgroundColor: FlutterFlowTheme.darkMessBackgroundColor,
        );
        ScaffoldMessenger.of(context).showSnackBar(snackBar);
        break;
      case 'info':
        final snackBar = SnackBar(
          content: Text(
            message,
            style: const TextStyle(color: FlutterFlowTheme.infoMessColor),
            textAlign: TextAlign.center,
          ),
          duration: const Duration(seconds: 3),
          behavior: SnackBarBehavior.floating,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(10),
          ),
          backgroundColor: FlutterFlowTheme.infoMessBackgroundColor,
        );
        ScaffoldMessenger.of(context).showSnackBar(snackBar);
        break;
      case 'success':
        final snackBar = SnackBar(
          content: Text(
            message,
            style: const TextStyle(color: FlutterFlowTheme.successMessColor),
            textAlign: TextAlign.center,
          ),
          duration: const Duration(seconds: 3),
          behavior: SnackBarBehavior.floating,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(10),
          ),
          backgroundColor: FlutterFlowTheme.successMessBackgroundColor,
        );
        ScaffoldMessenger.of(context).showSnackBar(snackBar);
        break;
      case 'warning':
        final snackBar = SnackBar(
          content: Text(
            message,
            style: const TextStyle(color: FlutterFlowTheme.warningMessColor),
            textAlign: TextAlign.center,
          ),
          duration: const Duration(seconds: 3),
          behavior: SnackBarBehavior.floating,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(10),
          ),
          backgroundColor: FlutterFlowTheme.warningMessBackgroundColor,
        );
        ScaffoldMessenger.of(context).showSnackBar(snackBar);
        break;
      case 'error':
        final snackBar = SnackBar(
          content: Text(
            message,
            style: const TextStyle(color: FlutterFlowTheme.errorMessColor),
            textAlign: TextAlign.center,
          ),
          duration: const Duration(seconds: 3),
          behavior: SnackBarBehavior.floating,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(10),
          ),
          backgroundColor: FlutterFlowTheme.errorMessBackgroundColor,
        );
        ScaffoldMessenger.of(context).showSnackBar(snackBar);
        break;
    }
  }
}

class CurrentTenancy extends StatelessWidget {
  var appContext = getIt.get<IApplicationContext>();
  final Function(dynamic) callback;

  CurrentTenancy(this.callback, {Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    if (appContext.configuration!.multiTenancy!.isEnabled == true && !AbpConfig.fixedMultiTenant) {
      return Row(mainAxisSize: MainAxisSize.max, mainAxisAlignment: MainAxisAlignment.center, children: [
        Column(
          mainAxisSize: MainAxisSize.max,
          mainAxisAlignment: MainAxisAlignment.start,
          children: [
            Row(
              mainAxisSize: MainAxisSize.max,
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Text(
                  lang.get('MB_CurrentTenancy') + ':',
                  style: FlutterFlowTheme.bodyText1.override(
                    fontFamily: FlutterFlowTheme.defaultFontFamily,
                    color: FlutterFlowTheme.secondaryColor,
                  ),
                )
              ],
            ),
            Row(
              mainAxisSize: MainAxisSize.max,
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                InkWell(
                  onTap: () => callback(context),
                  child: Text(appContext.currentTenant?.tenancyName ?? lang.get('MB_System'),
                      style: FlutterFlowTheme.bodyText1.override(
                        fontFamily: FlutterFlowTheme.defaultFontFamily,
                        color: FlutterFlowTheme.secondaryColor,
                      )),
                )
              ],
            )
          ],
        )
      ]);
    }
    return Row();
  }
}
