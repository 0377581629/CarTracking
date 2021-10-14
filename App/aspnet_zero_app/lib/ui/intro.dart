import 'package:aspnet_zero_app/helpers/localization_helper.dart';
import 'package:aspnet_zero_app/ui/login.dart';
import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';
import 'package:introduction_screen/introduction_screen.dart';

final lang = LocalizationHelper();

class IntroPage extends StatefulWidget {
  const IntroPage({Key? key}) : super(key: key);

  @override
  _IntroPageState createState() => _IntroPageState();
}

class _IntroPageState extends State<IntroPage> {
  final introKey = GlobalKey<IntroductionScreenState>();

  void _onIntroEnd(context) {
    Navigator.of(context).push(
      MaterialPageRoute(builder: (_) => LoginPage(title: lang.get('Login'))),
    );
  }

  Widget _buildImage(String assetName, [double width = 350]) {
    return Image(image: AssetImage('assets/images/$assetName'), width: width);
  }

  @override
  Widget build(BuildContext context) {
    const bodyStyle = TextStyle(fontSize: 19.0);

    const pageDecoration = PageDecoration(
      titleTextStyle: TextStyle(fontSize: 28.0, fontWeight: FontWeight.w700),
      bodyTextStyle: bodyStyle,
      descriptionPadding: EdgeInsets.fromLTRB(16.0, 0.0, 16.0, 16.0),
      pageColor: Colors.white,
      imagePadding: EdgeInsets.zero,
    );

    return IntroductionScreen(
      key: introKey,
      globalBackgroundColor: Colors.white,
      pages: [
        PageViewModel(
          title: lang.get("IntroPage_Title1"),
          body: lang.get("IntroPage_Body1"),
          image: _buildImage('img1.jpg'),
          decoration: pageDecoration,
        ),
        PageViewModel(
          title: lang.get("IntroPage_Title2"),
          body: lang.get("IntroPage_Body2"),
          image: _buildImage('img2.jpg'),
          decoration: pageDecoration,
        ),
        PageViewModel(
          title: lang.get("IntroPage_Title3"),
          body: lang.get("IntroPage_Body3"),
          image: _buildImage('img3.jpg'),
          decoration: pageDecoration,
        )
      ],
      onDone: () => _onIntroEnd(context),
      //onSkip: () => _onIntroEnd(context), // You can override onSkip callback
      showSkipButton: true,
      skipFlex: 0,
      nextFlex: 0,
      //rtl: true, // Display as right-to-left
      skip: Text(lang.get('Skip')),
      next: const Icon(Icons.arrow_forward),
      done: Text(lang.get('Done'),
          style: const TextStyle(fontWeight: FontWeight.w600)),
      curve: Curves.fastLinearToSlowEaseIn,
      controlsMargin: const EdgeInsets.all(16),
      controlsPadding: kIsWeb
          ? const EdgeInsets.all(12.0)
          : const EdgeInsets.fromLTRB(8.0, 4.0, 8.0, 4.0),
      dotsDecorator: const DotsDecorator(
        size: Size(10.0, 10.0),
        color: Color(0xFFBDBDBD),
        activeSize: Size(22.0, 10.0),
        activeShape: RoundedRectangleBorder(
          borderRadius: BorderRadius.all(Radius.circular(25.0)),
        ),
      ),
      dotsContainerDecorator: const ShapeDecoration(
        color: Colors.white,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.all(Radius.circular(8.0)),
        ),
      ),
    );
  }
}
