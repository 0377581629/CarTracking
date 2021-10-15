import 'package:json_annotation/json_annotation.dart';
part 'edition_info.g.dart';

@JsonSerializable()
class EditionInfo {
  int id;
  String displayName;

  int? trialDayCount;

  double? monthlyPrice;

  double? annualPrice;

  bool isHighestEdition;

  bool isFree;
  EditionInfo(this.id, this.displayName, this.trialDayCount, this.monthlyPrice,
      this.annualPrice, this.isHighestEdition, this.isFree);

  factory EditionInfo.fromJson(Map<String, dynamic> json) =>
      _$EditionInfoFromJson(json);

  Map<String, dynamic> toJson() => _$EditionInfoToJson(this);
}
