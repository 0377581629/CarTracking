import 'package:aspnet_zero_app/flutter_flow/flutter_flow_theme.dart';
import 'package:aspnet_zero_app/flutter_flow/flutter_flow_widgets.dart';

import 'package:aspnet_zero_app/ui/login/login_view.dart';
import 'package:aspnet_zero_app/helpers/localization_helper.dart';

import 'package:flutter/material.dart';
import 'package:smooth_page_indicator/smooth_page_indicator.dart';

final lang = LocalizationHelper();

class OnboardingPageWidget extends StatefulWidget {
  const OnboardingPageWidget({Key? key}) : super(key: key);

  @override
  _OnboardingPageWidgetState createState() => _OnboardingPageWidgetState();
}

class _OnboardingPageWidgetState extends State<OnboardingPageWidget> {
  PageController? pageViewController;
  bool _loadingButton = false;
  final scaffoldKey = GlobalKey<ScaffoldState>();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      key: scaffoldKey,
      backgroundColor: FlutterFlowTheme.primaryColor,
      body: SingleChildScrollView(
        child: Container(
          child: Column(
            mainAxisSize: MainAxisSize.max,
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Padding(
                padding: EdgeInsetsDirectional.fromSTEB(0, 40, 0, 0),
                child: Row(
                  mainAxisSize: MainAxisSize.max,
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    Image.asset(
                      'assets/images/trueinvest-logo.png',
                      width: 240,
                      height: 100,
                      fit: BoxFit.fitHeight,
                    )
                  ],
                ),
              ),
              Row(
                mainAxisSize: MainAxisSize.max,
                children: [
                  Expanded(
                    child: SizedBox(
                      width: double.infinity,
                      height: MediaQuery.of(context).size.height * 0.6,
                      child: Stack(
                        children: [
                          PageView(
                            controller: pageViewController ??=
                                PageController(initialPage: 0),
                            scrollDirection: Axis.horizontal,
                            children: [
                              Container(
                                width: 100,
                                height: 100,
                                decoration: BoxDecoration(),
                                child: Column(
                                  mainAxisSize: MainAxisSize.max,
                                  mainAxisAlignment: MainAxisAlignment.center,
                                  children: [
                                    Row(
                                      mainAxisSize: MainAxisSize.max,
                                      mainAxisAlignment: MainAxisAlignment.center,
                                      children: [
                                        Image.asset(
                                          'assets/images/onboarding_1.png',
                                          width: 350,
                                          height: 300,
                                          fit: BoxFit.fitWidth,
                                        )
                                      ],
                                    ),
                                    Expanded(
                                      child: Text(
                                        'Search for Books',
                                        textAlign: TextAlign.center,
                                        style:
                                        FlutterFlowTheme.title1.override(
                                          fontFamily: FlutterFlowTheme
                                              .defaultFontFamily,
                                          color: Colors.white,
                                          fontSize: 32,
                                          fontWeight: FontWeight.bold,
                                        ),
                                      ),
                                    ),
                                    Expanded(
                                      child: Text(
                                        'Find amazing classic books in our library.',
                                        textAlign: TextAlign.center,
                                        style: FlutterFlowTheme.subtitle2
                                            .override(
                                          fontFamily: FlutterFlowTheme
                                              .defaultFontFamily,
                                          color: Color(0x99FFFFFF),
                                          fontSize: 16,
                                          fontWeight: FontWeight.normal,
                                        ),
                                      ),
                                    )
                                  ],
                                ),
                              ),
                              Container(
                                width: 100,
                                height: 100,
                                decoration: BoxDecoration(),
                                child: Column(
                                  mainAxisSize: MainAxisSize.max,
                                  mainAxisAlignment: MainAxisAlignment.center,
                                  children: [
                                    Row(
                                      mainAxisSize: MainAxisSize.max,
                                      mainAxisAlignment: MainAxisAlignment.center,
                                      children: [
                                        Image.asset(
                                          'assets/images/onboarding_2.png',
                                          width: 300,
                                          height: 300,
                                          fit: BoxFit.fitWidth,
                                        )
                                      ],
                                    ),
                                    Expanded(
                                      child: Text(
                                        'Purchase Books',
                                        textAlign: TextAlign.center,
                                        style:
                                        FlutterFlowTheme.title1.override(
                                          fontFamily: FlutterFlowTheme
                                              .defaultFontFamily,
                                          color: Colors.white,
                                          fontSize: 32,
                                          fontWeight: FontWeight.bold,
                                        ),
                                      ),
                                    ),
                                    Expanded(
                                      child: Text(
                                        'Buy and view all your favorite books you find in this library.',
                                        textAlign: TextAlign.center,
                                        style: FlutterFlowTheme.subtitle2
                                            .override(
                                          fontFamily: FlutterFlowTheme
                                                  .defaultFontFamily,
                                          color: Color(0x99FFFFFF),
                                          fontSize: 16,
                                          fontWeight: FontWeight.normal,
                                        ),
                                      ),
                                    )
                                  ],
                                ),
                              ),
                              Container(
                                width: 100,
                                height: 100,
                                decoration: BoxDecoration(),
                                child: Column(
                                  mainAxisSize: MainAxisSize.max,
                                  mainAxisAlignment: MainAxisAlignment.center,
                                  children: [
                                    Row(
                                      mainAxisSize: MainAxisSize.max,
                                      mainAxisAlignment: MainAxisAlignment.center,
                                      children: [
                                        Image.asset(
                                          'assets/images/onboarding_3.png',
                                          width: 300,
                                          height: 250,
                                          fit: BoxFit.fitWidth,
                                        )
                                      ],
                                    ),
                                    Expanded(
                                      child: Text(
                                        'Review Purchases',
                                        textAlign: TextAlign.center,
                                        style:
                                        FlutterFlowTheme.title1.override(
                                          fontFamily: FlutterFlowTheme
                                              .defaultFontFamily,
                                          color: Colors.white,
                                          fontSize: 32,
                                          fontWeight: FontWeight.bold,
                                        ),
                                      ),
                                    ),
                                    Expanded(
                                      child: Text(
                                        'Keep track of all your purchases that you have made, want to trade books in? Go ahead and list them for exchange.',
                                        textAlign: TextAlign.center,
                                        style: FlutterFlowTheme.subtitle2
                                            .override(
                                          fontFamily: FlutterFlowTheme
                                              .defaultFontFamily,
                                          color: Color(0xFF82878C),
                                          fontSize: 16,
                                          fontWeight: FontWeight.normal,
                                        ),
                                      ),
                                    )
                                  ],
                                ),
                              )
                            ],
                          ),
                          Align(
                            alignment: AlignmentDirectional(0, 1),
                            child: Padding(
                              padding: EdgeInsetsDirectional.fromSTEB(0, 0,
                                  0, 0),
                              child: SmoothPageIndicator(
                                controller: pageViewController ??=
                                    PageController(initialPage: 0),
                                count: 3,
                                axisDirection: Axis.horizontal,
                                onDotClicked: (i) {
                                  pageViewController!.animateToPage(
                                    i,
                                    duration: Duration(milliseconds: 500),
                                    curve: Curves.ease,
                                  );
                                },
                                effect: ExpandingDotsEffect(
                                  expansionFactor: 2,
                                  spacing: 8,
                                  radius: 16,
                                  dotWidth: 16,
                                  dotHeight: 4,
                                  dotColor: Color(0x8AC6CAD4),
                                  activeDotColor: Colors.white,
                                  paintStyle: PaintingStyle.fill,
                                ),
                              ),
                            ),
                          ),
                        ],
                      ),
                    ),
                  )
                ],
              ),
              Padding(
                padding: const EdgeInsetsDirectional.fromSTEB(0, 10, 0, 0),
                child: Row(
                  mainAxisSize: MainAxisSize.max,
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    Padding(
                      padding: const EdgeInsetsDirectional.fromSTEB(0, 16, 0, 0),
                      child: FFButtonWidget(
                        onPressed: () async {
                          setState(() => _loadingButton = true);
                          try {
                            await Navigator.push(
                              context,
                              MaterialPageRoute(
                                builder: (context) => LoginPage(),
                              ),
                            );
                          } finally {
                            setState(() => _loadingButton = false);
                          }
                        },
                        text: lang.get('Skip'),
                        options: FFButtonOptions(
                          width: 200,
                          height: 50,
                          color: FlutterFlowTheme.secondaryColor,
                          textStyle: FlutterFlowTheme.subtitle1.override(
                            fontFamily: FlutterFlowTheme
                                .defaultFontFamily,
                            color: FlutterFlowTheme.primaryColor,
                            fontSize: 18,
                            fontWeight: FontWeight.w500,
                          ),
                          elevation: 2,
                          borderSide: const BorderSide(
                            color: Colors.transparent,
                            width: 1,
                          ),
                          borderRadius: 8,
                        ),
                        loading: _loadingButton,
                      ),
                    )
                  ],
                ),
              )
            ],
          ),
        ),
      )
      ,
    );
  }
}
