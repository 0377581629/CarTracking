import 'package:aspnet_zero_app/utility/colorResources.dart';
import 'package:aspnet_zero_app/utility/style.dart';
import 'package:flutter/material.dart';

class CustomButton extends StatefulWidget {
  final String btnTxt;
  final bool isWhiteBackground;
  final Function onTap;
  const CustomButton(this.btnTxt, this.onTap,
      {Key? key, this.isWhiteBackground = false})
      : super(key: key);

  @override
  State<StatefulWidget> createState() => _CustomButtonState();
}

class _CustomButtonState extends State<CustomButton> {
  @override
  Widget build(BuildContext context) {
    return Container(
      height: 45,
      width: double.infinity,
      decoration: BoxDecoration(
          boxShadow: [
            BoxShadow(
              color: Colors.grey.withOpacity(0.2),
              spreadRadius: 1,
              blurRadius: 7,
              offset: const Offset(0, 1), // changes position of shadow
            ),
          ],
          color: widget.isWhiteBackground
              ? ColorResources.COLOR_WHITE
              : ColorResources.COLOR_PRIMARY,
          borderRadius: BorderRadius.circular(10)),
      child: TextButton(
        style: TextButton.styleFrom(
          padding: const EdgeInsets.all(0),
        ),
        onPressed: widget.onTap(),
        child: Text(widget.btnTxt,
            style: khulaSemiBold.copyWith(
                color: widget.isWhiteBackground
                    ? ColorResources.COLOR_PRIMARY
                    : ColorResources.COLOR_WHITE)),
      ),
    );
  }
}
