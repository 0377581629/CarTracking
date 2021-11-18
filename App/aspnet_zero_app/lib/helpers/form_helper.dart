import 'package:flutter/cupertino.dart';

class FormHelper {
  static final Map<String, GlobalKey<FormState>> _formKeys = <String, GlobalKey<FormState>>{};
  static GlobalKey<FormState> getKey(String formName) {
    if (_formKeys.containsKey(formName)) {
      return _formKeys[formName] as GlobalKey<FormState>;
    } else {
      _formKeys[formName] = GlobalKey<FormState>();
      print(_formKeys);
      return _formKeys[formName] as GlobalKey<FormState>;
    }
  }
}