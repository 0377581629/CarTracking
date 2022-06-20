(function ($) {
    app.modals.CreateOrEditManagementUnitModal = function () {

        const _ManagementUnitService = abp.services.app.managementUnit;

        let _modalManager;
        let _$ManagementUnitInformationForm = null;

        let modal;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            modal = _modalManager.getModal();

            _modalManager.initControl();

            _$ManagementUnitInformationForm = _modalManager.getModal().find('form[name=ManagementUnitInformationsForm]');
            _$ManagementUnitInformationForm.validate();
        };

        this.save = function () {
            if (!_$ManagementUnitInformationForm.valid()) {
                return;
            }

            const ManagementUnit = _$ManagementUnitInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _ManagementUnitService.createOrEdit(
                ManagementUnit
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditManagementUnitModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);