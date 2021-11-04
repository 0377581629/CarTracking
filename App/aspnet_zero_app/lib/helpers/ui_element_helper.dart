import 'package:flutter/material.dart';

class UIHelper {
  static Widget appLogo({double? width = 240, double? height = 70}){
    return Image.asset(
      'assets/images/trueinvest-logo.png',
      width: width!,
      height: height!,
      fit: BoxFit.contain,
    );
  }
}