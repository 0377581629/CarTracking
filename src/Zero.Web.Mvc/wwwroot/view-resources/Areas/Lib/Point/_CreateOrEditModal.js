(function ($) {
    app.modals.CreateOrEditPointModal = function () {

        const _PointService = abp.services.app.point;

        let _modalManager;
        let _$PointInformationForm = null;

        let modal;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            modal = _modalManager.getModal();

            _modalManager.initControl();

            let managementUnitSelector = modal.find('#ManagementUnitId');
            baseHelper.SimpleSelector(managementUnitSelector, app.localize('NoneSelect'), 'Lib/GetPagedManagementUnits', true);

            let pointTypeSelector = modal.find('#PointTypeId');
            baseHelper.SimpleSelector(pointTypeSelector, app.localize('NoneSelect'), 'Lib/GetPagedPointTypes', true);

            _$PointInformationForm = _modalManager.getModal().find('form[name=PointInformationsForm]');
            _$PointInformationForm.validate();
        };

        this.save = function () {
            if (!_$PointInformationForm.valid()) {
                return;
            }

            const Point = _$PointInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _PointService.createOrEdit(
                Point
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditPointModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);