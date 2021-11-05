import 'package:aspnet_zero_app/abp/interfaces/application_context.dart';
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
