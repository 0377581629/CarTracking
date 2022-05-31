(function ($) {
    app.modals.CreateOrEditCarGroupModal = function () {

        const _CarGroupService = abp.services.app.carGroup;

        let _modalManager;
        let _$CarGroupInformationForm = null;

        let modal;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            modal = _modalManager.getModal();

            _modalManager.initControl();

            _$CarGroupInformationForm = _modalManager.getModal().find('form[name=CarGroupInformationsForm]');
            _$CarGroupInformationForm.validate();
        };

        this.save = function () {
            if (!_$CarGroupInformationForm.valid()) {
                return;
            }

            const CarGroup = _$CarGroupInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _CarGroupService.createOrEdit(
                CarGroup
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditCarGroupModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);