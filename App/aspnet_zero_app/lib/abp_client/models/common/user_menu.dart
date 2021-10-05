class UserMenu {
  String name;

  String displayName;

  dynamic CustomData;

  List<UserMenuItem> items;

  UserMenu(this.name, this.displayName, this.items);
}

class UserMenuItem {
  String name;

  String icon;

  String displayName;

  int order;

  String url;

  dynamic customData;

  String target;

  bool isEnabled;

  bool isVisible;

  List<UserMenuItem> items;

  UserMenuItem(this.name, this.icon, this.displayName, this.order, this.url,
      this.customData, this.target, this.isEnabled, this.isVisible, this.items);
}
