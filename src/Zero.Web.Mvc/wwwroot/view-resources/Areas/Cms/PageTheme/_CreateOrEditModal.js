(function ($) {
    app.modals.CreateOrEditPageThemeModal = function () {

        const _PageThemesService = abp.services.app.pageTheme;

        let _modalManager;
        let _$PageThemeInformationForm = null;

        let modal;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            modal = _modalManager.getModal();
            _modalManager.initControl();
            _$PageThemeInformationForm = _modalManager.getModal().find('form[name=PageThemeInformationsForm]');
            _$PageThemeInformationForm.validate();
        };

        this.save = function () {
            if (!_$PageThemeInformationForm.valid()) {
                return;
            }

            const PageTheme = _$PageThemeInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _PageThemesService.createOrEdit(
                PageTheme
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditPageThemeModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);