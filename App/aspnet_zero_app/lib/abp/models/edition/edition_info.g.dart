// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'edition_info.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

EditionInfo _$EditionInfoFromJson(Map<String, dynamic> json) => EditionInfo(
      json['id'] as int,
      json['displayName'] as String,
      json['trialDayCount'] as int?,
      (json['monthlyPrice'] as num?)?.toDouble(),
      (json['annualPrice'] as num?)?.toDouble(),
      json['isHighestEdition'] as bool,
      json['isFree'] as bool,
    );

Map<String, dynamic> _$EditionInfoToJson(EditionInfo instance) =>
    <String, dynamic>{
      'id': instance.id,
      'displayName': instance.displayName,
      'trialDayCount': instance.trialDayCount,
      'monthlyPrice': instance.monthlyPrice,
      'annualPrice': instance.annualPrice,
      'isHighestEdition': instance.isHighestEdition,
      'isFree': instance.isFree,
    };
