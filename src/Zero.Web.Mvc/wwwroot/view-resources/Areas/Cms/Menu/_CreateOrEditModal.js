(function ($) {
    app.modals.CreateOrEditMenuModal = function () {

        const _MenusService = abp.services.app.menu;

        let _modalManager;
        let _$MenuInformationForm = null;

        let modal;
        let menuGroupId;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            modal = _modalManager.getModal();
            _modalManager.initControl();
            
            menuGroupId = modal.find('#MenuGroupId');
            baseHelper.SimpleRequiredSelector(menuGroupId, app.localize('PleaseSelect'), "/Cms/GetPagedMenuGroups");

            _$MenuInformationForm = _modalManager.getModal().find('form[name=MenuInformationsForm]');
            _$MenuInformationForm.validate();
        };

        this.save = function () {

            let validSelector = _modalManager.validSelectors();
            if (!_$MenuInformationForm.valid() || !validSelector) {
                return;
            }

            const Menu = _$MenuInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _MenusService.createOrEditMenu(
                Menu
            ).done(function (result) {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.setResult(result);
                _modalManager.close();
                abp.event.trigger('app.createOrEditMenuModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);