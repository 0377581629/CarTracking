import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';

import '../flutter_flow/flutter_flow_theme.dart';

class ErrorPageWidget extends StatefulWidget {
  String errorMessage;

  ErrorPageWidget({Key? key, required this.errorMessage}) : super(key: key);

  @override
  _ErrorPageWidgetState createState() => _ErrorPageWidgetState();
}

class _ErrorPageWidgetState extends State<ErrorPageWidget> {
  final scaffoldKey = GlobalKey<ScaffoldState>();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      key: scaffoldKey,
      backgroundColor: FlutterFlowTheme.primaryColor,
      body: SafeArea(
        child: Column(
          mainAxisSize: MainAxisSize.max,
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Row(
              mainAxisSize: MainAxisSize.max,
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Image.asset(
                  'assets/images/trueinvest-icon.png',
                  width: 150,
                  height: 170,
                  fit: BoxFit.cover,
                )
              ],
            ),
            Padding(
              padding: const EdgeInsetsDirectional.fromSTEB(0, 20, 0, 0),
              child: Text(
                widget.errorMessage,
                textAlign: TextAlign.center,
                style: FlutterFlowTheme.title1.override(
                  fontFamily: FlutterFlowTheme.defaultFontFamily,
                  color: FlutterFlowTheme.secondaryColor,
                ),
              ),
            )
          ],
        ),
      ),
    );
  }
}
