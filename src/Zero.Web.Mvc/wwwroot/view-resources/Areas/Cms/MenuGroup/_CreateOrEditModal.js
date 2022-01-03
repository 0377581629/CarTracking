(function ($) {
    app.modals.CreateOrEditMenuGroupModal = function () {

        const _MenuGroupsService = abp.services.app.menuGroup;

        let _modalManager;
        let _$MenuGroupInformationForm = null;

        let modal;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            modal = _modalManager.getModal();
            _modalManager.initControl();
            _$MenuGroupInformationForm = _modalManager.getModal().find('form[name=MenuGroupInformationsForm]');
            _$MenuGroupInformationForm.validate();
        };

        this.save = function () {
            if (!_$MenuGroupInformationForm.valid()) {
                return;
            }

            const MenuGroup = _$MenuGroupInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _MenuGroupsService.createOrEdit(
                MenuGroup
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditMenuGroupModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);