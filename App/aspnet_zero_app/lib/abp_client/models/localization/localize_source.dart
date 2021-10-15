import 'package:json_annotation/json_annotation.dart';
part 'localize_source.g.dart';

@JsonSerializable()
class LocalizationSource {
  String? name;
  String? type;
  LocalizationSource({this.name, this.type});

  factory LocalizationSource.fromJson(Map<String, dynamic> json) =>
      _$LocalizationSourceFromJson(json);

  Map<String, dynamic> toJson() => _$LocalizationSourceToJson(this);
}
