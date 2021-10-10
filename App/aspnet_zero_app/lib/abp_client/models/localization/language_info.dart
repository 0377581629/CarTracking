import 'package:json_annotation/json_annotation.dart';
part 'language_info.g.dart';

@JsonSerializable()
class LanguageInfo {
  String? name;
  String? displayName;
  String? icon;
  bool? isDefault;
  bool? isDisabled;
  bool? isRightToLeft;
  LanguageInfo(
      {this.name,
      this.displayName,
      this.icon,
      this.isDefault,
      this.isDisabled,
      this.isRightToLeft});

  factory LanguageInfo.fromJson(Map<String, dynamic> json) =>
      _$LanguageInfoFromJson(json);

  Map<String, dynamic> toJson() => _$LanguageInfoToJson(this);
}
