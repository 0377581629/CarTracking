(function ($) {
    app.modals.CreateOrEditNetworkProviderModal = function () {

        const _NetworkProviderService = abp.services.app.networkProvider;

        let _modalManager;
        let _$NetworkProviderInformationForm = null;

        let modal;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            modal = _modalManager.getModal();

            _modalManager.initControl();

            _$NetworkProviderInformationForm = _modalManager.getModal().find('form[name=NetworkProviderInformationsForm]');
            _$NetworkProviderInformationForm.validate();
        };

        this.save = function () {
            if (!_$NetworkProviderInformationForm.valid()) {
                return;
            }

            const NetworkProvider = _$NetworkProviderInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _NetworkProviderService.createOrEdit(
                NetworkProvider
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditNetworkProviderModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);