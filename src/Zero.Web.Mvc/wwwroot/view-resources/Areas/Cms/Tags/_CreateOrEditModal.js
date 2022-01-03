(function ($) {
    app.modals.CreateOrEditTagsModal = function () {

        const _TagsService = abp.services.app.tags;

        let _modalManager;
        let _$TagsInformationForm = null;

        let modal;
        
        this.init = function (modalManager) {
            _modalManager = modalManager;
            modal = _modalManager.getModal();
            _modalManager.initControl();
            
            _$TagsInformationForm = _modalManager.getModal().find('form[name=TagsInformationsForm]');
            _$TagsInformationForm.validate();
        };
        
        this.save = function () {
            if (!_$TagsInformationForm.valid()) {
                return;
            }
            
            const Tags = _$TagsInformationForm.serializeFormToObject();
            
            _modalManager.setBusy(true);
            _TagsService.createOrEdit(
                Tags
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditTagsModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);