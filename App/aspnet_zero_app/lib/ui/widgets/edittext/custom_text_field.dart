import 'package:aspnet_zero_app/helpers/localization_helper.dart';
import 'package:aspnet_zero_app/utility/colorResources.dart';
import 'package:aspnet_zero_app/utility/dimensions.dart';
import 'package:aspnet_zero_app/utility/style.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

final lang = LocalizationHelper();

extension EmailValidator on String {
  bool isValidEmail() {
    return RegExp(
            r'^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$')
        .hasMatch(this);
  }
}

class CustomTextField extends StatefulWidget {
  final TextEditingController? controller;
  final String? hintTxt;
  final TextInputType? textInputType;
  final int? maxLine;
  final FocusNode? focusNode;
  final FocusNode? nextNode;
  final TextInputAction? textInputAction;
  final bool? isPhoneNumber;
  final bool? isEmail;

  CustomTextField(
      {this.controller,
      this.hintTxt,
      this.textInputType,
      this.maxLine,
      this.focusNode,
      this.nextNode,
      this.textInputAction,
      this.isEmail = false,
      this.isPhoneNumber = false});

  @override
  State<StatefulWidget> createState() => _CustomTextFieldState();
}

class _CustomTextFieldState extends State<CustomTextField> {
  @override
  Widget build(context) {
    return SizedBox(
      width: double.infinity,
      child: Stack(
        clipBehavior: Clip.none,
        children: [
          TextFormField(
            controller: widget.controller,
            maxLines: widget.maxLine ?? 1,
            focusNode: widget.focusNode,
            keyboardType: widget.textInputType ?? TextInputType.text,
            initialValue: null,
            textInputAction: widget.textInputAction ?? TextInputAction.next,
            onFieldSubmitted: (v) {
              FocusScope.of(context).requestFocus(widget.nextNode);
            },
            inputFormatters: [
              widget.isPhoneNumber!
                  ? FilteringTextInputFormatter.digitsOnly
                  : FilteringTextInputFormatter.singleLineFormatter
            ],
            validator: (input) => widget.isEmail!
                ? input!.isValidEmail()
                    ? null
                    : lang.get('PleaseProvideAValidEmail')
                : null,
            decoration: InputDecoration(
              hintText: widget.hintTxt ?? '',
              contentPadding:
                  const EdgeInsets.symmetric(vertical: 10.0, horizontal: 5),
              isDense: true,
              hintStyle: khulaRegular.copyWith(
                  color: ColorResources.COLOR_GREY,
                  fontSize: Dimensions.FONT_SIZE_SMALL),
              errorStyle: const TextStyle(height: 1.5),
              border: InputBorder.none,
              enabledBorder: InputBorder.none,
            ),
          ),
          Positioned(
            bottom: 5,
            left: 0,
            right: 0,
            child: Container(
              height: 1,
              color: ColorResources.COLOR_GAINSBORO,
            ),
          ),
        ],
      ),
    );
  }
}
