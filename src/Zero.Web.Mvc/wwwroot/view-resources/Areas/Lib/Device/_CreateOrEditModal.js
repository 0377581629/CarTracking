(function ($) {
    app.modals.CreateOrEditDeviceModal = function () {

        const _DeviceService = abp.services.app.device;

        let _modalManager;
        let _$DeviceInformationForm = null;

        let modal;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            modal = _modalManager.getModal();

            _modalManager.initControl();

            let networkProviderSelector = modal.find('#NetworkProviderId');
            baseHelper.SimpleSelector(networkProviderSelector, app.localize('NoneSelect'), 'Lib/GetPagedNetworkProviders',true);

            _$DeviceInformationForm = _modalManager.getModal().find('form[name=DeviceInformationsForm]');
            _$DeviceInformationForm.validate();
        };

        this.save = function () {
            if (!_$DeviceInformationForm.valid()) {
                return;
            }

            const Device = _$DeviceInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _DeviceService.createOrEdit(
                Device
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditDeviceModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);