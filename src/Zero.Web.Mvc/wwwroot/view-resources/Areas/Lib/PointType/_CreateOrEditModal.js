(function ($) {
    app.modals.CreateOrEditPointTypeModal = function () {

        const _PointTypeService = abp.services.app.pointType;

        let _modalManager;
        let _$PointTypeInformationForm = null;

        let modal;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            modal = _modalManager.getModal();

            _modalManager.initControl();

            _$PointTypeInformationForm = _modalManager.getModal().find('form[name=PointTypeInformationsForm]');
            _$PointTypeInformationForm.validate();
        };

        this.save = function () {
            if (!_$PointTypeInformationForm.valid()) {
                return;
            }

            const PointType = _$PointTypeInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _PointTypeService.createOrEdit(
                PointType
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditPointTypeModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);