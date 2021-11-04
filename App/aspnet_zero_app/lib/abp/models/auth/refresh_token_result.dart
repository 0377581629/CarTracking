import 'package:json_annotation/json_annotation.dart';
part 'refresh_token_result.g.dart';

@JsonSerializable(explicitToJson: true)
class RefreshTokenResult {
  String accessToken;
  String encryptedAccessToken;
  int expireInSeconds;
  RefreshTokenResult(
      this.accessToken, this.encryptedAccessToken, this.expireInSeconds);

  factory RefreshTokenResult.fromJson(Map<String, dynamic> json) =>
      _$RefreshTokenResultFromJson(json);

  Map<String, dynamic> toJson() => _$RefreshTokenResultToJson(this);
}
