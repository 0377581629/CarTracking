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
}
