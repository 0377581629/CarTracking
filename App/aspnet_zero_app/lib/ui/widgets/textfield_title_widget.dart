import 'package:aspnet_zero_app/utility/colorResources.dart';
import 'package:aspnet_zero_app/utility/style.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

class TextFieldTitleWidget extends StatefulWidget {
  final String title;
  final String imageUrl;

  const TextFieldTitleWidget({Key? key, this.title = '', this.imageUrl = ''})
      : super(key: key);

  @override
  State<StatefulWidget> createState() => _TextFieldTitleWidgetState();
}

class _TextFieldTitleWidgetState extends State<TextFieldTitleWidget> {
  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.start,
      crossAxisAlignment: CrossAxisAlignment.center,
      children: [
        Image.asset(
          widget.imageUrl,
          width: 15,
          height: 15,
          //color: ColorResources.COLOR_GREY,
          fit: BoxFit.scaleDown,
        ),
        const SizedBox(
          width: 10,
        ),
        Container(
          margin: const EdgeInsets.only(top: 5),
          child: Text(
            widget.title,
            textAlign: TextAlign.center,
            style: khulaSemiBold.copyWith(
              color: ColorResources.COLOR_GREY,
            ),
          ),
        ),
      ],
    );
  }
}
