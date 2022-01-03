(function ($) {
    app.modals.CreateOrEditPageModal = function () {

        const _PagesService = abp.services.app.page;
        let _modalManager;
        let _$PageInformationForm = null;
        let modal;
        let pageLayoutSelector;
        
        this.init = function (modalManager) {
            _modalManager = modalManager;
            modal = _modalManager.getModal();
            _modalManager.initControl();
            pageLayoutSelector = modal.find('#Page_PageLayoutId');
            baseHelper.SimpleRequiredSelector(pageLayoutSelector, app.localize('PleaseSelect'), "/Cms/GetPagedPageLayouts");
            
            _$PageInformationForm = _modalManager.getModal().find('form[name=PageInformationsForm]');
            _$PageInformationForm.validate();
        };
        
        this.save = function () {
            let selectValid = _modalManager.validSelectors();
            if (!_$PageInformationForm.valid() || !selectValid) {
                return;
            }
            
            const Page = _$PageInformationForm.serializeFormToObject();
            
            _modalManager.setBusy(true);
            _PagesService.createOrEdit(
                Page
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditPageModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);