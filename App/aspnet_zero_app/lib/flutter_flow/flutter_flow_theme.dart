import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

// ignore: avoid_classes_with_only_static_members
class FlutterFlowTheme {
  static const Color primaryColor = Color(0xFF152E06);
  static const Color secondaryColor = Color(0xFFF2D884);
  static const Color tertiaryColor = Color(0xFFFFFFFF);

  static const Color lightMessColor = Color(0xFF212121);
  static const Color lightMessBackgroundColor = Color(0xFFCBCBCB);

  static const Color darkMessColor = Color(0xFFFFFFFF);
  static const Color darkMessBackgroundColor = Color(0xFF939393);

  static const Color infoMessColor = Color(0xFF212121);
  static const Color infoMessBackgroundColor = Color(0xFFB630B6);

  static const Color successMessColor = Color(0xFF212121);
  static const Color successMessBackgroundColor = Color(0xFF3AD90E);

  static const Color warningMessColor = Color(0xFF212121);
  static const Color warningMessBackgroundColor = Color(0xFFE1A52C);

  static const Color errorMessColor = Color(0xFF212121);
  static const Color errorMessBackgroundColor = Color(0xFFF64E60);

  static const String defaultFontFamily = 'Lexend Deca';
  static const EdgeInsetsDirectional formFieldContentPadding = EdgeInsetsDirectional.fromSTEB(10, 10, 10, 10);

  String primaryFontFamily = 'Poppins';
  String secondaryFontFamily = 'Roboto';


  static TextStyle get title1 => GoogleFonts.getFont(
        'Poppins',
        color: const Color(0xFF303030),
        fontWeight: FontWeight.w600,
        fontSize: 24,
      );

  static TextStyle get title2 => GoogleFonts.getFont(
        'Poppins',
        color: const Color(0xFF303030),
        fontWeight: FontWeight.w500,
        fontSize: 22,
      );

  static TextStyle get title3 => GoogleFonts.getFont(
        'Poppins',
        color: const Color(0xFF303030),
        fontWeight: FontWeight.w500,
        fontSize: 20,
      );

  static TextStyle get subtitle1 => GoogleFonts.getFont(
        'Poppins',
        color: const Color(0xFF757575),
        fontWeight: FontWeight.w500,
        fontSize: 18,
      );

  static TextStyle get subtitle2 => GoogleFonts.getFont(
        'Poppins',
        color: const Color(0xFF616161),
        fontWeight: FontWeight.normal,
        fontSize: 16,
      );

  static TextStyle get bodyText1 => GoogleFonts.getFont(
        'Poppins',
        color: const Color(0xFF303030),
        fontWeight: FontWeight.normal,
        fontSize: 14,
      );

  static TextStyle get bodyText2 => GoogleFonts.getFont(
        'Poppins',
        color: const Color(0xFF424242),
        fontWeight: FontWeight.normal,
        fontSize: 14,
      );
}

extension TextStyleHelper on TextStyle {
  TextStyle override({
    String? fontFamily,
    Color? color,
    double? fontSize,
    FontWeight? fontWeight,
    FontStyle? fontStyle,
    bool useGoogleFonts = true,
  }) =>
      useGoogleFonts
          ? GoogleFonts.getFont(
              fontFamily!,
              color: color ?? this.color,
              fontSize: fontSize ?? this.fontSize,
              fontWeight: fontWeight ?? this.fontWeight,
              fontStyle: fontStyle ?? this.fontStyle,
            )
          : copyWith(
              fontFamily: fontFamily,
              color: color,
              fontSize: fontSize,
              fontWeight: fontWeight,
              fontStyle: fontStyle,
            );
}
