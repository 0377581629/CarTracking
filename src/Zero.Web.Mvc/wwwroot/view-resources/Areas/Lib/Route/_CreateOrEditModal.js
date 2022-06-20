(function ($) {
    app.modals.CreateOrEditRouteModal = function () {

        const _RouteService = abp.services.app.route;

        let _modalManager;
        let _$RouteInformationForm = null;

        let modal;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            modal = _modalManager.getModal();

            _modalManager.initControl();

            let managementUnitSelector = modal.find('#ManagementUnitId');
            baseHelper.SimpleSelector(managementUnitSelector, app.localize('NoneSelect'), 'Lib/GetPagedManagementUnits', true);

            _$RouteInformationForm = _modalManager.getModal().find('form[name=RouteInformationsForm]');
            _$RouteInformationForm.validate();
        };

        this.save = function () {
            if (!_$RouteInformationForm.valid()) {
                return;
            }

            const Route = _$RouteInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _RouteService.createOrEdit(
                Route
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditRouteModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);