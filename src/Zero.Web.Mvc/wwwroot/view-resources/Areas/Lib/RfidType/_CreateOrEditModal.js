(function ($) {
    app.modals.CreateOrEditRfidTypeModal = function () {

        const _RfidTypeService = abp.services.app.rfidType;

        let _modalManager;
        let _$RfidTypeInformationForm = null;

        let modal;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            modal = _modalManager.getModal();

            _modalManager.initControl();

            _$RfidTypeInformationForm = _modalManager.getModal().find('form[name=RfidTypeInformationsForm]');
            _$RfidTypeInformationForm.validate();
        };

        this.save = function () {
            if (!_$RfidTypeInformationForm.valid()) {
                return;
            }

            const RfidType = _$RfidTypeInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _RfidTypeService.createOrEdit(
                RfidType
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditRfidTypeModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);