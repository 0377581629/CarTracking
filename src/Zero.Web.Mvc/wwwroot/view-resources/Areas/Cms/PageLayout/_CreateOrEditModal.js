(function ($) {
    app.modals.CreateOrEditPageLayoutModal = function () {

        const _PageLayoutsService = abp.services.app.pageLayout;

        let _modalManager;
        let _$PageLayoutInformationForm = null;

        let modal;
        let pageThemeId;
        
        this.init = function (modalManager) {
            _modalManager = modalManager;
            modal = _modalManager.getModal();
            _modalManager.initControl();
            
            pageThemeId = modal.find('#PageLayout_PageThemeId');
            if (pageThemeId) {
                baseHelper.SimpleSelector(pageThemeId, app.localize('NoneSelect'), "/Cms/GetPagedPageThemes")
            }
            
            _$PageLayoutInformationForm = _modalManager.getModal().find('form[name=PageLayoutInformationsForm]');
            _$PageLayoutInformationForm.validate();
        };
        
        this.save = function () {
            if (!_$PageLayoutInformationForm.valid()) {
                return;
            }
            
            const PageLayout = _$PageLayoutInformationForm.serializeFormToObject();
            
            _modalManager.setBusy(true);
            _PageLayoutsService.createOrEdit(
                PageLayout
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditPageLayoutModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);