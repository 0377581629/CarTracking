(function ($) {
    app.modals.CreateOrEditCarTypeModal = function () {

        const _CarTypeService = abp.services.app.carType;

        let _modalManager;
        let _$CarTypeInformationForm = null;

        let modal;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            modal = _modalManager.getModal();

            _modalManager.initControl();

            _$CarTypeInformationForm = _modalManager.getModal().find('form[name=CarTypeInformationsForm]');
            _$CarTypeInformationForm.validate();
        };

        this.save = function () {
            if (!_$CarTypeInformationForm.valid()) {
                return;
            }

            const CarType = _$CarTypeInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _CarTypeService.createOrEdit(
                CarType
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditCarTypeModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);