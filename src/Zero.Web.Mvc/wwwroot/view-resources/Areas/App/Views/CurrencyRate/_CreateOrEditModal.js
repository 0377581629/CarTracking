(function ($) {
    app.modals.CreateOrEditCurrencyRateModal = function () {

        const _CurrencyRatesService = abp.services.app.currencyRate;

        let _modalManager;
        let _$CurrencyRateInformationForm = null;

        let modal;
        
        this.init = function (modalManager) {
            _modalManager = modalManager;

            modal = _modalManager.getModal();
            
           _modalManager.initControl();
           
            
            _$CurrencyRateInformationForm = _modalManager.getModal().find('form[name=CurrencyRateInformationsForm]');
            _$CurrencyRateInformationForm.validate();
        };
        
        this.save = function () {
            if (!_$CurrencyRateInformationForm.valid()) {
                return;
            }
            
            const CurrencyRate = _$CurrencyRateInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);
            _CurrencyRatesService.createOrEdit(
                CurrencyRate
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditCurrencyRateModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);