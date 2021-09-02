(function ($) {
    app.modals.CreateOrEditEmailTemplateModal = function () {

        const _EmailTemplatesService = abp.services.app.emailTemplate;

        let _modalManager;
        let _$EmailTemplateInformationForm = null;

        let modal;
        
        this.init = function (modalManager) {
            _modalManager = modalManager;

            modal = _modalManager.getModal();
            
            _modalManager.initControl();
            _modalManager.bigModal();
            
            new FroalaEditor('#EmailTemplate_Content', frEditorConfigSimple);
            
            _$EmailTemplateInformationForm = _modalManager.getModal().find('form[name=EmailTemplateInformationsForm]');
            _$EmailTemplateInformationForm.validate();
        };
        
        this.save = function () {
            if (!_$EmailTemplateInformationForm.valid()) {
                return;
            }
            
            const EmailTemplate = _$EmailTemplateInformationForm.serializeFormToObject();
            
            _modalManager.setBusy(true);
            _EmailTemplatesService.createOrEdit(
                EmailTemplate
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditEmailTemplateModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);